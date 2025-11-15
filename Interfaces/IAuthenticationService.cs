namespace MVC.Interfaces;

public interface IAuthenticationService
{
    bool login(string user, string pass);
    void Logout();
    bool IsAuthenticated();
    void HasAccessLever(string rol);
}