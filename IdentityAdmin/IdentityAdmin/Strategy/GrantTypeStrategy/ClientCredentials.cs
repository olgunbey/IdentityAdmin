using IdentityAdmin.Dto.Request;
using IdentityAdmin.Dto.Response;
using Microsoft.Extensions.Options;

namespace IdentityAdmin.Strategy.GrantTypeStrategy
{
    public class ClientCredentials:IGrantType
    {
        public ClientCredentials()
        {
        }
        public Task<ResponseTokenDto> Execute(RequestTokenDto requestTokenDto)
        {
            //burada işlemler
            throw new NotImplementedException();
        }
    }
}
