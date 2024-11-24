using IdentityAdmin.Dto.Request;
using IdentityAdmin.Dto.Response;

namespace IdentityAdmin.Strategy.GrantTypeStrategy
{
    public interface IGrantType
    {
        public Task<ResponseTokenDto> Execute(RequestTokenDto requestTokenDto);
    }
}
