using MVC.Models;

namespace MVC.Interfaces;

public interface IPresupuestoRepository
{
    void CrearPresupuesto(Presupuestos presupuesto);

    List<Presupuestos> ListarPresupuestos();

    Presupuestos GetPresupuesto(int id);

    void AgregarProductos(int idPresupuesto, int idProducto, int cantidad);

    void EliminarPresupuesto(int id);

    void ActualizarPresupuesto(Presupuestos presupuesto);
}