using System.Data;
using System.Text.RegularExpressions;
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
            var connectionString = ResolveEnvironmentPlaceholders(
                _configuration.GetConnectionString("PostgreSqlConnection"));

            if (string.IsNullOrWhiteSpace(connectionString) || connectionString.Contains("${", StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    "A connection string 'PostgreSqlConnection' não foi configurada. Defina um valor válido em User Secrets, appsettings.Development.local.json, appsettings.Development.json ou variáveis de ambiente.");
            }

            return new NpgsqlConnection(connectionString);
        }

        private static string? ResolveEnvironmentPlaceholders(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            return Regex.Replace(value, @"\$\{(?<name>[A-Za-z_][A-Za-z0-9_]*)\}", match =>
            {
                var environmentValue = Environment.GetEnvironmentVariable(match.Groups["name"].Value);
                return string.IsNullOrWhiteSpace(environmentValue) ? match.Value : environmentValue;
            });
        }
    }
}
