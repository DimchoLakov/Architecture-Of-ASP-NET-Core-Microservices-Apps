using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace MyOnlineShop.WebMVC.Areas.Admin.ViewModels.Common
{
    public class IFormFileWrapper
    {
        public IFormFileWrapper()
        {
            this.Files = new List<IFormFile>();
        }

        public ICollection<IFormFile> Files { get; set; }
    }
}
