using IdentityAdmin.Database;
using IdentityAdmin.Dto;
using IdentityAdmin.Dto.Request;
using Microsoft.AspNetCore.Mvc;
using ServiceStack.Redis;

namespace IdentityAdmin.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ConnectController : ControllerBase
    {
        private readonly TokenServices.TokenService _tokenService;
        private readonly IRedisClientAsync _redisClient;
        public ConnectController(TokenServices.TokenService tokenService, IRedisClientAsync redisClient)
        {
            _tokenService = tokenService;
            _redisClient = redisClient;
        }
        [HttpPost]
        public async Task<IResult> Token([FromBody]RequestTokenDto requestTokenDto)
        {
            var token = _tokenService.GenerateToken(requestTokenDto);
            CacheRefreshTokenDto cacheRefreshTokenDto = new CacheRefreshTokenDto
            {
                RefreshTokenExpire = token.RefreshTokenExpire,
                Refresh_Token = token.Refresh_Token,
                UserID = token.UserId,
                Permissions = token.Permissions,
            };
            await _redisClient.SetAsync("CacheRefreshToken", cacheRefreshTokenDto);
            return Results.Ok(token);
        }
        [HttpGet]
        public async Task<IResult> UserInfo([FromHeader]string access_Token)
        {
            return Results.Ok(await _tokenService.UserInfo(access_Token));
        }
        [HttpPost]
        public async Task<IResult> AddClient(RequestAddClient requestAddClient)
        {
            await new ClientDbExecute().AddClientAsync(requestAddClient);
            return Results.Ok();
        }
    }
}
