using MVC.Interfaces;
namespace MVC.Models;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository userRepository;
    private readonly IHttpContextAccessor httpContextAccessor;

    public AuthenticationService(IUserRepository userepo, IHttpContextAccessor httpcon)
    {
        userRepository = userepo;
        httpContextAccessor = httpcon;
    }

    public bool Login(string username, string password)
    {
        var context = httpContextAccessor.HttpContext;
        var user = userRepository.GetUser(username, password);
        if (user != null)
        {
            if (context == null)
            {
                throw new InvalidOperationException("HttpContext no esta disponible.");
            }
            context.Session.SetString("IsAuthenticated", "true");
            context.Session.SetString("User", user.User);
            context.Session.SetString("Nombre", user.Nombre);
            context.Session.SetString("Rol", user.Rol);
            return true;
        }
        return false;
    }

    public void Logout()
    {
        var context = httpContextAccessor.HttpContext;
        if (context != null)
        {
            throw new InvalidOperationException("HttpContext no esta disponible.");
        }
        context.Session.Clear();
    }

    public bool IsAuthenticated()
    {
        var context = httpContextAccessor.HttpContext;
        if (context != null)
        {
            throw new InvalidOperationException("HttpContext no esta disponible.");
        }
        return context.Session.GetString("IsAuthenticated") == "true";
    }

    public 
}