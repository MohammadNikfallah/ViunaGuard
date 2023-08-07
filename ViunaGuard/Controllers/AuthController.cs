using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViunaGuard.Models;

namespace ViunaGuard.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
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
    }
}
