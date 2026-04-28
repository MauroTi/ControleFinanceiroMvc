# ControleFinanceiroMvc

Aplicação ASP.NET MVC para controle financeiro pessoal.

## Pré-requisitos

- .NET SDK 8.0+
- PostgreSQL 14+ (ou compatível)
- DBeaver (opcional, recomendado para executar scripts SQL)

## Configuração Rápida (qualquer PC)

1. Restaurar e compilar:

```powershell
dotnet restore
dotnet build
```

2. Criar e popular banco:

- Execute os scripts em `Scripts/README.md`
- Ordem:
  - `Scripts/00_create_database.sql`
  - `Scripts/01_schema.sql`
  - `Scripts/02_seed_dev.sql`

3. Configurar conexão (escolha uma opção):

- `appsettings.Development.local.json` (local, não versionado):
  - use `appsettings.Development.local.example.json` como modelo
- User Secrets:

```powershell
dotnet user-secrets set "ConnectionStrings:PostgreSqlConnection" "Host=localhost;Port=5432;Database=controle_financeiro_db;Username=postgres;Password=1234"
```

- Variáveis de ambiente:
  - use `.env.example` como referência para os nomes
  - ou defina diretamente `ConnectionStrings__PostgreSqlConnection`

4. Executar:

```powershell
dotnet run
```

## Compatibilidade de Conexão

O projeto suporta:

- connection string completa em `ConnectionStrings:PostgreSqlConnection`
- placeholders no `appsettings.json` (`${DB_HOST}`, `${DB_PORT}`, `${DB_NAME}`, `${DB_USER}`, `${DB_PASSWORD}`) resolvidos por variáveis de ambiente
- User Secrets para ambiente local
