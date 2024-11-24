using IdentityAdmin.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IdentityAdmin.Database
{
    public class UserDbExecute : IUserDbExecute
    {
        public async Task<User?> GetUser(string name, string password)
        {
            using (AppDbContext appDbContext = new())
            {
                return await appDbContext.User.Include(y => y.UserRoles).ThenInclude(y => y.Role).Include(y => y.UserPermissions).FirstOrDefaultAsync(y => y.Name == name && y.Password == password);
            }
        }

        public async Task<User> GetUserById(int userId)
        {
            using (AppDbContext appDbContext = new())
            {
                return (await appDbContext.User.Include(y => y.UserRoles).ThenInclude(y => y.Role).FirstOrDefaultAsync(y => y.Id == userId))!;
            }
        }

        public async Task<User?> GetUserProperties(Expression<Func<User, bool>> expression)
        {
            using (AppDbContext appDbContext = new())
            {
                return await appDbContext.User.Include(y => y.UserRoles).ThenInclude(y => y.Role).Include(y => y.UserPermissions).FirstOrDefaultAsync(expression);
            }
        }

        public async Task<bool> UserHasPermissionControl(string permission, string username)
        {
            using (AppDbContext appDbContext = new())
            {
                User user = await appDbContext.User.FirstAsync(y => y.Name == username);

                await appDbContext.Entry(user).Collection(y => y.UserPermissions).LoadAsync();

                foreach (var userPermission in user.UserPermissions)
                {
                  await  appDbContext.Entry(userPermission).Reference(y => y.Permission).LoadAsync();
                }
                bool hasPermission = user.UserPermissions.Any(y => y.Permission.Name == permission);

                if (hasPermission)
                    return true;
                return false;
            }

        }
    }
    public interface IUserDbExecute
    {
        Task<User?> GetUser(string name, string password);
        Task<User> GetUserById(int userId);
        Task<bool> UserHasPermissionControl(string permission, string username);
        Task<User?> GetUserProperties(Expression<Func<User, bool>> expression);
    }
}
