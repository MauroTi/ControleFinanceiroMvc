using ControleFinanceiroMvc.Models.Entities;
using ControleFinanceiroMvc.Models.ViewModels;
using ControleFinanceiroMvc.Repositories;
using Npgsql;

namespace ControleFinanceiroMvc.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<IReadOnlyList<CategoriaViewModel>> ListarPorUsuarioIdAsync(int usuarioId)
        {
            var categorias = await _categoriaRepository.ListarPorUsuarioIdAsync(usuarioId);

            return categorias
                .Select(categoria => new CategoriaViewModel
                {
                    Id = categoria.Id,
                    Nome = categoria.Nome,
                    Tipo = categoria.Tipo
                })
                .ToList();
        }

        public async Task<CategoriaViewModel?> ObterParaEdicaoAsync(int usuarioId, int categoriaId)
        {
            var categoria = await _categoriaRepository.ObterPorIdAsync(usuarioId, categoriaId);

            if (categoria is null)
            {
                return null;
            }

            return new CategoriaViewModel
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                Tipo = categoria.Tipo
            };
        }

        public async Task<(bool Sucesso, string? Erro)> CriarAsync(int usuarioId, CategoriaViewModel model)
        {
            var nome = model.Nome.Trim();
            var tipo = model.Tipo.Trim();

            if (await _categoriaRepository.ExisteComMesmoNomeAsync(usuarioId, nome))
            {
                return (false, "Já existe uma categoria com esse nome para este usuário.");
            }

            var categoria = new Categoria
            {
                UsuarioId = usuarioId,
                Nome = nome,
                Tipo = tipo,
                Ativo = true,
                CriadoEm = DateTime.UtcNow
            };

            await _categoriaRepository.CriarAsync(categoria);
            return (true, null);
        }

        public async Task<(bool Sucesso, string? Erro)> AtualizarAsync(int usuarioId, CategoriaViewModel model)
        {
            var nome = model.Nome.Trim();
            var tipo = model.Tipo.Trim();

            var categoriaExistente = await _categoriaRepository.ObterPorIdAsync(usuarioId, model.Id);
            if (categoriaExistente is null)
            {
                return (false, "Categoria não encontrada.");
            }

            if (await _categoriaRepository.ExisteComMesmoNomeAsync(usuarioId, nome, model.Id))
            {
                return (false, "Já existe outra categoria com esse nome para este usuário.");
            }

            categoriaExistente.Nome = nome;
            categoriaExistente.Tipo = tipo;
            categoriaExistente.AtualizadoEm = DateTime.UtcNow;

            var atualizado = await _categoriaRepository.AtualizarAsync(categoriaExistente);
            return atualizado ? (true, null) : (false, "Categoria não encontrada.");
        }

        public async Task<(bool Sucesso, string? Erro)> ExcluirAsync(int usuarioId, int categoriaId)
        {
            try
            {
                var excluido = await _categoriaRepository.ExcluirAsync(usuarioId, categoriaId);
                return excluido ? (true, null) : (false, "Categoria não encontrada.");
            }
            catch (PostgresException ex) when (ex.SqlState == "23503")
            {
                return (false, "Não é possível excluir a categoria porque ela possui lançamentos vinculados.");
            }
        }
    }
}
