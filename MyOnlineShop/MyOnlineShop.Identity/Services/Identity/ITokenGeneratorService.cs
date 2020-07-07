using MyOnlineShop.Identity.Data.Models;
using System.Collections.Generic;

namespace MyOnlineShop.Identity.Services.Identity
{
    public interface ITokenGeneratorService
    {
        public string GenerateToken(User user, IEnumerable<string> roles = null);
    }
}
