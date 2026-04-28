# Scripts SQL

Use os scripts nesta ordem:

1. `00_create_database.sql`
2. `01_schema.sql`
3. `02_seed_dev.sql`

No DBeaver, execute o `00_create_database.sql` conectado ao banco administrativo `postgres`.
Depois conecte/abra o banco `controle_financeiro_db` e execute os demais scripts nessa base.

Exemplo com `psql`:

```powershell
psql -U postgres -d postgres -f .\Scripts\00_create_database.sql
psql -U postgres -d controle_financeiro_db -f .\Scripts\01_schema.sql
psql -U postgres -d controle_financeiro_db -f .\Scripts\02_seed_dev.sql
```

O seed cria o usuário de desenvolvimento com `id = 1`, categorias padrão e lançamentos de exemplo para popular a tela inicial. O script também completa automaticamente até 200 lançamentos para o usuário dev. Esse `id = 1` é o mesmo valor temporário usado hoje pelos controllers até a autenticação ser implementada.
