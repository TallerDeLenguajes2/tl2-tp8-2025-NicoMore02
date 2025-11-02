using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_NicoMore02.Models;

namespace tl2_tp8_2025_NicoMore02.Controllers;

public class PresupuestosController : Controller
{
    private PresupuestosRepository presupuestosRepository;
    public PresupuestosController()
    {
        presupuestosRepository = new PresupuestosRepository();
    }

    [HttpGet]
    public IActionResult Index()
    {
        List<Presupuestos> presupuestos = presupuestosRepository.ListarPresupuestos();
        return View(presupuestos);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var presupuesto = presupuestosRepository.GetPresupuesto(id);
        if (presupuesto == null)
        {
            NotFound();
        }
        return View(presupuesto);
    }

    //GET y POST para CREAR
    [HttpGet]
    public IActionResult Create()
    {
        var presupuesto = new Presupuestos
        {
            FechaCreacion = DateTime.Now
        };
        return View(presupuesto);
    }

    [HttpPost]
    public IActionResult Create(Presupuestos presupuestos)
    {
        presupuestosRepository.CrearPresupuesto(presupuestos);
        TempData["Success"] = "Presupuesto creado correctamente";
        return RedirectToAction(nameof(Index));
    }

    //GET y POST para EDITAR

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var presupuesto = presupuestosRepository.GetPresupuesto(id);
        if (presupuesto == null)
        {
            TempData["Error"] = "Presupuesto no encontrado";
            return RedirectToAction(nameof(Index));
        }
        return View(presupuesto);
    }

    [HttpPost]
    public IActionResult Edit(int id, Presupuestos presupuesto)
    {
        presupuestosRepository.ActualizarPresupuesto(presupuesto);
        TempData["Success"] = "Presupuesto actualizado correctamente";
        return RedirectToAction(nameof(Details), new { id = presupuesto.idPresupuesto });
    }

    // GET y POST para Eliminar
    [HttpGet]
    public IActionResult Delete(int id)
    {

        var presupuesto = presupuestosRepository.GetPresupuesto(id);
        if (presupuesto == null)
        {
            TempData["Error"] = "Presupuesto no encontrado";
            return RedirectToAction(nameof(Index));
        }
        return View(presupuesto);
    }

    [HttpPost, ActionName("EliminarConfirmado")]
    public IActionResult DeleteConfirmed(int id)
    {
        var presupuesto = presupuestosRepository.GetPresupuesto(id);
        if (presupuesto == null)
        {
            TempData["Error"] = "Presupuesto no encontrado";
            return RedirectToAction(nameof(Index));
        }
        presupuestosRepository.EliminarPresupuesto(id);
        TempData["Success"] = "Presupuesto Eliminado correctamente";
        return RedirectToAction(nameof(Index));
    }

    //GET y POST para agregar producto a un presupuesto

    [HttpGet]
    public IActionResult AddProduct(int id)
    {

        var presupuesto = presupuestosRepository.GetPresupuesto(id);
        if (presupuesto == null)
        {
            TempData["Error"] = "Presupuesto no encontrado";
            return RedirectToAction(nameof(Index));
        }

        return View(presupuesto);
    }

    [HttpPost, ActionName("AgregarProducto")]
    public IActionResult AddProduct(int idPresupuesto, int idProducto, int cantidad)
    {
        presupuestosRepository.AgregarProductos(idPresupuesto, idProducto, cantidad);
        
        TempData["Success"] = $"Producto agregado correctamente al presupuesto (Cantidad: {cantidad})";
        return RedirectToAction(nameof(Details), new { id = idPresupuesto });
    }
        
}