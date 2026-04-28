using ControleFinanceiroMvc.Models.Entities;
using ControleFinanceiroMvc.Models.ViewModels;

namespace ControleFinanceiroMvc.Repositories
{
    public interface ILancamentoRepository
    {
        Task<IReadOnlyList<LancamentoListaItemViewModel>> ListarAsync(int usuarioId, LancamentoFiltroViewModel filtro);
        Task<ResumoFinanceiroViewModel> ObterResumoAsync(int usuarioId, LancamentoFiltroViewModel filtro);
        Task<Lancamento?> ObterPorIdAsync(int usuarioId, int lancamentoId);
        Task<int> CriarAsync(Lancamento lancamento);
        Task<bool> AtualizarAsync(Lancamento lancamento);
        Task<bool> ExcluirAsync(int usuarioId, int lancamentoId);
    }
}
