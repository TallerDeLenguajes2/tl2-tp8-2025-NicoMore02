using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaVentas.Web.ViewModels;
using MVC.Models;
using MVC.Repositorios;
using MVC.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.Controllers;

public class LoginController : Controller
{
    private readonly IAuthenticationRepository authentication;

    public LoginController (IAuthenticationRepository authenticationRepository)
    {
        authentication = authenticationRepository;
    }

    [HttpGet]
    public IActionResult Index()
    {
        if (authentication.IsAuthenticated())
        {
            return RedirectToAction("Index", "Home");
        }
        return View(new LoginViewModel());
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if(string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
        {
            model.ErrorMessage = "Debe Ingresar usuario y contraseña";
            return View("Index", model);
        }

        if (authentication.Login(model.Username, model.Password))
        {
            return RedirectToAction("Index", "Home");
        }

        model.ErrorMessage = "Credenciales Invalidas";
        return View("Index", model);
    }

    [HttpGet]
    public IActionResult Logout()
    {
        authentication.Logout();
        return RedirectToAction("Index");
    }
}