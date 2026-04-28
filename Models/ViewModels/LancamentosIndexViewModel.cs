namespace ControleFinanceiroMvc.Models.ViewModels
{
    public class LancamentosIndexViewModel
    {
        public LancamentoFiltroViewModel Filtro { get; set; } = new();
        public IReadOnlyList<LancamentoListaItemViewModel> Lancamentos { get; set; } = Array.Empty<LancamentoListaItemViewModel>();
        public ResumoFinanceiroViewModel Resumo { get; set; } = new();
    }
}
