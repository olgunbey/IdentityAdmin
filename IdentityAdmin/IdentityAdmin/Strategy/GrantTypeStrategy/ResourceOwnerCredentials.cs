using IdentityAdmin.Database;
using IdentityAdmin.Dto.Request;
using IdentityAdmin.Dto.Response;
using IdentityAdmin.Entity;
using IdentityAdmin.Exceptions;
using IdentityAdmin.HashExecute;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;

namespace IdentityAdmin.Strategy.GrantTypeStrategy
{
    public class ResourceOwnerCredentials : IGrantType
    {
        private readonly IClientDbExecute _clientDbExecute;
        private readonly IUserDbExecute _userDbExecute;
        public ResourceOwnerCredentials(IClientDbExecute clientDbExecute, IUserDbExecute userDbExecute)
        {

            _clientDbExecute = clientDbExecute;
            _userDbExecute = userDbExecute;
        }
        public async Task<ResponseTokenDto> Execute(RequestTokenDto requestTokenDto)
        {
            Client client = await _clientDbExecute.GetClient(requestTokenDto.Client_Id);
            if (client == null || client.Secret != requestTokenDto.Secret)
            {
                throw new Exception("Tanımsız ClientId veya invalid_secretkey");
            }

            User? user = await _userDbExecute.GetUser(requestTokenDto.Username!, requestTokenDto.Password!);
            if (user is null)
            {
                throw new NotUserException("bu kullanıcı yok");
            }

            var userRoles = FillClaimRoles(user.UserRoles.Select(y => y.Role)).ToList();


            var userInfos = client.ClientUserInfos.Select(y => y.UserInfo);
            var userInfoClaim= FillClaimUserInfo(userInfos, user).ToList();

            userRoles.AddRange(userInfoClaim);

            userRoles.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

            DateTime expires = DateTime.UtcNow.AddMinutes(1);
            var securityKey = new SymmetricSecurityKey(Hashing.Hash(client.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var SecretToken = new JwtSecurityToken(issuer: client.Issuer,
                audience: client.Audience,
                claims: userRoles,
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
                AccessTokenExpire = expires,
                Permissions=user.UserPermissions.Select(x => x.Permission.Name).ToList()
            };
        }

        private IEnumerable<Claim> FillClaimRoles(IEnumerable<Role> roles)
        {
            foreach (Role role in roles)
            {
                yield return new Claim(ClaimTypes.Role, role.Rolename);
            }
        }
        private IEnumerable<Claim> FillClaimUserInfo(IEnumerable<UserInfo> userInfos,User user)
        {
            foreach (var userInfo in userInfos)
            {
                PropertyInfo? propertyInfo = typeof(User).GetProperty(userInfo.UserInfoName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo == null)
                {
                    continue;
                }
                yield return new Claim(userInfo.UserInfoName, propertyInfo.GetValue(user)!.ToString()!);
            }
        }
    }
}
