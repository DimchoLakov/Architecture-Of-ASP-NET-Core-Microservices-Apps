using System.Collections;
using System.Collections.Generic;

namespace MyOnlineShop.Identity.Models
{
    public class LoginCustomerResultModel
    {
        public LoginCustomerResultModel()
        {
            this.Errors = new List<string>();
        }
 
        public bool Succeeded { get; set; }

        public string Token { get; set; }

        public IList<string> Errors { get; set; }
    }
}
