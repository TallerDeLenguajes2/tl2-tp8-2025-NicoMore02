using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaVentas.Web.ViewModels;
using MVC.Models;
using MVC.Repositorios;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Interfaces;


namespace tl2_tp8_2025_NicoMore02.Controllers;

public class PresupuestosController : Controller
{
    private readonly IPresupuestoRepository _repo;
    private readonly IProductoRepository _productoRepo;
    private readonly IAuthenticationRepository _authService;

    public PresupuestosController(
        IPresupuestoRepository repo,
        IProductoRepository productoRepo,
        IAuthenticationRepository authService)
    {
        _repo = repo;
        _productoRepo = productoRepo;
        _authService = authService;
    }

    private IActionResult CheckPermissions()
    {
        if (!_authService.IsAuthenticated())
            return RedirectToAction("Index", "Login");

        if (_authService.HasAccessLevel("Administrador") || _authService.HasAccessLevel("Cliente")) return null;

        return RedirectToAction(nameof(AccesoDenegado));
    }

    private IActionResult CheckWritePermissions()
    {
        if (!_authService.IsAuthenticated()) return RedirectToAction("Index", "Login");

        if (!_authService.HasAccessLevel("Administrador")) return RedirectToAction(nameof(AccesoDenegado));

        return null;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var check = CheckPermissions();
        if (check != null) return check;

        List<Presupuestos> presupuestos = _repo.ListarPresupuestos();
        return View(presupuestos);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var check = CheckPermissions();
        if (check != null) return check;

        var presupuesto = _repo.GetPresupuesto(id);
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
        var check = CheckPermissions();
        if (check != null) return check;

        var presupuesto = new PresupuestoViewModel
        {
            FechaCreacion = DateTime.Now
        };
        return View(presupuesto);
    }

    [HttpPost]
    public IActionResult Create(PresupuestoViewModel presupuestoVm)
    {
        var check = CheckWritePermissions();
        if (check != null) return check;

        if (!ModelState.IsValid)
        {
            return View(presupuestoVm);
        }
        var nuevoPresupuesto = new Presupuestos
        {
            NombreDestinatario = presupuestoVm.NombreDestinatario,
            FechaCreacion = presupuestoVm.FechaCreacion
        };
        _repo.CrearPresupuesto(nuevoPresupuesto);
        TempData["Success"] = "Presupuesto creado correctamente";
        return RedirectToAction(nameof(Index));
    }

    //GET y POST para EDITAR

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var check = CheckPermissions();
        if (check != null) return check;

        var presupuesto = _repo.GetPresupuesto(id);
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
        var check = CheckWritePermissions();
        if (check != null) return check;

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
        _repo.ActualizarPresupuesto(presupuestoEditar);
        TempData["Success"] = "Presupuesto actualizado correctamente";
        return RedirectToAction(nameof(Details), new { id = presupuestoView.idPresupuesto });
    }

    // GET y POST para Eliminar
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var check = CheckPermissions();
        if (check != null) return check;

        var presupuesto = _repo.GetPresupuesto(id);
        if (presupuesto == null)
        {
            TempData["Error"] = "Presupuesto no encontrado";
            return RedirectToAction(nameof(Index));
        }
        return View(presupuesto);
    }

    [HttpPost, ActionName("EliminarConfirmado")]
    public IActionResult DeleteConfirmed(int idPresupuesto)
    {
        var check = CheckWritePermissions();
        if (check != null) return check;

        var presupuesto = _repo.GetPresupuesto(idPresupuesto);
        if (presupuesto == null)
        {
            TempData["Error"] = "Presupuesto no encontrado";
            return RedirectToAction(nameof(Index));
        }
        _repo.EliminarPresupuesto(idPresupuesto);
        TempData["Success"] = "Presupuesto Eliminado correctamente";
        return RedirectToAction(nameof(Index));
    }

    //GET y POST para agregar producto a un presupuesto

    //[HttpGet]
    public IActionResult AgregarProducto(int id)
    {
        var check = CheckPermissions();
        if (check != null) return check;
        
        List<Productos> productos = _productoRepo.ListarTodos();
        var modelo = new AgregarProductoViewModel
        {
            idPresupuesto = id,
            ListaProductos = new SelectList(productos, "idProducto", "descripcion")
        };

        return View(modelo);
    }

    [HttpPost]
    public IActionResult AgregarProducto(AgregarProductoViewModel modelo)
    {
        var check = CheckWritePermissions();
        if (check != null) return check;

        if (!ModelState.IsValid)
        {
            // ❌ LÓGICA CRÍTICA DE RECARGA: Si la validación falla (ej. Cantidad < 1),
            // Muestra todos los errores en la Consola/Debug Output de Visual Studio
            foreach (var modelStateKey in ModelState.Keys)
            {
                var modelStateVal = ModelState[modelStateKey];
                foreach (var error in modelStateVal.Errors)
                {
                    // Imprime el nombre del campo y el error de validación exacto.
                    Console.WriteLine($"Error en el campo '{modelStateKey}': {error.ErrorMessage}");
                }
            }
    
            // DEBEMOS recargar el SelectList antes de devolver la vista.
            var productos = _productoRepo.ListarTodos();
            modelo.ListaProductos = new SelectList(productos, "idProducto", "descripcion");

            // Devolvemos el modelo con los errores y el dropdown recargado
            return View(modelo);
        }
        _repo.AgregarProductos(modelo.idPresupuesto, modelo.idProducto, modelo.cantidad);

        TempData["Success"] = $"Producto agregado correctamente al presupuesto (Cantidad: {modelo.cantidad})";
        return RedirectToAction(nameof(Details), new { id = modelo.idPresupuesto });
    }

    
    [HttpGet]
    public IActionResult AccesoDenegado()
    {
        return View();
    }
}