namespace MVC.Models;
public class Presupuestos
{
    public int idPresupuesto { get; set; }
    public string NombreDestinatario { get; set; }
    public DateTime FechaCreacion { get; set; }
    public List<PresupuestosDetalle> detalle { get; set; } = new List<PresupuestosDetalle>();

    public decimal MontoPresupuesto()
    {
        //detalle = new List<PresupuestosDetalle>();

        decimal total = 0;
        foreach (var detalles in detalle)
        {
            total += detalles.producto.precio * detalles.cantidad; 
        }
        return total;
    }

    public decimal MontoPresupuestoConIva()
    {
        decimal monto = MontoPresupuesto();
        decimal total = monto + (monto * 0.21m);
        return total;
    }

    public int CantidadProductos()
    {
        //detalle = new List<PresupuestosDetalle>();

        int total = 0;
        foreach (var detalles in detalle)
        {
            total += detalles.cantidad;
        }
        return total;
    }
}