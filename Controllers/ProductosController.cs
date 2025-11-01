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
}