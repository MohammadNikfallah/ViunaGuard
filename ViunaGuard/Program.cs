global using ViunaGuard.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using ViunaGuard.Services;

var builder = WebApplication.CreateBuilder(args);

string responseString = "";

try
{
    var client = new HttpClient();
    var request = new HttpRequestMessage()
    {
        RequestUri = new Uri("https://localhost:7120/api/OAuth/PublicKey"),
        Method = HttpMethod.Get
    };
    var response = await client.SendAsync(request);
    responseString = await response.Content.ReadAsStringAsync();
}
catch (Exception e)
{
    File.AppendAllText("log","couldn't grab key: " + e.Message + "\n");
}

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IGuardService, GuardService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddAuthentication()
    .AddCookie("RoleCookie", options =>
    {
        options.Cookie.Name = "RC";
    })
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
            OnMessageReceived = async ctx =>
            {
                if (ctx.Token == null) {
                    if (ctx.Request.Cookies.ContainsKey("VAT"))
                        ctx.Token = ctx.Request.Cookies["VAT"];
                    else if (ctx.Request.Cookies.ContainsKey("VRT"))
                    {
                        var tokens = await AccessRefresh(ctx.Request.Cookies["VRT"]!);
                        if (tokens != null)
                        {
                            var accessToken = tokens.RootElement.GetString("access_token");
                            ctx.Response.Cookies.Append("VAT", accessToken!, new CookieOptions
                            {
                                Expires = DateTime.Now.AddMinutes(10),
                                HttpOnly = true,
                                SameSite = SameSiteMode.Strict,
                                Secure = true
                            });
                            ctx.Response.Cookies.Append("VRT", tokens.RootElement.GetString("refresh_token")!, new CookieOptions
                            {
                                Expires = DateTime.Now.AddDays(30),
                                HttpOnly = true,
                                SameSite = SameSiteMode.Strict,
                                Secure = true
                            });
                            ctx.Token = accessToken;
                        }
                    }
                    else
                    {
                        ctx.Response.Cookies.Delete("RC");
                    }
                }
            },

            OnAuthenticationFailed = async ctx =>
            {
                var refreshToken = ctx.Request.Cookies.FirstOrDefault(x => x.Key == "VRT").Value;
                if (ctx.Exception.GetType() == typeof(SecurityTokenExpiredException) && refreshToken != null)
                {
                    var tokens = await AccessRefresh(refreshToken);
                    if (tokens != null)
                    {
                        var accessToken = tokens.RootElement.GetString("access_token");
                        ctx.Response.Cookies.Append("VAT", accessToken!, new CookieOptions
                        {
                            Expires = DateTime.Now.AddMinutes(10),
                            HttpOnly = true,
                            SameSite = SameSiteMode.Strict,
                            Secure = true
                        });
                        ctx.Response.Cookies.Append("VRT", tokens.RootElement.GetString("refresh_token")!, new CookieOptions
                        {
                            Expires = DateTime.Now.AddDays(30),
                            HttpOnly = true,
                            SameSite = SameSiteMode.Strict,
                            Secure = true
                        });

                        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);

                        var sp = builder.Services.BuildServiceProvider();
                        var dbContext = sp.GetService<DataContext>();
                        var claim = jwt.Claims.First(c => c.Type == "ID").Value;
                        var id = await dbContext.AuthIds
                            .FirstOrDefaultAsync(a => a.AuthId.ToString() == claim);

                        File.AppendAllText("log", id.ViunaUserId.ToString() + "\n");

                        ctx.Principal = new ClaimsPrincipal(
                                    new ClaimsIdentity(
                                        new Claim[]
                                        {
                                            new Claim("ID", id.ViunaUserId.ToString())
                                        },
                                        "Cookie"
                                        ));
                        ctx.Success();
                    }
                }
                ctx.Response.Cookies.Delete("RC");
            },

            OnTokenValidated = async ctx =>
            {
                var sp = builder.Services.BuildServiceProvider();
                var dbContext = sp.GetService<DataContext>();
                var id = await dbContext!.AuthIds
                    .FirstOrDefaultAsync(a => a.AuthId.ToString() == ctx.Principal!.FindFirstValue("ID"));

                if (id == null)
                {
                    ctx.Fail(new Exception("you need to register in ViunaGuard"));
                }
                else
                {
                    ctx.Principal = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new Claim[]
                            {
                                new Claim("ID", id.ViunaUserId.ToString())
                            },
                            "Cookie"
                        ));
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

    o.AddPolicy("RoleCookie", new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes("RoleCookie")
        .Build());
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

async Task<JsonDocument> AccessRefresh(string refreshToken)
{
    File.AppendAllText("log", DateTime.Now + " : refreshing the access token\n");
    var request = new RtRequest()
    {
        ClientId = "12345",
        ClientSecret = "secretTest",
        RefreshToken = refreshToken,
        TokenEndpoint = "https://localhost:7120/api/OAuth/token"
    };

    var tokens = await RefreshTokenHandlerClass.RefreshTokenHandler(request);
    return tokens;
}