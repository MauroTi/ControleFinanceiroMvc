using System.Data;
using Npgsql;

namespace ControleFinanceiroMvc.Data
{
    public class DbConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            var connectionString = _configuration.GetConnectionString("PostgreSqlConnection");

            if (string.IsNullOrWhiteSpace(connectionString) || connectionString.Contains("${", StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    "A connection string 'PostgreSqlConnection' não foi configurada. Defina um valor válido em User Secrets, appsettings.Development.json ou variáveis de ambiente.");
            }

            return new NpgsqlConnection(connectionString);
        }
    }
}
