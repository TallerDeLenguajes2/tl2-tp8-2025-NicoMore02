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
    public IActionResult Details()
    {
        List<Presupuestos> presupuestos = presupuestosRepository.ListarPresupuestos();
        return View(presupuestos);
    }
}