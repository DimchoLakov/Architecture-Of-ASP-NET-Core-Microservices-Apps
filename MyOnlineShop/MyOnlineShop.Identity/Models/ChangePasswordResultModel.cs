using System.Collections;
using System.Collections.Generic;

namespace MyOnlineShop.Identity.Models
{
    public class ChangePasswordResultModel
    {
        public ChangePasswordResultModel()
        {
            this.Errors = new List<string>();
        }

        public bool Succeeded { get; set; }

        public IList<string> Errors { get; set; }
    }
}
