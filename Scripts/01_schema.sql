create table if not exists usuarios (
    id serial primary key,
    nome varchar(150) not null,
    email varchar(150) not null unique,
    senha_hash varchar(255),
    ativo boolean not null default true,
    email_confirmado boolean not null default false,
    ultimo_login_em timestamp,
    criado_em timestamp not null default now(),
    atualizado_em timestamp,
    provedor_login varchar(50) not null default 'Local',
    provedor_id varchar(150),
    foto_url varchar(500)
);

create table if not exists categorias (
    id serial primary key,
    usuario_id integer not null references usuarios(id) on delete cascade,
    nome varchar(80) not null,
    tipo varchar(30) not null,
    ativo boolean not null default true,
    criado_em timestamp not null default now(),
    atualizado_em timestamp
);

create unique index if not exists ux_categorias_usuario_nome
    on categorias (usuario_id, lower(nome));

create table if not exists lancamentos (
    id serial primary key,
    usuario_id integer not null references usuarios(id) on delete cascade,
    categoria_id integer not null references categorias(id) on delete restrict,
    descricao varchar(150) not null,
    valor numeric(14,2) not null check (valor > 0),
    tipo varchar(30) not null,
    data_lancamento date not null,
    observacao text,
    criado_em timestamp not null default now(),
    atualizado_em timestamp
);

create index if not exists ix_lancamentos_usuario_data
    on lancamentos (usuario_id, data_lancamento desc);

create index if not exists ix_lancamentos_categoria
    on lancamentos (categoria_id);

create table if not exists tokens_recuperacao_senha (
    id serial primary key,
    usuario_id integer not null references usuarios(id) on delete cascade,
    token varchar(200) not null unique,
    expiracao_em timestamp not null,
    utilizado boolean not null default false,
    utilizado_em timestamp,
    criado_em timestamp not null default now()
);

create table if not exists emails_enviados (
    id serial primary key,
    usuario_id integer references usuarios(id) on delete set null,
    email_destino varchar(150) not null,
    assunto varchar(200) not null,
    corpo text not null,
    enviado boolean not null default false,
    erro text,
    criado_em timestamp not null default now(),
    enviado_em timestamp
);
