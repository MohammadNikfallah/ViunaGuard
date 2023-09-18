global using ViunaGuard.Data;
global using ViunaGuard.Services;
global using ViunaGuard.Models;
global using ViunaGuard.Dtos;
global using ViunaGuard.Models.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.HttpLogging;
using NuGet.Common;

var builder = WebApplication.CreateBuilder(args);

string responseString = "";
string oauthBaseUrl = builder.Configuration.GetValue<string>("Constants:OauthBaseUrl")!;

try
{
    var client = new HttpClient();
    var request = new HttpRequestMessage()
    {
        RequestUri = new Uri($"{oauthBaseUrl}api/OAuth/PublicKey"),
        Method = HttpMethod.Get
    };
    var response = await client.SendAsync(request);
    responseString = await response.Content.ReadAsStringAsync();
}
catch (Exception e)
{
    File.AppendAllText("log",DateTime.Now +": couldn't grab key: " + e.Message + "\n");
}


builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.RequestHeaders.Add("sec-ch-ua-mobile");
    logging.RequestHeaders.Add("sec-ch-ua-platform");
    logging.RequestHeaders.Add("sec-fetch-site");
    logging.RequestHeaders.Add("sec-fetch-mode");
    logging.RequestHeaders.Add("sec-fetch-dest");
    logging.RequestHeaders.Add("sec-fetch-user");
    logging.RequestHeaders.Add("Cookie");
    logging.RequestHeaders.Add("Authorization");
    logging.ResponseHeaders.Add("Set-Cookie");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IGuardService, GuardService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IRefreshTokenHandler, RefreshTokenHandlerClass>();

builder.Services.AddAuthentication()
    .AddCookie("Cookie", options =>
    {
        options.Cookie.Name = "ClientCookie";
        options.ExpireTimeSpan = TimeSpan.Zero;
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.MaxAge = options.ExpireTimeSpan;
        options.SlidingExpiration = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    })
    .AddJwtBearer("JwtBearer", options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            RequireExpirationTime = true,
            ValidIssuer = "https://localhost:7120",
            ValidAudience = "12345",
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        options.Configuration = new OpenIdConnectConfiguration()
        {
            SigningKeys = { JsonWebKey.Create(responseString) }
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = ctx =>
            {
                if (ctx.Token == null) {
                    if (ctx.Request.Cookies.ContainsKey("VAT"))
                        ctx.Token = ctx.Request.Cookies["VAT"];
                    else if (ctx.Request.Cookies.ContainsKey("VRT"))
                        ctx.Token = ctx.Request.Cookies["VRT"];
                }

                return Task.CompletedTask;
            },
        };
    })
    .AddOAuth("OAuth", o =>
    {
        o.SignInScheme = "Cookie";
        o.ClientId = "12345";
        o.ClientSecret = "secretTest";
        o.SaveTokens = true;

        o.AuthorizationEndpoint = $"{oauthBaseUrl}api/OAuth/authorize";
        o.TokenEndpoint = $"{oauthBaseUrl}api/OAuth/token";
        o.CallbackPath = "/Auth/custom_cb";
        o.Events.OnTicketReceived = async x =>
        {
            // x.ReturnUri = "http://192.168.0.112:7123/";
            var tokens = x.Properties.GetTokens();
            var access = tokens.First(t => t.Name == "access_token").Value;
            var refresh = tokens.First(t => t.Name == "access_token").Value;
            x.Response.Headers.Add("Access-Token", access);
            x.Response.Headers.Add("Refresh-Token", refresh);

            using MemoryStream stream = new MemoryStream();
            var jsonObject = new Tokens()
            {
                AccessToken = access,
                RefreshToken = refresh
            };
            await JsonSerializer.SerializeAsync(stream, jsonObject, typeof(Tokens));
            var readOnlyMemory = new ReadOnlyMemory<byte>(stream.ToArray());
            
            x.Response.StatusCode = 200;
            
            x.Response.ContentType = "application/json; charset=utf-8";
            x.Response.ContentLength = stream.ToArray().Length;
            
            await x.Response.Body.WriteAsync(readOnlyMemory);
            await x.Response.Body.FlushAsync();
            
        };

        o.UsePkce = true;
        o.Events.OnCreatingTicket = async x =>
        {
            var payloadBase64 = x.AccessToken!.Split(".")[1];
            var payloadJson = Base64UrlTextEncoder.Decode(payloadBase64);
            var payload = JsonDocument.Parse(payloadJson);
            x.RunClaimActions(payload.RootElement);
            
            // x.Response.Headers.Add("Refresh-token",x.RefreshToken);
            // x.Response.Headers.Add("Access-token",x.AccessToken);
            
            // Serialize using the settings provided
            // using MemoryStream stream = new MemoryStream();
            // var jsonObject = new Tokens()
            // {
            //     AccessToken = x.AccessToken,
            //     RefreshToken = x.RefreshToken
            // };
            // await JsonSerializer.SerializeAsync(stream, jsonObject, typeof(Tokens));
            // ReadOnlyMemory<byte> readOnlyMemory = new ReadOnlyMemory<byte>(stream.ToArray());
            //
            // x.Response.StatusCode = 200;
            //
            // x.Response.ContentType = "application/json; charset=utf-8";
            //
            // await x.Response.Body.WriteAsync(readOnlyMemory);
            // await x.Response.Body.FlushAsync();
            
            x.Response.Cookies.Append("VAT", x.AccessToken, new CookieOptions
            {
                Expires = DateTime.Now.AddHours(1),
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = true
            });
            x.Response.Cookies.Append("VRT", x.RefreshToken!, new CookieOptions
            {
                Path = "/Auth/RefreshToken",
                Expires = DateTime.Now.AddDays(30),
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = true
            });
        };
    });

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault;
    });

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddAuthorization(o =>
{
    o.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes("JwtBearer")
        .RequireClaim("Type", "AccessToken")
        .Build();
    
    o.AddPolicy("RefreshToken",new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes("JwtBearer")
        .RequireClaim("Type", "RefreshToken")
        .Build());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHttpLogging();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/test", () =>
{
    
    return Task.CompletedTask;
});

app.Run();

class Tokens
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}