using IdentityAdmin.Database;
using IdentityAdmin.Dto;
using IdentityAdmin.Dto.Request;
using IdentityAdmin.Dto.Response;
using IdentityAdmin.Entity;
using IdentityAdmin.Enums;
using IdentityAdmin.Exceptions;
using IdentityAdmin.HashExecute;
using IdentityAdmin.Strategy.GrantTypeStrategy;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServiceStack;
using ServiceStack.Redis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace IdentityAdmin.TokenServices
{
    public class TokenService
    {
        private readonly IClientDbExecute _clientDbExecute;
        private readonly IRedisClientAsync _cacheClientAsync;
        private readonly IUserDbExecute _userDbExecute;
        public TokenService(IClientDbExecute clientDbExecute, IRedisClientAsync cacheClientAsync, IUserDbExecute userDbExecute)
        {
            _clientDbExecute = clientDbExecute;
            _cacheClientAsync = cacheClientAsync;
            _userDbExecute = userDbExecute;
        }
        public ResponseTokenDto GenerateToken(RequestTokenDto requestTokenDto)
        {
            return requestTokenDto.Grant_Type switch
            {
                nameof(GrantTypeEnum.clientCredentials) => new ClientCredentials().Execute(requestTokenDto).Result,
                nameof(GrantTypeEnum.password) => new ResourceOwnerCredentials(_clientDbExecute, _userDbExecute).Execute(requestTokenDto).Result,
                nameof(GrantTypeEnum.refreshtoken) => new RefreshTokenCredentials(_clientDbExecute, _cacheClientAsync, _userDbExecute).Execute(requestTokenDto).Result,
                _ => throw new InvalidGrantTypeException("invalid_granttype")
            };
        }
        public async Task<ResponseUserInfoDto> UserInfo(string token)
        {
            string[] parts = token.Split('.');
            if (parts.Length != 3)
            {
                Console.WriteLine("geçersiz jwt token");
            }
            ResponseUserInfoDto responseUserInfoDto = new();
            responseUserInfoDto.UserInfos = new();
            string payload = parts[1];
            string payloadJson = Base64UrlDecode.Base64Url(payload);
            var payloadObject = JsonSerializer.Deserialize<JsonElement>(payloadJson);
            string client_Id = payloadObject.GetProperty(ClaimTypes.NameIdentifier).GetString()!;
            using (AppDbContext appDbContext = new())
            {
                Client? client = await appDbContext.Client.Include(y => y.ClientUserInfos).ThenInclude(y => y.UserInfo).FirstOrDefaultAsync(y => y.Client_Id == client_Id);
                if (client is null)
                {
                    throw new InvalidClientException("invalid_client!!");
                }
                List<UserInfo> userInfos = client.ClientUserInfos.Select(y => y.UserInfo).ToList();




                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                try
                {
                    var validationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Hashing.Hash(client.Secret)),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = client.Audience,
                        ValidIssuer = client.Issuer,
                        ClockSkew = TimeSpan.Zero //Token süresi bittiğinde hemen geçersiz kılınır
                    };

                    var ClaimPrincipal = jwtSecurityTokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                    foreach (var userInfo in userInfos)
                    {
                        Claim? claim = ClaimPrincipal.FindFirst(userInfo.UserInfoName);

                        if (claim is not null)
                        {
                            responseUserInfoDto.UserInfos.Add(claim.Type, claim.Value);
                        }
                    }
                }
                catch
                {
                    throw new UnAuthorizedException("UnAuthorized"); //burayı düzeltecegim
                }
                return responseUserInfoDto;
            }
        }
    }
}
