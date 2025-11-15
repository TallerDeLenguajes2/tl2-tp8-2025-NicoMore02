using System.ComponentModel.DataAnnotations;

namespace SistemaVentas.Web.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "El usuario es obligatorio")]
    [Display(Name = "Usuario")]
    public string Username { get; set; }

    [Required(ErrorMessage ="La Contraseña es obligatoria")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; }

    public string ErrorMessage { get; set; }
}