using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;


namespace ViunaGuard.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;

        public AuthController(DataContext context)
        {
            _context = context;
        }
        
        [HttpGet("GetToken")]
        public IResult GetToken()
        {
            return Results.Challenge(
                new AuthenticationProperties()
                {
                    RedirectUri = "https://localhost:7063/"
                },
                authenticationSchemes: new List<string>() { "OAuth" });
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
        public async Task<ActionResult<object>> PostLogin(UserLogin userLogin)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:7120/api/OAuth/login");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonSerializer.Serialize(new
                {
                    username = userLogin.Username,
                    password = userLogin.Password
                });

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse) await httpWebRequest.GetResponseAsync();

            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var cookie = httpResponse.GetResponseHeader("Set-Cookie");
                var cookieName = cookie.Split('=')[0];
                var cookieString = cookie.Split('=')[1].Split(';')[0];
                
                
                Results.Challenge(
                    new AuthenticationProperties()
                    {
                        RedirectUri = "https://localhost:7063/"
                    },
                    authenticationSchemes: new List<string>() { "OAuth" });
                return Task.FromResult<ActionResult<object>>(Ok());
            }

            return Task.FromResult<ActionResult<object>>(BadRequest());
            // return Results.Challenge(
            //     new AuthenticationProperties()
            //     {
            //         RedirectUri = "https://localhost:7063/"
            //     },
            //     authenticationSchemes: new List<string>() { "OAuth" });
        }
        /**
         *logout the user
         */
        [HttpGet("Logout")]
        public ActionResult Logout()
        {
            Response.Cookies.Delete("VRT");
            Response.Cookies.Delete("VAT");
            Response.Cookies.Delete("RC");
            return Ok();
        }

        [HttpGet("RoleLogin")]
        [Authorize]
        public async Task<ActionResult> RoleLogin(int employeeId)
        {
            var job = _context.Employees.Find(employeeId);
            if (job != null && job.PersonId.ToString() == HttpContext.User.FindFirstValue("ID"))
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim("EmployeeId", employeeId.ToString()));
                if(job.EmployeeTypeId == 1)
                    identity.AddClaim(new Claim(ClaimTypes.Role, "Guard"));
                else
                    identity.AddClaim(new Claim(ClaimTypes.Role, "Employee"));
                await HttpContext.SignInAsync("RoleCookie",
                new ClaimsPrincipal(
                    identity));
                return Ok();
            }
            await HttpContext.SignInAsync("RoleCookie",
            new ClaimsPrincipal(
                new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Role, "User"),
                }, CookieAuthenticationDefaults.AuthenticationScheme)));
            return Ok();
        }

        [HttpGet("Test")]
        [Authorize]
        [Authorize(Policy = "RoleCookie")]
        public ActionResult Test()
        {
            return Ok();
        }



    }
}
