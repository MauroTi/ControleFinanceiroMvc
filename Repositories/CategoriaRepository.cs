using ControleFinanceiroMvc.Data;
using ControleFinanceiroMvc.Models.Entities;
using Dapper;

namespace ControleFinanceiroMvc.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public CategoriaRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IReadOnlyList<Categoria>> ListarPorUsuarioIdAsync(int usuarioId)
        {
            const string sql = """
                select
                    id,
                    usuario_id as UsuarioId,
                    nome,
                    tipo,
                    ativo,
                    criado_em as CriadoEm,
                    atualizado_em as AtualizadoEm
                from categorias
                where usuario_id = @UsuarioId
                order by nome;
                """;

            using var connection = _connectionFactory.CreateConnection();
            var categorias = await connection.QueryAsync<Categoria>(sql, new { UsuarioId = usuarioId });
            return categorias.ToList();
        }

        public async Task<Categoria?> ObterPorIdAsync(int usuarioId, int categoriaId)
        {
            const string sql = """
                select
                    id,
                    usuario_id as UsuarioId,
                    nome,
                    tipo,
                    ativo,
                    criado_em as CriadoEm,
                    atualizado_em as AtualizadoEm
                from categorias
                where usuario_id = @UsuarioId
                  and id = @CategoriaId;
                """;

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Categoria>(sql, new { UsuarioId = usuarioId, CategoriaId = categoriaId });
        }

        public async Task<bool> ExisteComMesmoNomeAsync(int usuarioId, string nome, int? ignorarCategoriaId = null)
        {
            const string sql = """
                select exists (
                    select 1
                    from categorias
                    where usuario_id = @UsuarioId
                      and lower(nome) = lower(@Nome)
                      and (@IgnorarCategoriaId::integer is null or id <> @IgnorarCategoriaId::integer)
                );
                """;

            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<bool>(sql, new
            {
                UsuarioId = usuarioId,
                Nome = nome,
                IgnorarCategoriaId = ignorarCategoriaId
            });
        }

        public async Task<int> CriarAsync(Categoria categoria)
        {
            const string sql = """
                insert into categorias (usuario_id, nome, tipo, ativo, criado_em)
                values (@UsuarioId, @Nome, @Tipo, @Ativo, @CriadoEm)
                returning id;
                """;

            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, categoria);
        }

        public async Task<bool> AtualizarAsync(Categoria categoria)
        {
            const string sql = """
                update categorias
                set
                    nome = @Nome,
                    tipo = @Tipo,
                    atualizado_em = @AtualizadoEm
                where usuario_id = @UsuarioId
                  and id = @Id;
                """;

            using var connection = _connectionFactory.CreateConnection();
            var linhasAfetadas = await connection.ExecuteAsync(sql, categoria);
            return linhasAfetadas > 0;
        }

        public async Task<bool> ExcluirAsync(int usuarioId, int categoriaId)
        {
            const string sql = """
                delete from categorias
                where usuario_id = @UsuarioId
                  and id = @CategoriaId;
                """;

            using var connection = _connectionFactory.CreateConnection();
            var linhasAfetadas = await connection.ExecuteAsync(sql, new { UsuarioId = usuarioId, CategoriaId = categoriaId });
            return linhasAfetadas > 0;
        }
    }
}
