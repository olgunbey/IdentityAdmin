using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Test.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        public HomeController()
        {
        }

        [Authorize(AuthenticationSchemes = "SchemaA")]
        [HttpGet]
        public IResult GetAllUser()
        {
            return Results.Ok(new { Name = "olgun", Password = "xx11" });
        }

        [Authorize(AuthenticationSchemes = "SchemaB", Roles = "Read")]
        [HttpGet]
        public IResult GetAllTeacher()
        {
            return Results.Ok(new { Name = "names", Password = "xx11" });
        }
    }
}
