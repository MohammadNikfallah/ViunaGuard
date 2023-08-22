using System.ComponentModel;

namespace ViunaGuard.Models;

public class UserLogin
{
    [DisplayName(displayName:"username")]
    public string Username { get; set; }
    [DisplayName(displayName:"password")]
    public string Password { get; set; }
}