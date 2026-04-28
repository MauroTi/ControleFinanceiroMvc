namespace ControleFinanceiroMvc.Models.ViewModels
{
    public class DashboardViewModel
    {
        public ResumoFinanceiroViewModel Resumo { get; set; } = new();
        public IReadOnlyList<LancamentoListaItemViewModel> UltimosLancamentos { get; set; } = Array.Empty<LancamentoListaItemViewModel>();
        public string? Aviso { get; set; }
    }
}
