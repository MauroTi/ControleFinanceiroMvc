namespace ControleFinanceiroMvc.Models.ViewModels
{
    public class LancamentoListaItemViewModel
    {
        public int Id { get; set; }

        public string Descricao { get; set; } = string.Empty;

        public decimal Valor { get; set; }

        public string Tipo { get; set; } = string.Empty;

        public DateTime DataLancamento { get; set; }

        public string CategoriaNome { get; set; } = string.Empty;

        public string? Observacao { get; set; }
    }
}