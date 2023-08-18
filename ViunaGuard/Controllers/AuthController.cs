using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;
using ViunaGuard.Models;

namespace ViunaGuard.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext context;

        public AuthController(DataContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public IResult Login()
        {
            return Results.Challenge(
                new Microsoft.AspNetCore.Authentication.AuthenticationProperties()
                {
                    RedirectUri = "https://localhost:7063/"
                },
                authenticationSchemes: new List<string>() { "OAuth" });
        }

        [HttpGet("RoleLogin")]
        [Authorize]
        public async Task<ActionResult> RoleLogin(int EmployeeId)
        {
            var job = context.Employees.Find(EmployeeId);
            if (job != null && job.PersonId.ToString() == HttpContext.User.FindFirstValue("ID"))
            {
                await HttpContext.SignInAsync("RoleCookie",
                new ClaimsPrincipal(
                    new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Role, "Guard"),
                        new Claim("EmployeeId", EmployeeId.ToString()),
                    }, CookieAuthenticationDefaults.AuthenticationScheme)));
                return Ok();
            } else
            {
                await HttpContext.SignInAsync("RoleCookie",
                new ClaimsPrincipal(
                    new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Role, "User"),
                    }, CookieAuthenticationDefaults.AuthenticationScheme)));
                return Ok();
            }
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
