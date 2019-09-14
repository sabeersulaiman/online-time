using api.Models;

namespace api.Repositories
{
    public interface IUserRepository
    {
        User FindUserByEmail(string email);
        void SaveUser(User user);
    }
}