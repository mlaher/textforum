using textforum.domain.models;

namespace textforum.domain.interfaces
{
    public interface IUserService
    {
        Task<User> GetFromCredentials(string email, string password);
        Task<User> GetFromEmail(string email);
        Task<User> GetFromUserId(long userId);
        Task<User?> Register(User user);
        Task<bool> UserExists(string email);
    }
}