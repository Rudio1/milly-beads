using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MillyBeads.Models.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        [Display(Name = "Nome")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        [Display(Name = "Descrição")]
        public required string Description { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero")]
        [Display(Name = "Preço")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "Imagem")]
        public IFormFile? ImageFile { get; set; }

        public string? ImagePath { get; set; }
    }
} 