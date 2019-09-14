using api.Models;

namespace api.Services
{
    public interface IUserService
    {
        User Login(string email, string password);
        void SignUp(User user);
        bool EnsureAdminUser(string email);
    }
}