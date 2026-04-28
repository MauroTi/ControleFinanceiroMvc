-- Execute este script conectado ao banco administrativo "postgres".
-- No DBeaver: abra a conexão do PostgreSQL, selecione o banco "postgres"
-- e execute este arquivo antes dos scripts de tabelas.

create database controle_financeiro_db
    with
    owner = postgres
    encoding = 'UTF8'
    template = template0;
