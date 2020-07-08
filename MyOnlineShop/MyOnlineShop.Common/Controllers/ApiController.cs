using Microsoft.AspNetCore.Mvc;

namespace MyOnlineShop.Common.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        public const string PathSeparator = "/";

        public const string Id = "{id}";
    }
}
