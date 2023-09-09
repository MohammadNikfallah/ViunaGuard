using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


namespace ViunaGuard.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenHandler _refreshTokenHandler;

        public AuthController(IConfiguration configuration,IRefreshTokenHandler refreshTokenHandler)
        {
            _configuration = configuration;
            _refreshTokenHandler = refreshTokenHandler;
        }
        
        [HttpGet("Login")]
        public IResult Login()
        {
            
            return Results.Challenge(
                new AuthenticationProperties()
                {
                    RedirectUri = "https://localhost:7063/"
                },
                authenticationSchemes: new List<string>() { "OAuth" });
        }
        
        [HttpPost("Login")]
        public async Task<ActionResult> PostLogin(UserLogin userLogin)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{_configuration.GetValue<string>("Constants:OauthBaseUrl")}api/OAuth/login");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonSerializer.Serialize(new
                {
                    username = userLogin.Username,
                    password = userLogin.Password
                });

                await streamWriter.WriteAsync(json);
            }

            var httpResponse = (HttpWebResponse) await httpWebRequest.GetResponseAsync();

            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var cookie = httpResponse.GetResponseHeader("Set-Cookie");
                var cookieName = cookie.Split('=')[0];
                var cookieString = cookie.Split('=')[1].Split(';')[0];
                
                Response.Cookies.Append(cookieName, cookieString);
                return Ok();
            }

            return BadRequest();
        }
        
        
        [HttpGet("Logout")]
        public ActionResult Logout()
        {
            Response.Cookies.Delete("VRT");
            Response.Cookies.Delete("VAT");
            Response.Cookies.Delete("RC");
            return Ok();
        }

        [HttpGet("RefreshToken")]
        [Authorize("RefreshToken")]
        public async Task<ActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies.FirstOrDefault(x => x.Key == "VRT").Value;
            if (refreshToken == null)
            {
                refreshToken = Request.Headers["Authorization"];
                if (refreshToken!.StartsWith("Bearer"))
                    refreshToken = refreshToken.Split(' ')[1];
                else
                    return BadRequest();
            }
            var tokens = await _refreshTokenHandler.AccessRefresh(refreshToken);
            var accessToken = tokens.RootElement.GetString("access_token");
            if (accessToken == null)
            {
                return BadRequest();
            }
            Response.Cookies.Append("VAT", accessToken, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddHours(1),
                SameSite = SameSiteMode.Strict,
                Secure = true
            });
            var newRefreshToken = tokens.RootElement.GetString("refresh_token");
            Response.Cookies.Append("VRT", newRefreshToken!, new CookieOptions
            {
                Path = "/Auth/RefreshToken",
                Expires = DateTime.Now.AddDays(30),
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = true
            });
            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken!
            });
        }

        [HttpGet("Test")]
        [Authorize]
        public ActionResult Test()
        {
            return Ok();
        }

    }
}
