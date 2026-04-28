using ControleFinanceiroMvc.Models.ViewModels;

namespace ControleFinanceiroMvc.Services
{
    public interface ILancamentoService
    {
        Task<LancamentosIndexViewModel> ObterIndexAsync(int usuarioId, LancamentoFiltroViewModel filtro);
        Task<LancamentoFormViewModel> CriarFormularioAsync(int usuarioId);
        Task<LancamentoFormViewModel?> ObterFormularioEdicaoAsync(int usuarioId, int lancamentoId);
        Task<(bool Sucesso, string? Erro)> CriarAsync(int usuarioId, LancamentoFormViewModel model);
        Task<(bool Sucesso, string? Erro)> AtualizarAsync(int usuarioId, LancamentoFormViewModel model);
        Task<(bool Sucesso, string? Erro)> ExcluirAsync(int usuarioId, int lancamentoId);
    }
}
