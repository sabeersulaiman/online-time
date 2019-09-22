using System.Threading.Tasks;
using api.Models;

namespace api.Services
{
    public interface IUserService
    {
        Task<User> Login(string email, string password);
        Task SignUp(User user);
        Task<bool> EnsureAdminUser(string email);
    }
}