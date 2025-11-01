namespace tl2_tp8_2025_NicoMore02.Models;
public class Presupuestos
{
    public int idPresupuesto { get; set; }
    public string NombreDestinatario { get; set; }
    public DateTime FechaCreacion { get; set; }
    public List<PresupuestosDetalle> detalle { get; set; } = new List<PresupuestosDetalle>();

    public int MontoPresupuesto()
    {
        //detalle = new List<PresupuestosDetalle>();

        int total = 0;
        foreach (var detalles in detalle)
        {
            total += detalles.producto.precio * detalles.cantidad; 
        }
        return total;
    }

    public double MontoPresupuestoConIva()
    {
        int monto = MontoPresupuesto();
        double total = monto + (monto * 0.21);
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