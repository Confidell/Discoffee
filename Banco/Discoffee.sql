drop database if exists dbDiscoffee;
create database dbDiscoffee;
use dbDiscoffee;

-- TABELAS
create table tbPerfil(
Id_Perfil int primary key auto_increment,
Tipo enum('Cliente', 'Funcionario', 'Administrador') not null unique
);

/* CRIA OS 3 TIPOS DE PERFIS NA TABELA PERFIL */
insert into tbPerfil(Tipo) values(1);
insert into tbPerfil(Tipo) values(2);
insert into tbPerfil(Tipo) values(3);

select * from tbPerfil;

create table tbCEP(
Id_CEP int primary key auto_increment,
CEP varchar(9) unique
);

create table tbCupom(
Id_Cupom int primary key auto_increment,
Codigo_Cupom varchar(10) not null unique,
Desconto decimal(10,2) not null,
DataValidade datetime
);

create table tbCategoria(
Id_Categoria int primary key auto_increment,
Categoria varchar(50)
);

create table tbUsuario(
Id_Usuario int primary key auto_increment,
NomeCompleto varchar(150) not null,
Email varchar(100) not null unique,
Senha varchar(100) not null,
CPF varchar(15) not null unique,
Stats enum('Ativo','Inativo') default 'Ativo',
Telefone varchar(15),
DataNasc date,
Numero_Casa varchar(10) not null,
Complemento varchar(50) default 'Sem Complemento',
Id_Perfil int,
Id_CEP int,
constraint FK_Id_Perfil foreign key(Id_Perfil) references tbPerfil(Id_Perfil),
constraint FK_Id_CEP foreign key(Id_CEP) references tbCEP(Id_CEP) 
);

create table tbCupom_Utilizado(
Id_Cupom_Utilizado int primary key auto_increment,
Id_Usuario int,
Id_Cupom int,
constraint FK_Id_Usuario foreign key(Id_Usuario) references tbUsuario(Id_Usuario),
constraint FK_Id_Cupom foreign key(Id_Cupom) references tbCupom(Id_Cupom),
unique(Id_Usuario, Id_Cupom)
);

create table tbPedido(
Id_Pedido int primary key auto_increment,
Tipo enum('Retirada','Entrega') not null,
Stats enum('Pendente', 'Preparando', 'Finalizado', 'Em Entrega', 'Entregue', 'Cancelado') not null,
SubTotal decimal(10,2),
Total decimal(10,2),
DataHora datetime default current_timestamp,
Frete decimal(10,2),
Desconto decimal(10,2),
Id_Usuario int,
constraint FK_Id_USusario_Pedido foreign key(Id_Usuario) references tbUsuario(Id_Usuario)
);

create table tbProduto(
Id_Produto int primary key auto_increment,
Nome varchar(50) not null,	
Preco decimal(10,2) not null,
Descricao varchar(200) not null,
Imagem varchar(100) not null,
Disponibilidade boolean not null default true,
Id_Categoria int,
constraint FK_Id_Categoria_Produto foreign key(Id_Categoria) references tbCategoria(Id_Categoria)
);

create table tbAvaliacao(
Id_Avaliacao int primary key auto_increment,
Comentario varchar(100) not null,
Nota int check(Nota between 1 and 5) not null,
DataAval datetime default current_timestamp,
Id_Produto int,
Id_Usuario int,
constraint fk_Avaliacao_tbUsuario foreign key (Id_Usuario) references tbUsuario(Id_Usuario),
constraint fk_Avaliacao_tbProduto foreign key (Id_Produto) references tbProduto(Id_Produto)
);

create table tbCartao(
Id_Cartao int primary key auto_increment,
Nome_Titular varchar(75) not null,
Apelido varchar(50),
Numero_Mascarado varchar(100) not null,
DataVal date not null,
Id_Usuario int,
constraint FK_Cartao_tbUsuario foreign key (Id_Usuario) references tbUsuario(Id_Usuario)
);

create table tbItem_Pedido(
Id_Item int primary key auto_increment,
Preco_Unitario decimal(10,2),
Quantidade smallint,
Id_Pedido int,
Id_Produto int,
constraint FK_Item_Pedido_tbPedido foreign key (Id_Pedido) references tbPedido(Id_Pedido),
constraint FK_Item_Pedido_tbProduto foreign key (Id_Produto) references tbProduto(Id_Produto)
);

create table tbOpcao_Personalizacao(
Id_Opcao int primary key auto_increment,
Tipo enum('Leite', 'Acucar', 'Adicional') not null,
Opcao varchar(50) not null unique,
Preco_Extra decimal(10,2) not null,
Id_Categoria int,
constraint fk_Opcao_Personalizacao foreign key (Id_Categoria) references tbCategoria(Id_Categoria)
);

create table tbItemPedido_Opcao(
Id_Opcao int,
Id_Item int,
Preco decimal(10,2),
primary key(Id_Opcao, Id_Item),
constraint FK_ItemPedido_Opcao_tbOpcao_Personalizacao foreign key (Id_Opcao) references tbOpcao_Personalizacao(Id_Opcao),
constraint FK_ItemPedido_Opcao_tbItem_Pedido foreign key (Id_Item) references tbItem_Pedido(Id_Item)
);

create table tbPagamento(
Id_Pagamento int primary key auto_increment,
ValorPago decimal(10,2) not null,
DataHora datetime default current_timestamp,
FormaPagamento enum('PIX','Cartao') not null,
Stats enum('Pago', 'Recusado') not null,
Id_Pedido int not null,
Id_Cupom int,
Id_Cartao int,
constraint fk_Pagamento_tbCartao foreign key (Id_Cartao) references tbCartao(Id_Cartao),
constraint fk_Pagamento_tbPedido foreign key (Id_Pedido) references tbPedido(Id_Pedido),
constraint fk_Pagamento_tbCupom foreign key (Id_Cupom) references tbCupom(Id_Cupom)
);

create table tbNF(
Id_NF int primary key auto_increment,
Numeracao int not null,	
ValorTotal decimal(10,2) not null,
DataEmissao datetime default current_timestamp,
CNPJ_Discoffe varchar(15) not null,
Id_Pagamento int,
constraint fk_NF_tbPagamento foreign key (Id_Pagamento) references tbPagamento(Id_Pagamento)
);

/*
--------------------------------------------------------------------------------------------
------------------------------ PROCEDURES DE INSERÇÃO --------------------------------------
--------------------------------------------------------------------------------------------
*/

-- CADASTRA O CEP DO USUÁRIO
delimiter $$
create procedure cadastrarCEP(
	vCEP varchar(15),
    out vId_CEP int -- VARIÁVEL PARA RETORNAR O ID DO CEP CADASTRADO OU ENCONTRADO APÓS O FIM DA PROCEDURE
)
begin
    
    -- CADASTRA CASO AINDA NÃO ESTEJA REGISTRADO
    if(not exists( select * from tbCEP where CEP = vCEP limit 1 ))
    then
		insert into tbCEP(CEP)
			values(vCEP);
    end if;
    
    -- BUSCA O ID DO CEP CADASTRADO OU JÁ EXISTENTE E O SALVA NA VARIAVEL CRIADA ACIMA
    select Id_CEP into vId_CEP from tbCEP where CEP = vCEP limit 1;
    
end;
$$

-- CADASTRA O USUÁRIO
delimiter $$ 
create procedure cadastrarUsuario(
	vNomeCompleto varchar(150),
	vEmail varchar(100),
	vSenha varchar(100),
	vCPF varchar(15),
	vTelefone varchar(15),
	vDataNasc date,
	vNumero_Casa varchar(10),
	vComplemento varchar(50),
    vCEP varchar(9),
	vPerfil int
)
begin
	declare vId_CEP int; -- VARIÁVEL PARA RECEBER O ID DO CEP
    declare vId_Perfil int; -- VARIÁVEL PARA RECEBER O ID DO PERFIL
    
    if(not exists(select * from tbUsuario where Email = vEmail limit 1))
    then
		call cadastrarCEP(vCEP, vId_CEP); -- CHAMA A PROCEDURE DE CADASTRO DE CEP E ATRIBUI O NOVO CEP À VARIÁVEL "vId_CEP"
        select Id_Perfil into vId_Perfil from tbPerfil where Tipo = vPerfil limit 1;
    
		insert into tbUsuario(NomeCompleto, Email, Senha, CPF, Telefone, DataNasc, Numero_Casa, Complemento, Id_Perfil, Id_CEP)
			values (vNomeCompleto, vEmail, vSenha, vCPF, vTelefone, vDataNasc, vNumero_Casa, vComplemento, vId_Perfil, vId_CEP);
    end if;

end;
$$

-- INSERE A CATEGORIA
delimiter $$
create procedure cadastrarCategoria(
    vCategoria varchar(50),
    out vId_Categoria int
)
begin
 
	-- CADASTRA CASO O ID AINDA NÃO EXISTA
	if(not exists(select * from tbCategoria where Categoria = vCategoria limit 1))
    then
		insert into tbCategoria(Categoria) values(vCategoria);
    end if;
    
    select Id_Categoria into vId_Categoria from tbCategoria where Categoria = vCategoria limit 1;
 
end $$
delimiter ;
 
-- INSERE O PRODUTO
delimiter $$
create procedure inserirProduto(
    vNome varchar(50),
    vPreco decimal(10,2),
    vDescricao varchar(200),
    vImagem varchar(100),
    vDisponibilidade boolean,
    vId_Categoria int,
    out vId_Produto int
)
begin
    set vId_Produto = 0;

    if not exists (select 1 from tbProduto where Nome = vNome) then
        insert into tbProduto(Nome, Preco, Descricao, Imagem, Disponibilidade, Id_Categoria)
        values (vNome, vPreco, vDescricao, vImagem, vDisponibilidade, vId_Categoria);

        set vId_Produto = last_insert_id();
    end if;
end $$
delimiter ;

-- CATEGORIAS
call cadastrarCategoria('Tradicional', @catTradicional);
call cadastrarCategoria('Drink', @catDrink);
call cadastrarCategoria('Doce', @catDoce);
call cadastrarCategoria('Salgado', @catSalgado);
call cadastrarCategoria('Refresco', @catRefresco);
call cadastrarCategoria('Shake', @catShake);

-- PRODUTOS
call inserirProduto('Macchiatto', 29.99, 'Café extremamente cremoso', 'Macchiatto.png', true, @catTradicional, @idProd);
call inserirProduto('Café Gelado', 18.50, 'Café gelado com caramelo', 'Café_Gelado.png', true, @catDrink, @idProd);
call inserirProduto('Brigadeiro', 8.50, 'Brigadeiro gourmet', 'Brigadeiro.png', true, @catDoce, @idProd);
call inserirProduto('Coxinha', 7.00, 'Coxinha de frango', 'Coxinha.png', true, @catSalgado, @idProd);
call inserirProduto('Limonada', 9.90, 'Limonada suíça', 'Limonada.png', true, @catRefresco, @idProd);
call inserirProduto('Shake de Chocolate', 16.90, 'Shake cremoso', 'Shake_Chocolate.png', true, @catShake, @idProd);

select * from tbCategoria;
select * from tbProduto;

-- ATIVA OU DESATIVA UM PRODUTO (ALTERA SÓ A DISPONIBILIDADE)
delimiter $$
create procedure alterarDisponibilidadeProduto(
    vId_Produto int,
    vDisponibilidade boolean
)
begin
    update tbProduto
    set Disponibilidade = vDisponibilidade
    where Id_Produto = vId_Produto;
end $$
delimiter ;
 
-- INSERE AS OPÇÕES DE PERSONALIZAÇÃO
delimiter $$
create procedure cadastrarOpcaoPersonalizacao(
	vTipo int,
    vOpcao varchar(50),
    vPreco_Extra decimal(10,2),
    vCategoria varchar(50)
)
begin
	declare vId_Categoria int;
 
	-- CADASTRA CASO A OPÇÃO AINDA NÃO EXISTA (Opcao É UNIQUE)
	if(not exists(select * from tbOpcao_Personalizacao where Opcao = vOpcao limit 1))
    then
    
		call cadastrarCategoria(vCategoria, vId_Categoria);
    
		insert into tbOpcao_Personalizacao(Tipo, Opcao, Preco_Extra, Id_Categoria)
			values(vTipo, vOpcao, vPreco_Extra, vId_Categoria);
    end if;
 
end $$
delimiter ;
 
call cadastrarOpcaoPersonalizacao(1, 'Integral', 0, 'Tradicional');
call cadastrarOpcaoPersonalizacao(1, 'Desnatado', 1.00, 'Tradicional');
call cadastrarOpcaoPersonalizacao(1, 'Vegetal', 5.00, 'Tradicional');
call cadastrarOpcaoPersonalizacao(2, 'Sem Açucar', 0, 'Tradicional');
call cadastrarOpcaoPersonalizacao(2, 'Com Sache', 1, 'Tradicional');
call cadastrarOpcaoPersonalizacao(2, 'Adoçante', 3, 'Tradicional');
call cadastrarOpcaoPersonalizacao(3, 'Canela', 2.50, 'Tradicional');
call cadastrarOpcaoPersonalizacao(3, 'Chantilly', 2, 'Tradicional');
call cadastrarOpcaoPersonalizacao(3, 'Achocolatado', 4.90, 'Tradicional');

select * from tbOpcao_Personalizacao;

-- PEDIDOS
delimiter $$
create procedure criarPedido(
    vTipo int,
    vStats int,
    vFrete decimal(10,2),
    vDesconto decimal(10, 2),
    vId_Usuario int
)
begin

    -- CRIAR O PEDIDO COM OS VALORES INICIAIS
    insert into tbPedido(Tipo, Stats, SubTotal, Total, Frete, Desconto, Id_Usuario)
		values(vTipo, vStats, 0, vFrete, vFrete, vDesconto, vId_Usuario);

    -- RETORNAR O ID DO PEDIDO CRIADO
    select last_insert_id() as Id_Pedido;
end $$
delimiter ;

delimiter $$
create procedure inserirItemPedido(
    vId_Pedido int,
    vId_Produto int,
    vQuantidade smallint
)
begin
    declare vPreco decimal(10,2);

    -- BUSCAR O PREÇO-BASE DO PRODUTO
    select Preco into vPreco from tbProduto where Id_Produto = vId_Produto;

    -- INSERIR O ITEM NO PEDIDO
    insert into tbItem_Pedido(Preco_Unitario, Quantidade, Id_Pedido, Id_Produto)
		values (vPreco, vQuantidade, vId_Pedido, vId_Produto);
        
	-- RETORNAR O ID DO ITEM CRIADO
    select last_insert_id() as Id_Item;
end $$
delimiter ;

delimiter $$
create procedure atualizarTotalPedido(
    vId_Pedido int
)
begin

    -- CALCULAR O SUBTOTAL DOS ITENS E PERSONALIZAÇÕES
    update tbPedido
    set SubTotal = (
        select coalesce(sum(
            (ip.Preco_Unitario + coalesce(op.Extras, 0))
            * ip.Quantidade
        ), 0)
        from tbItem_Pedido as ip
        left join (
            select
                Id_Item,
                sum(Preco) as Extras
            from tbItemPedido_Opcao
            group by Id_Item
        )
        as op
            on op.Id_Item = ip.Id_Item
        where ip.Id_Pedido = vId_Pedido
    )
    where Id_Pedido = vId_Pedido;

    -- CALCULAR O TOTAL COM O FRETE
    update tbPedido set Total = SubTotal + Frete - coalesce(Desconto, 0) where Id_Pedido = vId_Pedido;
end $$
delimiter ;

delimiter $$
create procedure inserirOpcaoItemPedido(
    vId_Item int,
    vId_Opcao int
)
begin
    declare vPreco decimal(10,2) default null;

    -- BUSCAR O PREÇO E VALIDAR A CATEGORIA
    select op.Preco_Extra into vPreco from tbOpcao_Personalizacao as op
    inner join tbItem_Pedido as ip
        on ip.Id_Item = vId_Item
    inner join tbProduto as p
        on p.Id_Produto = ip.Id_Produto
    where op.Id_Opcao = vId_Opcao
      and op.Id_Categoria = p.Id_Categoria;

    -- REGISTRAR A OPÇÃO ESCOLHIDA
    insert into tbItemPedido_Opcao(Id_Opcao, Id_Item, Preco)
		values(vId_Opcao, vId_Item, vPreco);

end $$
delimiter ;

select * from tbOpcao_Personalizacao;
select * from tbProduto;
select * from tbCategoria;
select * from tbPedido;
select * from tbItem_Pedido;

delimiter $$
create procedure registrarPagamento(
	vFormaPagamento int,
	vStats int,
	vId_Pedido int,
	vId_Cartao int,
	vId_Cupom int
)
begin
	declare vTotalPago decimal(10,2);
    
    if (exists(select * from tbPedido where Id_Pedido = vId_Pedido limit 1))
    then
		select Total into vTotalPago from tbPedido where Id_Pedido = vId_Pedido;
        
		if(not exists(select * from tbPagamento where Id_Pedido = vId_Pedido limit 1))
        then 
        
			insert into tbPagamento(ValorPago, FormaPagamento, Stats, Id_Pedido, Id_Cartao, Id_Cupom)
			values(vTotalPago, vFormaPagamento, vStats, vId_Pedido, vId_Cartao, vId_Cupom);
        end if;
    end if;

end $$
delimiter ;

select * from tbPedido;
select * from tbUsuario;
select * from tbPagamento;

-- drop procedure registrarPagamento;
call registrarPagamento(1, 1, 1, null, null);

delimiter $$
create procedure cadastrarCartao(
	vNome_Titular varchar(75),
    vApelido varchar(50),
    vNumero_Mascarado varchar(100),
    vDataVal date,
    vId_Usuario int
)
begin
	
    if(not exists(select * from tbCartao where Numero_Mascarado = vNumero_Mascarado limit 1))
	then
		insert into tbCartao(Nome_Titular, Apelido, Numero_Mascarado, DataVal, Id_Usuario)
			values(vNome_Titular, vApelido, vNumero_Mascarado, vDataVal, vId_Usuario);
    end if;
end;
$$
delimiter ;

select * from tbCartao;
 
delimiter $$
create procedure cadastrarCupom(
    vCodigo_Cupom varchar(10),
    vDesconto decimal(10,2),
    vDataValidade varchar(20)
)
begin
 
	-- CADASTRA CASO O CÓDIGO AINDA NÃO ESTEJA REGISTRADO
	if(not exists(select * from tbCupom where Codigo_Cupom = vCodigo_Cupom limit 1))
    then
		insert into tbCupom(Codigo_Cupom, Desconto, DataValidade)
			values(vCodigo_Cupom, vDesconto, str_to_date(vDataValidade, '%d/%m/%Y %H:%i:%s'));
    end if;
 
end;
$$
delimiter ;
 
call cadastrarCupom('DESC15', 15.00, '19/11/2026 23:59:59');
select * from tbCupom;

/*
--------------------------------------------------------------------------------------------
--------------------------------- PROCEDURES DE BUSCA --------------------------------------
--------------------------------------------------------------------------------------------
*/

-- BUSCA USUARIO
delimiter $$
create procedure buscarUsuarioPorId(
	vId_Usuario int
    -- vFiltro VARCHAR(150) -- FILTRO DE QUALQUER DADO DO USUARIO
)
begin

    select
        u.NomeCompleto,
        u.Email,
        u.Senha,
        u.CPF,
        u.Telefone,
        u.DataNasc,
        u.Stats,
        u.Numero_Casa,
        u.Complemento,
        c.CEP,
        p.Tipo as Perfil
    from tbUsuario u
    left join tbPerfil p ON p.Id_Perfil = u.Id_Perfil
    left join tbCEP c ON c.Id_CEP = u.Id_CEP
    where u.Id_Usuario = vId_Usuario;
end $$
delimiter ;

-- BUSCA TODOS OS USUÁRIOS
delimiter $$
create procedure buscarTodosUsuarios()
begin
    select
        u.Id_Usuario,
        u.NomeCompleto,
        u.Email,
        u.Senha,
        u.CPF,
        u.Telefone,
        u.DataNasc,
        u.Stats,
        u.Numero_Casa,
        u.Complemento,
        c.CEP,
        p.Tipo as Perfil
    from tbUsuario u
    left join tbPerfil p ON p.Id_Perfil = u.Id_Perfil
    left join tbCEP c ON c.Id_CEP = u.Id_CEP;
end $$
delimiter ;

-- BUSCA TODOS OS PRODUTOS (CARDAPIO)
delimiter $$
create procedure buscarTodosProdutos()
begin

    select
        p.Id_Produto,
        p.Nome,
        p.Preco,
        p.Descricao,
        p.Imagem,
        p.Disponibilidade,
        c.Id_Categoria,
        c.Categoria
    from tbProduto p
    inner join tbCategoria c
        ON p.Id_Categoria = c.Id_Categoria
    order by c.Categoria, p.Nome;

end $$
delimiter ;

call buscarTodosProdutos();

-- BUSCA PRODUTO POR ID
delimiter $$
create procedure buscarProdutoPorId(
    in vId_Produto int
)
begin

    select
        p.Id_Produto,
        p.Nome,
        p.Preco,
        p.Descricao,
        p.Imagem,
        p.Disponibilidade,
        c.Id_Categoria,
        c.Categoria,
        o.Id_Opcao,
        o.Tipo,
        o.Opcao,
        o.Preco_Extra
    from tbProduto p
    inner join tbCategoria c
        ON p.Id_Categoria = c.Id_Categoria
    left join tbOpcao_Personalizacao o
        ON c.Id_Categoria = o.Id_Categoria
    where p.Id_Produto = vId_Produto
    order by o.Tipo, o.Id_Opcao;

end $$
delimiter ;

call buscarProdutoPorId(1);

-- BUSCA OPÇÕES DE PERSONALIZAÇÃO DISPONIVEL POR CATEGORIA DE PRODUTO
delimiter $$
create procedure buscarOpcoesPersonalizacaoPorProduto(
    in vId_Categoria int
)
begin
    -- BUSCAR TODAS AS OPÇÕES DA CATEGORIA
    select
        Id_Opcao,
        Tipo,
        Opcao,
        Preco_Extra
    from tbOpcao_Personalizacao
    where Id_Categoria = vId_Categoria
    order by Id_Opcao;

end $$
delimiter ;

select * from tbOpcao_Personalizacao; 
call buscarOpcoesPersonalizacaoPorProduto('1');

delimiter $$
create procedure buscarTodosPedidos()
begin
    select
        -- USUARIO
        pe.Id_Usuario,
        us.NomeCompleto,
        us.CPF,

        -- PEDIDO
        pe.Id_Pedido,
        pe.DataHora,
        pe.Tipo as Tipo_Pedido,
        pe.Stats as Status_Pedido,
        pe.SubTotal,
        pe.Frete,
        pe.Desconto,
        pe.Total,

        -- ITEM
        ip.Id_Item,
        ip.Id_Produto,
        p.Nome as Nome_Produto,
        ip.Quantidade,
        ip.Preco_Unitario,

        -- PERSONALIZACAO
        ipo.Id_Opcao,
        op.Tipo as Tipo_Personalizacao,
        op.Opcao,
        ipo.Preco as Preco_Extra,
        
		-- PAGAMENTO
        pg.Id_Pagamento,
        pg.FormaPagamento,
        pg.DataHora as DataHora_Pagamento,
        pg.Stats as Status_Pagamento,
        pg.Id_Cupom,
        
        -- CUPOM
        cup.Codigo_Cupom as Codigo,
        cup.Desconto

    from tbPedido as pe

    inner join tbUsuario as us
        ON pe.Id_Usuario = us.Id_Usuario

    inner join tbItem_Pedido as ip
        ON pe.Id_Pedido = ip.Id_Pedido

    inner join tbProduto as p
        ON ip.Id_Produto = p.Id_Produto

    left join tbItemPedido_Opcao as ipo
        ON ip.Id_Item = ipo.Id_Item

    left join tbOpcao_Personalizacao as op
        ON ipo.Id_Opcao = op.Id_Opcao
        
	left join tbPagamento as pg
		ON pe.Id_Pedido = pg.Id_Pedido
        
	left join tbCupom as cup
		on pg.Id_Cupom = cup.Id_Cupom;
end $$
delimiter ;

call buscarTodosPedidos();

delimiter $$
create procedure buscarPedidosPorUsuario(vId_Usuario int)
begin
	select
        -- PEDIDO
        pe.Id_Pedido,
        pe.DataHora,
        pe.Tipo as Tipo_Pedido,
        pe.Stats as Status_Pedido,
        pe.SubTotal,
        pe.Frete,
        pe.Desconto,
        pe.Total,
        pe.Id_Usuario,

        -- ITEM
        ip.Id_Item,
        p.Nome as Nome_Produto,
        ip.Quantidade,
        ip.Preco_Unitario,

        -- PERSONALIZACAO
        ipo.Id_Opcao,
        op.Tipo as Tipo_Personalizacao,
        op.Opcao,
        ipo.Preco as Preco_Extra,
        
        -- PAGAMENTO
        pg.Id_Pagamento,
        pg.FormaPagamento,
        pg.DataHora as DataHora_Pagamento,
        pg.Stats as Status_Pagamento,
        pg.Id_Cupom,
        
        -- CUPOM
        cup.Codigo_Cupom as Codigo,
        cup.Desconto


	from tbPedido as pe
	inner join tbItem_Pedido as ip
		ON pe.Id_Pedido = ip.Id_Pedido
        
	inner join tbProduto as p
		ON ip.Id_Produto = p.Id_Produto
        
	left join tbItemPedido_Opcao as ipo
		ON ip.Id_Item = ipo.Id_Item
        
	left join tbOpcao_Personalizacao as op
		ON ipo.Id_Opcao = op.Id_Opcao
        
	left join tbPagamento as pg
		ON pe.Id_Pedido = pg.Id_Pedido
        
	left join tbCupom as cup
		on pg.Id_Cupom = cup.Id_Cupom
        
	where pe.Id_Usuario = vId_Usuario
	order by
		pe.Id_Pedido desc,
		pe.DataHora;
end $$
delimiter ;

-- drop procedure BuscarPedidosPorUsuario;
call buscarPedidosPorUsuario(1);

select * from tbPedido;

/*
--------------------------------------------------------------------------------------------
--------------------------------- PROCEDURES DE EDIÇÃO --------------------------------------
--------------------------------------------------------------------------------------------
*/

-- ATUALIZA DADOS DO USUARIO
delimiter $$
create procedure atualizarUsuario(
	vId_Usuario int,
	vNomeCompleto varchar(150),
	vEmail varchar(100),
	vSenha varchar(100),
	vCPF varchar(15),
	vTelefone varchar(15),
	vDataNasc date,
	vNumero_Casa varchar(10),
	vComplemento varchar(50),
	vCEP varchar(9)
)
begin
    declare vId_CEP int;

    -- VERIFICA SE JA EXISTE UM USUARIO
    if (exists (select 1 from tbUsuario where Id_Usuario = vId_Usuario))
    then

        -- PROCURA OU CADASTRA O CEP CASO UM TENHA SIDO ENVIADO
        if vCEP is not null and vCEP <> '' then

            select Id_CEP into vId_CEP from tbCEP where CEP = vCEP limit 1;

            -- CADASTRA CASO  O CEP AINDA NÃO EXISTA
            if vId_CEP is null then
                call insertTbCEP(vCEP, vId_CEP);
            end if;
        end if;

        update tbUsuario
        set
            NomeCompleto = coalesce(vNomeCompleto, NomeCompleto),
            Email = coalesce(vEmail, Email),
            Senha = coalesce(vSenha, Senha),
            CPF = coalesce(vCPF, CPF),
            Telefone = coalesce(vTelefone, Telefone),
            DataNasc = coalesce(vDataNasc, DataNasc),
            Numero_Casa = coalesce(vNumero_Casa, Numero_Casa),
            Complemento = coalesce(vComplemento, Complemento),
            Id_CEP = coalesce(vId_CEP, Id_CEP)
        where Id_Usuario = vId_Usuario;

    end if;
end $$
delimiter 

-- ATUALIZA USUARIO PELA GERENCIA (TAMBEM ALTERA STATUS E PERFIL)
delimiter $$
create procedure atualizarUsuarioGerencia(
	vId_Usuario int,
	vNomeCompleto varchar(150),
	vEmail varchar(100),
	vSenha varchar(100),
	vCPF varchar(15),
	vTelefone varchar(15),
	vDataNasc date,
	vNumero_Casa varchar(10),
	vComplemento varchar(50),
	vCEP varchar(9),
	vPerfil int,
	vStatus varchar(10)
)
begin
    declare vId_CEP int;
    declare vId_Perfil int;

    if (exists (select 1 from tbUsuario where Id_Usuario = vId_Usuario))
    then

        -- PROCURA OU CADASTRA O CEP CASO UM TENHA SIDO ENVIADO
        if vCEP is not null and vCEP <> '' then
            call cadastrarCEP(vCEP, vId_CEP);
        end if;

        -- BUSCA O ID DO PERFIL CASO UM TENHA SIDO ENVIADO
        if vPerfil is not null then
            select Id_Perfil into vId_Perfil from tbPerfil where Tipo = vPerfil limit 1;
        end if;

        update tbUsuario
        set
            NomeCompleto = coalesce(vNomeCompleto, NomeCompleto),
            Email = coalesce(vEmail, Email),
            Senha = coalesce(vSenha, Senha),
            CPF = coalesce(vCPF, CPF),
            Telefone = coalesce(vTelefone, Telefone),
            DataNasc = coalesce(vDataNasc, DataNasc),
            Numero_Casa = coalesce(vNumero_Casa, Numero_Casa),
            Complemento = coalesce(vComplemento, Complemento),
            Id_CEP = coalesce(vId_CEP, Id_CEP),
            Id_Perfil = coalesce(vId_Perfil, Id_Perfil),
            Stats = coalesce(vStatus, Stats)
        where Id_Usuario = vId_Usuario;

    end if;
end $$
delimiter ;

delimiter $$
create procedure atualizarProduto(
    vId_Produto int,
    vNome varchar(50),
    vPreco decimal(10,2),
    vDescricao varchar(200),
    vImagem varchar(100),
    vDisponibilidade boolean,
    vId_Categoria int,
    out vAtualizado int
)
begin
    if exists (select 1 from tbProduto
               where Nome = vNome and Id_Produto <> vId_Produto) then
        set vAtualizado = 0;
    else
        update tbProduto
        set Nome = vNome,
            Preco = vPreco,
            Descricao = vDescricao,
            Imagem = vImagem,
            Disponibilidade = vDisponibilidade,
            Id_Categoria = vId_Categoria
        where Id_Produto = vId_Produto;

        set vAtualizado = 1;
    end if;
end $$
delimiter ;