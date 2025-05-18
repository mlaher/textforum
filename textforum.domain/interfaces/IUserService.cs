using textforum.domain.models;

namespace textforum.domain.interfaces
{
    public interface IUserService
    {
        Task<User> GetFromCredentials(string email, string passwor, string correlationIdd);
        Task<User> GetFromEmail(string email, string correlationId);
        Task<User> GetFromUserId(long userId, string correlationId);
        Task<User?> Register(User user, string correlationId);
        Task<bool> UserExists(string email, string correlationId);
    }
}