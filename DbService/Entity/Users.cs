using System.ComponentModel.DataAnnotations;

namespace DbService.Entity
{
    public class Users :Base
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string Mail { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } 
    }
}
