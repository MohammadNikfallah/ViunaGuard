global using ViunaGuard.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using ViunaGuard.Services;

var builder = WebApplication.CreateBuilder(args);

var client = new HttpClient();
var request = new HttpRequestMessage()
{
    RequestUri = new Uri("https://localhost:7120/api/OAuth/PublicKey"),
    Method = HttpMethod.Get
};
var response = await client.SendAsync(request);
var responseString = await response.Content.ReadAsStringAsync();

builder.Services.AddScoped<IGuardService, GuardService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddAuthentication()
    .AddCookie("Cookie", options =>
    {
        options.Cookie.Name = "ClientCookie";
        options.ExpireTimeSpan = TimeSpan.Zero;
        options.Cookie.MaxAge = options.ExpireTimeSpan;
        options.SlidingExpiration = true;
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
                if (ctx != null)
                {
                    if (ctx.Request.Cookies.ContainsKey("VAT") && ctx.Token == null)
                    {
                        ctx.Token = ctx.Request.Cookies["VAT"];
                    }

                    if (ctx.Token != null)
                    {
                        ctx.Response.Cookies.Append("VAT", ctx.Token);
                    }
                }
                return Task.CompletedTask;
            },

            OnAuthenticationFailed = async ctx =>
            {
                var refreshToken = ctx.Request.Cookies.FirstOrDefault(x => x.Key == "VRT").Value;
                if (ctx.Exception.GetType() == typeof(SecurityTokenExpiredException) && refreshToken != null)
                {
                    File.AppendAllText("log", DateTime.Now.ToString() + " : refreshing the access token\n");
                    var request = new RTRequest()
                    {
                        ClientId = "12345",
                        ClientSecret = "secretTest",
                        RefreshToken = refreshToken,
                        TokenEndpoint = "https://localhost:7120/api/OAuth/token"
                    };

                    var tokens = await RefreshTokenHandlerClass.RefreshTokenHandler(request);


                    if (tokens != null)
                    {
                        var accessToken = tokens.RootElement.GetString("access_token");
                        ctx.Response.Cookies.Append("VAT", accessToken!);
                        ctx.Response.Cookies.Append("VRT", tokens.RootElement.GetString("refresh_token")!);

                        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);

                        ctx.Principal = new ClaimsPrincipal(
                            new ClaimsIdentity(
                                jwt.Claims.Select(x => new Claim(x.Type, x.Value)).ToList(),
                                "Cookie"
                                ));
                    }
                    ctx.Success();
                }
            }
        };
    })
    .AddOAuth("OAuth", o =>
    {
        o.SignInScheme = "Cookie";

        o.ClientId = "12345";
        o.ClientSecret = "secretTest";

        o.AuthorizationEndpoint = "https://localhost:7120/api/OAuth/authorize";
        o.TokenEndpoint = "https://localhost:7120/api/OAuth/token";
        o.CallbackPath = "/api/custom_cb";

        o.UsePkce = true;
        o.ClaimActions.MapJsonKey("sub", "sub");
        o.ClaimActions.MapJsonKey("Id", "Id");
        o.Events.OnCreatingTicket = x =>
        {
            var payloadBase64 = x.AccessToken!.Split(".")[1];
            var payloadJson = Base64UrlTextEncoder.Decode(payloadBase64);
            var payload = JsonDocument.Parse(payloadJson);
            x.RunClaimActions(payload.RootElement); ;
            x.Response.Cookies.Append("VAT", x.AccessToken);
            x.Response.Cookies.Append("VRT", x.RefreshToken!);
            x.Response.Cookies.Delete("ClientCookie");

            return Task.CompletedTask;
        };
    });

builder.Services.AddControllers()
    //.AddNewtonsoftJson(options =>
    //options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
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
        .Build();
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();