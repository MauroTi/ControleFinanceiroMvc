using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiroMvc.Models.ViewModels
{
    public class CategoriaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(80, ErrorMessage = "Nome deve ter no máximo 80 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tipo é obrigatório")]
        [StringLength(30, ErrorMessage = "Tipo deve ter no máximo 30 caracteres")]
        public string Tipo { get; set; } = string.Empty;
    }
}
