insert into usuarios (id, nome, email, senha_hash, ativo, email_confirmado, criado_em, provedor_login)
values (1, 'Usuário Dev', 'dev@controlefinanceiro.local', null, true, true, now(), 'Local')
on conflict (id) do nothing;

insert into categorias (usuario_id, nome, tipo, ativo, criado_em)
values
    (1, 'Salário', 'Receita', true, now()),
    (1, 'Freelance', 'Receita', true, now()),
    (1, 'Moradia', 'Despesa', true, now()),
    (1, 'Alimentação', 'Despesa', true, now())
on conflict do nothing;
