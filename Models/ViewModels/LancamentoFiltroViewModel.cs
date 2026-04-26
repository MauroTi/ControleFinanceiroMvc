namespace ControleFinanceiroMvc.Models.ViewModels
{
    public class LancamentoFiltroViewModel
    {
        public int? CategoriaId { get; set; }

        public string? Tipo { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        public List<CategoriaViewModel> Categorias { get; set; } = new();
    }
}