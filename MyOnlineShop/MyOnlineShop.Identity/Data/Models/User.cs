using Microsoft.AspNetCore.Identity;

namespace MyOnlineShop.Identity.Data.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
