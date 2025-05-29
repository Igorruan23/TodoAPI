using System.ComponentModel.DataAnnotations;

namespace TodoAPI.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Usuario Obrigatório")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "A senha deve conter no mínimo 8 caracteres, incluindo uma letra maiúscula," +
            " uma minúscula, um número e um caractere especial.")]
        [Required(ErrorMessage = "Senha Obrigatoria")]
        public string Password { get; set; }
    }
}
