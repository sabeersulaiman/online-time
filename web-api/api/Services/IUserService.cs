using api.Models;

namespace api.Services
{
    public interface IUserService
    {
        User Login(string userName, string password);
        void SignUp(User user);
    }
}