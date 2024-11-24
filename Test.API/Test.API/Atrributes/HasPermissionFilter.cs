using IdentityAdmin.Database;
using IdentityAdmin.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Test.API.Atrributes
{
    public class HasPermissionFilter : IAsyncAuthorizationFilter
    {
        public string Permission { get; set; }
        private readonly IUserDbExecute _userDbExecute;
        public HasPermissionFilter(string permission, IUserDbExecute userDbExecute)
        {
            Permission = permission;
            _userDbExecute = userDbExecute;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            Claim? userClaim = context.HttpContext.User.FindFirst("Name");
            Claim? userRole = context.HttpContext.User.FindFirst(ClaimTypes.Role);

            if (userClaim == null || userRole == null)
            {
                context.Result = new UnauthorizedResult();
            }
            bool hasPermission = await _userDbExecute.UserHasPermissionControl(Permission,userClaim!.Value);

            if (!hasPermission)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
