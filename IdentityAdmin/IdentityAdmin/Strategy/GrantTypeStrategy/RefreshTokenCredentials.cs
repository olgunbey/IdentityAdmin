using IdentityAdmin.Database;
using IdentityAdmin.Dto;
using IdentityAdmin.Dto.Request;
using IdentityAdmin.Dto.Response;
using IdentityAdmin.Entity;
using IdentityAdmin.Exceptions;
using IdentityAdmin.HashExecute;
using Microsoft.IdentityModel.Tokens;
using ServiceStack.Redis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityAdmin.Strategy.GrantTypeStrategy
{
    public class RefreshTokenCredentials : IGrantType
    {
        private readonly IClientDbExecute _clientDbExecute;
        private readonly IRedisClientAsync _redisClientAsync;
        private readonly IUserDbExecute _userDbExecute;
        public RefreshTokenCredentials(IClientDbExecute clientDbExecute, IRedisClientAsync redisClientAsync, IUserDbExecute userDbExecute)
        {
            _clientDbExecute = clientDbExecute;
            _redisClientAsync = redisClientAsync;
            _userDbExecute = userDbExecute;
        }
        public async Task<ResponseTokenDto> Execute(RequestTokenDto requestTokenDto)
        {
            Client client = await _clientDbExecute.GetClient(requestTokenDto.Client_Id);

            if (client.Secret != requestTokenDto.Secret)
            {
                throw new Exception("invalid_secretkey");
            }

            CacheRefreshTokenDto cacheRefreshTokenDto = await _redisClientAsync.GetAsync<CacheRefreshTokenDto>("CacheRefreshToken");

            if (cacheRefreshTokenDto.RefreshTokenExpire > DateTime.UtcNow && requestTokenDto.RefreshToken == cacheRefreshTokenDto.Refresh_Token)
            {
                var securityKey = new SymmetricSecurityKey(Hashing.Hash(client.Secret));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                User? user = await _userDbExecute.GetUserById(cacheRefreshTokenDto.UserID);

                List<Claim> claims = new List<Claim>();
                foreach (var role in user.UserRoles.Select(y => y.Role))
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Rolename));
                }
                claims.Add(new Claim("Username", user.Name));  //bu kısım düzeltilecek
                claims.Add(new Claim("Surname", user.Surname));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, requestTokenDto.Client_Id));
                DateTime expires = DateTime.UtcNow.AddMinutes(10);
                var SecretToken = new JwtSecurityToken(issuer: client.Issuer,
                    audience: client.Audience,
                    claims: claims,
                    expires: expires,
                    signingCredentials: credentials
                    );

                var token = new JwtSecurityTokenHandler().WriteToken(SecretToken);
                return new ResponseTokenDto
                {
                    Access_Token = token,
                    UserId = user.Id,
                    Refresh_Token = Guid.NewGuid().ToString(),
                    RefreshTokenExpire = DateTime.UtcNow.AddDays(1),
                    AccessTokenExpire = expires
                };
            }
            throw new UnAuthorizedException("UnAuthorized");
        }
    }
}
