using Microsoft.AspNetCore.Identity;

namespace IdentityServerAspNetIdentity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser(string userName, string email = null)
        {
            UserName = userName;
            Email = email;
        }
    }
}
