using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SistemaVentas.Web.ViewModels
{
    public class PresupuestoViewModel
    {
        public int idPresupuesto { get; set; }

        [Display(Name = "Nombre o Email del destinatario")]
        [Required(ErrorMessage = "El Nombre o el Email es obligatorio")]
        public string NombreDestinatario { get; set; }

        [Display(Name = "Fecha de Creacion")]
        [Required(ErrorMessage = "La fecha es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime FechaCreacion { get; set; }
    }
}