using Microsoft.AspNetCore.Identity;

namespace Menu_QRCode
{
    public class User : IdentityUser
    {
        public string Role { get; set; }
    }
}
