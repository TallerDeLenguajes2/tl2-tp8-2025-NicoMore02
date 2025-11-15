using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Repositorios;
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
        productosRepository.ActualizarProducto(id, ProductoEditado);
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