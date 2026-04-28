# Scripts SQL

Use os scripts nesta ordem:

1. `01_schema.sql`
2. `02_seed_dev.sql`

Exemplo com `psql`:

```powershell
psql -U postgres -d controle_financeiro_db -f .\Scripts\01_schema.sql
psql -U postgres -d controle_financeiro_db -f .\Scripts\02_seed_dev.sql
```

O seed cria o usuário de desenvolvimento com `id = 1`, que é o mesmo valor temporário usado hoje pelos controllers até a autenticação ser implementada.
