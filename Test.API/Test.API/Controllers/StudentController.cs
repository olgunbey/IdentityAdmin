using IdentityAdmin.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test.API.Atrributes;

namespace Test.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [Authorize(Roles = RoleConstants.StudentRole)]
        [HttpGet]
        public IActionResult Profile()
        {
            return Ok("Student profile giriş yaptı");
        }
        [HasPermission(permission: PermissionConstants.StudentViewGrades)]
        [HttpGet]
        public IActionResult Grades()
        {
            return Ok("Notlar listelendi");
        }
    }
}
