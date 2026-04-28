using ControleFinanceiroMvc.Models.Entities;
using ControleFinanceiroMvc.Models.ViewModels;
using ControleFinanceiroMvc.Repositories;

namespace ControleFinanceiroMvc.Services
{
    public class LancamentoService : ILancamentoService
    {
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly ICategoriaService _categoriaService;

        public LancamentoService(ILancamentoRepository lancamentoRepository, ICategoriaService categoriaService)
        {
            _lancamentoRepository = lancamentoRepository;
            _categoriaService = categoriaService;
        }

        public async Task<LancamentosIndexViewModel> ObterIndexAsync(int usuarioId, LancamentoFiltroViewModel filtro)
        {
            filtro.Categorias = (await _categoriaService.ListarPorUsuarioIdAsync(usuarioId)).ToList();

            var lancamentos = await _lancamentoRepository.ListarAsync(usuarioId, filtro);
            var resumo = await _lancamentoRepository.ObterResumoAsync(usuarioId, filtro);

            return new LancamentosIndexViewModel
            {
                Filtro = filtro,
                Lancamentos = lancamentos,
                Resumo = resumo
            };
        }

        public async Task<LancamentoFormViewModel> CriarFormularioAsync(int usuarioId)
        {
            return new LancamentoFormViewModel
            {
                Categorias = (await _categoriaService.ListarPorUsuarioIdAsync(usuarioId)).ToList()
            };
        }

        public async Task<LancamentoFormViewModel?> ObterFormularioEdicaoAsync(int usuarioId, int lancamentoId)
        {
            var lancamento = await _lancamentoRepository.ObterPorIdAsync(usuarioId, lancamentoId);
            if (lancamento is null)
            {
                return null;
            }

            return new LancamentoFormViewModel
            {
                Id = lancamento.Id,
                Descricao = lancamento.Descricao,
                Valor = lancamento.Valor,
                Tipo = lancamento.Tipo,
                DataLancamento = lancamento.DataLancamento,
                Observacao = lancamento.Observacao,
                CategoriaId = lancamento.CategoriaId,
                Categorias = (await _categoriaService.ListarPorUsuarioIdAsync(usuarioId)).ToList()
            };
        }

        public async Task<(bool Sucesso, string? Erro)> CriarAsync(int usuarioId, LancamentoFormViewModel model)
        {
            if (!string.Equals(model.Tipo, "Receita", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(model.Tipo, "Despesa", StringComparison.OrdinalIgnoreCase))
            {
                return (false, "Tipo de lançamento inválido.");
            }

            var categorias = await _categoriaService.ListarPorUsuarioIdAsync(usuarioId);
            if (!categorias.Any(c => c.Id == model.CategoriaId))
            {
                return (false, "Categoria inválida para este usuário.");
            }

            var lancamento = new Lancamento
            {
                UsuarioId = usuarioId,
                CategoriaId = model.CategoriaId,
                Descricao = model.Descricao.Trim(),
                Valor = model.Valor,
                Tipo = model.Tipo.Trim(),
                DataLancamento = model.DataLancamento.Date,
                Observacao = string.IsNullOrWhiteSpace(model.Observacao) ? null : model.Observacao.Trim(),
                CriadoEm = DateTime.UtcNow
            };

            await _lancamentoRepository.CriarAsync(lancamento);
            return (true, null);
        }

        public async Task<(bool Sucesso, string? Erro)> AtualizarAsync(int usuarioId, LancamentoFormViewModel model)
        {
            if (!model.Id.HasValue)
            {
                return (false, "Lançamento inválido.");
            }

            if (!string.Equals(model.Tipo, "Receita", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(model.Tipo, "Despesa", StringComparison.OrdinalIgnoreCase))
            {
                return (false, "Tipo de lançamento inválido.");
            }

            var categorias = await _categoriaService.ListarPorUsuarioIdAsync(usuarioId);
            if (!categorias.Any(c => c.Id == model.CategoriaId))
            {
                return (false, "Categoria inválida para este usuário.");
            }

            var lancamentoExistente = await _lancamentoRepository.ObterPorIdAsync(usuarioId, model.Id.Value);
            if (lancamentoExistente is null)
            {
                return (false, "Lançamento não encontrado.");
            }

            lancamentoExistente.CategoriaId = model.CategoriaId;
            lancamentoExistente.Descricao = model.Descricao.Trim();
            lancamentoExistente.Valor = model.Valor;
            lancamentoExistente.Tipo = model.Tipo.Trim();
            lancamentoExistente.DataLancamento = model.DataLancamento.Date;
            lancamentoExistente.Observacao = string.IsNullOrWhiteSpace(model.Observacao) ? null : model.Observacao.Trim();
            lancamentoExistente.AtualizadoEm = DateTime.UtcNow;

            var atualizado = await _lancamentoRepository.AtualizarAsync(lancamentoExistente);
            return atualizado ? (true, null) : (false, "Lançamento não encontrado.");
        }

        public async Task<(bool Sucesso, string? Erro)> ExcluirAsync(int usuarioId, int lancamentoId)
        {
            var excluido = await _lancamentoRepository.ExcluirAsync(usuarioId, lancamentoId);
            return excluido ? (true, null) : (false, "Lançamento não encontrado.");
        }
    }
}
