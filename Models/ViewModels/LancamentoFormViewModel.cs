using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiroMvc.Models.ViewModels
{
    public class LancamentoFormViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Descrição é obrigatória")]
        [StringLength(150)]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "Valor é obrigatório")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "Tipo é obrigatório")]
        public string Tipo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Data é obrigatória")]
        public DateTime DataLancamento { get; set; } = DateTime.Today;

        public string? Observacao { get; set; }

        [Required(ErrorMessage = "Categoria é obrigatória")]
        public int CategoriaId { get; set; }

        // 🔥 Sem ViewBag → dropdown vem daqui
        public List<CategoriaViewModel> Categorias { get; set; } = new();
    }
}
