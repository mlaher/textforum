namespace textforum.domain.interfaces
{
    public interface IAppAuthenticationService
    {
        bool AuthenticateApp(string appToken, string ipAddress, string machineIdentifier, string correlationId);
    }
}