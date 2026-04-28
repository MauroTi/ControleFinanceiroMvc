using ControleFinanceiroMvc.Models.ViewModels;

namespace ControleFinanceiroMvc.Services
{
    public interface ICategoriaService
    {
        Task<IReadOnlyList<CategoriaViewModel>> ListarPorUsuarioIdAsync(int usuarioId);
        Task<CategoriaViewModel?> ObterParaEdicaoAsync(int usuarioId, int categoriaId);
        Task<(bool Sucesso, string? Erro)> CriarAsync(int usuarioId, CategoriaViewModel model);
        Task<(bool Sucesso, string? Erro)> AtualizarAsync(int usuarioId, CategoriaViewModel model);
        Task<(bool Sucesso, string? Erro)> ExcluirAsync(int usuarioId, int categoriaId);
    }
}
