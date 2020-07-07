using System.Collections;
using System.Collections.Generic;

namespace MyOnlineShop.Identity.ViewModels
{
    public class RegisterCustomerResultModel
    {
        public RegisterCustomerResultModel()
        {
            this.Errors = new List<string>();
        }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool Succeeded { get; set; }

        public IList<string> Errors { get; set; }
    }
}
