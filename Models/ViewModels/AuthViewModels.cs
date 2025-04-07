using System.ComponentModel.DataAnnotations;

namespace MillyBeads.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter entre {2} e {1} caracteres", MinimumLength = 3)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [StringLength(100, ErrorMessage = "A senha deve ter entre {2} e {1} caracteres", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Compare("Password", ErrorMessage = "As senhas não conferem")]
        [DataType(DataType.Password)]
        public required string ConfirmPassword { get; set; }
    }

    public class AuthResponseViewModel
    {
        public required string Token { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
    }
} 