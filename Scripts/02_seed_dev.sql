insert into usuarios (id, nome, email, senha_hash, ativo, email_confirmado, criado_em, provedor_login)
values (1, 'Usuário Dev', 'dev@controlefinanceiro.local', null, true, true, now(), 'Local')
on conflict (id) do nothing;

select setval(pg_get_serial_sequence('usuarios', 'id'), coalesce((select max(id) from usuarios), 1), true);

insert into categorias (usuario_id, nome, tipo, ativo, criado_em)
values
    (1, 'Salário', 'Receita', true, now()),
    (1, 'Freelance', 'Receita', true, now()),
    (1, 'Investimentos', 'Receita', true, now()),
    (1, 'Reembolsos', 'Receita', true, now()),
    (1, 'Moradia', 'Despesa', true, now()),
    (1, 'Alimentação', 'Despesa', true, now()),
    (1, 'Transporte', 'Despesa', true, now()),
    (1, 'Saúde', 'Despesa', true, now()),
    (1, 'Educação', 'Despesa', true, now()),
    (1, 'Lazer', 'Despesa', true, now()),
    (1, 'Assinaturas', 'Despesa', true, now())
on conflict do nothing;

with lancamentos_seed (usuario_id, categoria_nome, descricao, valor, tipo, data_lancamento, observacao) as (
    values
        (1, 'Salário', 'Salário mensal', 5200.00, 'Receita', current_date - interval '27 days', 'Pagamento mensal'),
        (1, 'Freelance', 'Projeto landing page', 1350.00, 'Receita', current_date - interval '22 days', 'Serviço avulso'),
        (1, 'Investimentos', 'Rendimento CDB', 86.45, 'Receita', current_date - interval '12 days', 'Rendimento mensal'),
        (1, 'Reembolsos', 'Reembolso transporte', 74.90, 'Receita', current_date - interval '8 days', 'Reembolso corporativo'),
        (1, 'Moradia', 'Aluguel', 1650.00, 'Despesa', current_date - interval '26 days', 'Vencimento mensal'),
        (1, 'Moradia', 'Conta de energia', 214.78, 'Despesa', current_date - interval '19 days', 'Consumo residencial'),
        (1, 'Moradia', 'Internet residencial', 119.90, 'Despesa', current_date - interval '16 days', 'Plano fibra'),
        (1, 'Alimentação', 'Supermercado', 486.32, 'Despesa', current_date - interval '14 days', 'Compra do mês'),
        (1, 'Alimentação', 'Restaurante', 72.50, 'Despesa', current_date - interval '6 days', 'Almoço'),
        (1, 'Transporte', 'Combustível', 210.00, 'Despesa', current_date - interval '11 days', 'Abastecimento'),
        (1, 'Transporte', 'Aplicativo de transporte', 38.70, 'Despesa', current_date - interval '4 days', 'Deslocamento'),
        (1, 'Saúde', 'Farmácia', 96.40, 'Despesa', current_date - interval '9 days', 'Medicamentos'),
        (1, 'Educação', 'Curso online', 129.90, 'Despesa', current_date - interval '18 days', 'Mensalidade'),
        (1, 'Lazer', 'Cinema', 64.00, 'Despesa', current_date - interval '5 days', 'Fim de semana'),
        (1, 'Assinaturas', 'Streaming', 39.90, 'Despesa', current_date - interval '2 days', 'Assinatura mensal')
)
insert into lancamentos (usuario_id, categoria_id, descricao, valor, tipo, data_lancamento, observacao, criado_em)
select
    seed.usuario_id,
    categoria.id,
    seed.descricao,
    seed.valor,
    seed.tipo,
    seed.data_lancamento::date,
    seed.observacao,
    now()
from lancamentos_seed seed
inner join categorias categoria
    on categoria.usuario_id = seed.usuario_id
   and lower(categoria.nome) = lower(seed.categoria_nome)
where not exists (
    select 1
    from lancamentos lancamento
    where lancamento.usuario_id = seed.usuario_id
      and lancamento.descricao = seed.descricao
      and lancamento.valor = seed.valor
      and lancamento.tipo = seed.tipo
      and lancamento.data_lancamento = seed.data_lancamento::date
);

with categorias_ordenadas as (
    select
        id,
        nome,
        tipo,
        row_number() over (order by tipo, nome) as rn,
        count(*) over () as total
    from categorias
    where usuario_id = 1
      and ativo = true
),
faltantes as (
    select greatest(0, 200 - count(*)) as qtd
    from lancamentos
    where usuario_id = 1
),
sequencia as (
    select generate_series(1, (select qtd from faltantes)) as n
)
insert into lancamentos (usuario_id, categoria_id, descricao, valor, tipo, data_lancamento, observacao, criado_em)
select
    1 as usuario_id,
    categoria.id as categoria_id,
    format('%s %s %s', categoria.nome, to_char(current_date - ((n % 120) || ' days')::interval, 'YYYYMMDD'), lpad(n::text, 3, '0')) as descricao,
    round((25 + ((n * 37) % 900) + ((n % 100) / 100.0))::numeric, 2) as valor,
    categoria.tipo as tipo,
    (current_date - ((n % 120) || ' days')::interval)::date as data_lancamento,
    format('Lançamento automático %s', lpad(n::text, 3, '0')) as observacao,
    now() as criado_em
from sequencia
inner join categorias_ordenadas categoria
    on categoria.rn = ((sequencia.n - 1) % categoria.total) + 1;
