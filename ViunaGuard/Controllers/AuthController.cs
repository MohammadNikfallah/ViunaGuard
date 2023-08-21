using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
                await HttpContext.SignInAsync("RoleCookie",
                new ClaimsPrincipal(
                    new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Role, "Guard"),
                        new Claim("EmployeeId", employeeId.ToString()),
                    }, CookieAuthenticationDefaults.AuthenticationScheme)));
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
        [Authorize(Roles = "User", Policy = "RoleCookie")]
        public ActionResult Test()
        {
            return Ok(HttpContext.User.Claims.Select(c => new {c.Type, c.Value}).ToList());
        }



    }
}
