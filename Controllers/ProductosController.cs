using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC.Interfaces;
using MVC.Models;
using MVC.Repositorios;
using SistemaVentas.Web.ViewModels;

namespace MVC.Controllers;

public class ProductosController : Controller
{
    private readonly IProductoRepository _repo;
    private readonly IAuthenticationRepository _authService;

    public ProductosController(IProductoRepository prodRepo, IAuthenticationRepository authService)
    {
        _repo = prodRepo;
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;

        List<Productos> productos = _repo.ListarTodos();
        return View(productos);
    }

    private IActionResult CheckAdminPermissions()
        {
            
            if (!_authService.IsAuthenticated())
            {
                return RedirectToAction("Index", "Login");
            }

            
            if (!_authService.HasAccessLevel("Administrador"))
            {
                return RedirectToAction(nameof(AccesoDenegado));
            }

            return null; 
        }

    public IActionResult AccesoDenegado()
        {
            return View();
        }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var check = CheckAdminPermissions();
        if (check != null) return check;

        var producto = _repo.BuscarPorId(id);
        return View(producto);
    }

    //GET y POST para Editar Producto
    [HttpGet]
    public IActionResult Edit(int id)
    {

        var producto = _repo.BuscarPorId(id);
        if (producto == null)
        {
            return NotFound();
        }

        var productoEditar = new ProductoViewModel
        {
            idProducto = producto.idProducto,
            descripcion = producto.descripcion,
            precio = producto.precio
        };
        return View(productoEditar);
    }

    [HttpPost]
    public IActionResult Edit(int id, ProductoViewModel productoVM)
    {
        var check = CheckAdminPermissions();
        if (check != null) return check;

        if (id != productoVM.idProducto) return NotFound();

        if (!ModelState.IsValid)
        {
            return View(productoVM);
        }
        var ProductoEditado = new Productos
        {
            idProducto = productoVM.idProducto,
            descripcion = productoVM.descripcion,
            precio = productoVM.precio
        };
        _repo.ActualizarProducto(id, ProductoEditado);
        return RedirectToAction(nameof(Index));
    }

    //GET y POST para Crear Producto
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(ProductoViewModel ProductoVM)
    {
        var check = CheckAdminPermissions();
        if (check != null) return check;

        if (!ModelState.IsValid)
        {
            return View(ProductoVM);
        }
        var nuevoProducto = new Productos
        {
            descripcion = ProductoVM.descripcion,
            precio = ProductoVM.precio
        };
        _repo.CrearProducto(nuevoProducto);
        return RedirectToAction(nameof(Index));
    }

    //GET y POST para Eliminar Poducto
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var producto = _repo.BuscarPorId(id);
        if (producto == null)
        {
            return NotFound();
        }
        return View(producto);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var check = CheckAdminPermissions();
        if (check != null) return check;

        _repo.EliminarProducto(id);
        TempData["Success"] = "Se elimino el producto con exitó";
        return RedirectToAction(nameof(Index));
    }
}