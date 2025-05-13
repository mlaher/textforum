
namespace textforum.domain.interfaces
{
    public interface IUserAuthenticationService
    {
        Task<(bool isValid, string token)> AuthenticateUser(string username, string password);
        Task<(bool isValid, IDictionary<string, string?>? claims)> GetClaims(string jwt);
    }
}