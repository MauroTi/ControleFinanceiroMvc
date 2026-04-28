using ControleFinanceiroMvc.Models.Entities;

namespace ControleFinanceiroMvc.Repositories
{
    public interface ICategoriaRepository
    {
        Task<IReadOnlyList<Categoria>> ListarPorUsuarioIdAsync(int usuarioId);
        Task<Categoria?> ObterPorIdAsync(int usuarioId, int categoriaId);
        Task<bool> ExisteComMesmoNomeAsync(int usuarioId, string nome, int? ignorarCategoriaId = null);
        Task<int> CriarAsync(Categoria categoria);
        Task<bool> AtualizarAsync(Categoria categoria);
        Task<bool> ExcluirAsync(int usuarioId, int categoriaId);
    }
}
