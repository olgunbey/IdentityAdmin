using IdentityAdmin.Dto;
using IdentityAdmin.Entity;
using Microsoft.EntityFrameworkCore;

namespace IdentityAdmin.Database
{
    public class ClientDbExecute : IClientDbExecute
    {
        public async Task AddClientAsync(RequestAddClient requestAddClient)
        {
            using (AppDbContext appDbContext = new())
            {
                var UserInfo = appDbContext.UserInfo.AsQueryable()
                    .Where(y => requestAddClient.UserInfo.Contains(y.Id)).ToList();

                var client = new Client()
                {
                    Audience = requestAddClient.Audience,
                    Issuer = requestAddClient.Issuer,
                    Client_Id = requestAddClient.Client_Id,
                    TokenExpireType = requestAddClient.TokenExpireType,
                    Secret = requestAddClient.Secret,
                    ClientUserInfos = UserInfo.Select(y => new ClientUserInfo()
                    {
                        UserInfoId = y.Id,
                    }).ToList()
                };
                appDbContext.Add(client);

                await appDbContext.SaveChangesAsync();

            }
        }

        public void Dispose()
        {
        }

        public async Task<Client> GetClient(string client_id)
        {
            using (AppDbContext appDbContext = new())
            {
                Client client = await appDbContext.Client.Include(y=>y.ClientUserInfos).ThenInclude(y=>y.UserInfo).FirstAsync(y => y.Client_Id == client_id);
                return client;
            }
        }
    }
    public interface IClientDbExecute:IDisposable
    {
        public Task AddClientAsync(RequestAddClient requestAddClient);
        public Task<Client> GetClient(string client_id);
    }
}
