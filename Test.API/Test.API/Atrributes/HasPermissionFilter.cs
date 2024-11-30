using IdentityAdmin.Database;
using IdentityAdmin.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceStack.Redis;
using System.Security.Claims;

namespace Test.API.Atrributes
{
    public class HasPermissionFilter : IAsyncAuthorizationFilter
    {
        public string Permission { get; set; }
        private readonly IUserDbExecute _userDbExecute;
        private readonly IRedisClientAsync _redisClientAsync;
        public HasPermissionFilter(string permission, IUserDbExecute userDbExecute, IRedisClientAsync redisClientAsync)
        {
            Permission = permission;
            _userDbExecute = userDbExecute;
            _redisClientAsync = redisClientAsync;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            Claim? userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            Claim? userRole = context.HttpContext.User.FindFirst(ClaimTypes.Role);

            if (userIdClaim == null || userRole == null)
            {
                context.Result = new UnauthorizedResult();
            }
            List<CacheRefreshTokenDto> cacheRefreshTokenDto = await _redisClientAsync.GetAsync<List<CacheRefreshTokenDto>>("CacheRefreshToken");

            var user = cacheRefreshTokenDto.First(y => y.UserID == int.Parse(userIdClaim!.Value));
            if (user!= null && user.RefreshTokenExpire>=DateTime.UtcNow)
            {
                if(!user.Permissions.Any(y => y.Equals(Permission)))
                {
                    context.Result = new ForbidResult();
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
