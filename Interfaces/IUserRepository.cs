using MVC.Models;
namespace MVC.Interfaces;

public interface IUserRepository
{
    Usuario GetUser(string username, string password);
}