using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_NicoMore02.Models;
using SistemaVentas.Web.ViewModels;
using tl2_tp8_2025_NicoMore02.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace tl2_tp8_2025_NicoMore02.Controllers;

public class PresupuestosController : Controller
{
    private PresupuestosRepository presupuestosRepository;
    private readonly ProductosRepository productosRepository = new ProductosRepository();
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
        var presupuesto = new PresupuestoViewModel
        {
            FechaCreacion = DateTime.Now
        };
        return View(presupuesto);
    }

    [HttpPost]
    public IActionResult Create(PresupuestoViewModel presupuestoVm)
    {
        if (!ModelState.IsValid)
        {
            return View(presupuestoVm);
        }
        var nuevoPresupuesto = new Presupuestos
        {
            NombreDestinatario = presupuestoVm.NombreDestinatario,
            FechaCreacion = presupuestoVm.FechaCreacion
        };
        presupuestosRepository.CrearPresupuesto(nuevoPresupuesto);
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
        var presupuestoEditar = new PresupuestoViewModel
        {
            idPresupuesto = presupuesto.idPresupuesto,
            NombreDestinatario = presupuesto.NombreDestinatario,
            FechaCreacion = presupuesto.FechaCreacion
        };
        return View(presupuestoEditar);
    }

    [HttpPost]
    public IActionResult Edit(int id, PresupuestoViewModel presupuestoView)
    {
        if (!ModelState.IsValid)
        {
            return View(presupuestoView);
        }
        var presupuestoEditar = new Presupuestos
        {
            idPresupuesto = presupuestoView.idPresupuesto,
            NombreDestinatario = presupuestoView.NombreDestinatario,
            FechaCreacion = presupuestoView.FechaCreacion
        };
        presupuestosRepository.ActualizarPresupuesto(presupuestoEditar);
        TempData["Success"] = "Presupuesto actualizado correctamente";
        return RedirectToAction(nameof(Details), new { id = presupuestoView.idPresupuesto });
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
    public IActionResult AgregarProducto(int id)
    {

        var productos = productosRepository.ListarTodos();
        var modelo = new AgregarProductoViewModel
        {
            idPresupuesto = id,
            ListaProductos = new SelectList(productos, "idProducto", "descripcion")
        };

        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AgregarProducto(AgregarProductoViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            var producto = productosRepository.ListarTodos();
            modelo.ListaProductos = new SelectList(producto, "idProducto", "descripcion");
            return View(modelo);
        }
        presupuestosRepository.AgregarProductos(modelo.idPresupuesto, modelo.idProducto, modelo.cantidad);

        TempData["Success"] = $"Producto agregado correctamente al presupuesto (Cantidad: {modelo.cantidad})";
        return RedirectToAction(nameof(Details), new { id = modelo.idPresupuesto });
    }

}