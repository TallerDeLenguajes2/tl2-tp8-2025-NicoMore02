using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaVentas.Web.ViewModels
{
    public class AgregarProductoViewModel
    {
        public int idPresupuesto { get; set; }

        [Display(Name = "Agregar Producto")]
        [Required(ErrorMessage = "Debe seleccionar un producto")]
        public int idProducto { get; set; }

        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "La cantidad es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero")]
        public int cantidad { get; set; }

        public SelectList ListaProductos { get; set; }
    }
}