using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_NicoMore02.Models;

namespace tl2_tp8_2025_NicoMore02.Controllers;

public class ProductosController : Controller
{
    private ProductosRepository productosRepository;
    public ProductosController()
    {
        productosRepository = new ProductosRepository();
    }

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
    public IActionResult Create(Productos producto)
    {
        productosRepository.CrearProducto(producto);
        TempData["Success"] = "Producto Creado";
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