using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_NicoMore02.Models;
using SistemaVentas.Web.ViewModels;

namespace tl2_tp8_2025_NicoMore02.Controllers;

public class ProductosController : Controller
{
    private readonly ProductosRepository productosRepository = new ProductosRepository();
    //public ProductosController()
    //{
    //    productosRepository = new ProductosRepository();
    //}

    [HttpGet]
    public IActionResult Index()
    {
        List<Productos> productos = productosRepository.ListarTodos();
        return View(productos);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var producto = productosRepository.BuscarPorId(id);
        return View(producto);
    }

    //GET y POST para Editar Producto
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var producto = productosRepository.BuscarPorId(id);
        if (producto == null)
        {
            return NotFound();
        }
        return View(producto);
    }

    [HttpPost]
    public IActionResult Edit(int id, Productos producto)
    {
        productosRepository.ActualizarProducto(id, producto);
        TempData["Success"] = "Producto Actualizado";
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
        if (!ModelState.IsValid)
        {
            return View(ProductoVM);
        }
        var nuevoProducto = new Productos
        {
            descripcion = ProductoVM.descripcion,
            precio = ProductoVM.precio
        };
        productosRepository.CrearProducto(nuevoProducto);
        return RedirectToAction(nameof(Index));
    }

    //GET y POST para Eliminar Poducto
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var producto = productosRepository.BuscarPorId(id);
        if (producto == null)
        {
            return NotFound();
        }
        return View(producto);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        productosRepository.EliminarProducto(id);
        TempData["Success"] = "Se elimino el producto con exitó";
        return RedirectToAction(nameof(Index));
    }
}