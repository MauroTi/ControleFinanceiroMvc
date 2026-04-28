using ControleFinanceiroMvc.Data;
using ControleFinanceiroMvc.Models.Entities;
using ControleFinanceiroMvc.Models.ViewModels;
using Dapper;

namespace ControleFinanceiroMvc.Repositories
{
    public class LancamentoRepository : ILancamentoRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public LancamentoRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IReadOnlyList<LancamentoListaItemViewModel>> ListarAsync(int usuarioId, LancamentoFiltroViewModel filtro)
        {
            const string sql = """
                select
                    l.id,
                    l.descricao,
                    l.valor,
                    l.tipo,
                    l.data_lancamento::timestamp as DataLancamento,
                    c.nome as CategoriaNome,
                    l.observacao
                from lancamentos l
                inner join categorias c on c.id = l.categoria_id
                where l.usuario_id = @UsuarioId
                  and (@CategoriaId::integer is null or l.categoria_id = @CategoriaId::integer)
                  and (@Tipo::text is null or l.tipo = @Tipo::text)
                  and (@DataInicio::date is null or l.data_lancamento >= @DataInicio::date)
                  and (@DataFim::date is null or l.data_lancamento <= @DataFim::date)
                order by l.data_lancamento desc, l.id desc;
                """;

            using var connection = _connectionFactory.CreateConnection();
            var lancamentos = await connection.QueryAsync<LancamentoListaItemViewModel>(sql, new
            {
                UsuarioId = usuarioId,
                filtro.CategoriaId,
                filtro.Tipo,
                filtro.DataInicio,
                filtro.DataFim
            });

            return lancamentos.ToList();
        }

        public async Task<ResumoFinanceiroViewModel> ObterResumoAsync(int usuarioId, LancamentoFiltroViewModel filtro)
        {
            const string sql = """
                select
                    coalesce(sum(case when l.tipo = 'Receita' then l.valor else 0 end), 0) as TotalReceitas,
                    coalesce(sum(case when l.tipo = 'Despesa' then l.valor else 0 end), 0) as TotalDespesas
                from lancamentos l
                where l.usuario_id = @UsuarioId
                  and (@CategoriaId::integer is null or l.categoria_id = @CategoriaId::integer)
                  and (@Tipo::text is null or l.tipo = @Tipo::text)
                  and (@DataInicio::date is null or l.data_lancamento >= @DataInicio::date)
                  and (@DataFim::date is null or l.data_lancamento <= @DataFim::date);
                """;

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleAsync<ResumoFinanceiroViewModel>(sql, new
            {
                UsuarioId = usuarioId,
                filtro.CategoriaId,
                filtro.Tipo,
                filtro.DataInicio,
                filtro.DataFim
            });
        }

        public async Task<Lancamento?> ObterPorIdAsync(int usuarioId, int lancamentoId)
        {
            const string sql = """
                select
                    id,
                    usuario_id as UsuarioId,
                    categoria_id as CategoriaId,
                    descricao,
                    valor,
                    tipo,
                    data_lancamento::timestamp as DataLancamento,
                    observacao,
                    criado_em as CriadoEm,
                    atualizado_em as AtualizadoEm
                from lancamentos
                where usuario_id = @UsuarioId
                  and id = @LancamentoId;
                """;

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Lancamento>(sql, new { UsuarioId = usuarioId, LancamentoId = lancamentoId });
        }

        public async Task<int> CriarAsync(Lancamento lancamento)
        {
            const string sql = """
                insert into lancamentos
                    (usuario_id, categoria_id, descricao, valor, tipo, data_lancamento, observacao, criado_em)
                values
                    (@UsuarioId, @CategoriaId, @Descricao, @Valor, @Tipo, @DataLancamento, @Observacao, @CriadoEm)
                returning id;
                """;

            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, lancamento);
        }

        public async Task<bool> AtualizarAsync(Lancamento lancamento)
        {
            const string sql = """
                update lancamentos
                set
                    categoria_id = @CategoriaId,
                    descricao = @Descricao,
                    valor = @Valor,
                    tipo = @Tipo,
                    data_lancamento = @DataLancamento,
                    observacao = @Observacao,
                    atualizado_em = @AtualizadoEm
                where usuario_id = @UsuarioId
                  and id = @Id;
                """;

            using var connection = _connectionFactory.CreateConnection();
            var linhasAfetadas = await connection.ExecuteAsync(sql, lancamento);
            return linhasAfetadas > 0;
        }

        public async Task<bool> ExcluirAsync(int usuarioId, int lancamentoId)
        {
            const string sql = """
                delete from lancamentos
                where usuario_id = @UsuarioId
                  and id = @LancamentoId;
                """;

            using var connection = _connectionFactory.CreateConnection();
            var linhasAfetadas = await connection.ExecuteAsync(sql, new { UsuarioId = usuarioId, LancamentoId = lancamentoId });
            return linhasAfetadas > 0;
        }
    }
}
