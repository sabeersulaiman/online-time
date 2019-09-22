using System.Threading.Tasks;
using api.Models;

namespace api.Repositories
{
    public interface IUserRepository
    {
        Task<User> FindUserByEmail(string email);
        Task SaveUser(User user);
    }
}