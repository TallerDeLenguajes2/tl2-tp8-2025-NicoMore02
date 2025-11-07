using System.ComponentModel.DataAnnotations;


namespace SistemaVentas.Web.ViewModels
{
    public class ProductoViewModel
    {
        public int idProducto { get; set; }
        [Display(Name = "Descripcion del Producto")]
        [StringLength(250, ErrorMessage = "La descripcion no puede superar los 250 caracteres.")]
        public string descripcion { get; set; }

        [Display(Name = "Precio Unitario")]
        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio deber ser un valor positivo.")]
        public decimal precio { get; set; }

    }
}