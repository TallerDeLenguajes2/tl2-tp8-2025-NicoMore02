using MVC.Models;

namespace MVC.Interfaces;

public interface IProductoRepository
{
    void CrearProducto(Productos producto);

    void ActualizarProducto(int id, Productos producto);

    List<Productos> ListarTodos();

    Productos BuscarPorId(int id);

    void EliminarProducto(int id);
}