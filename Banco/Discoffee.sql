DROP DATABASE IF EXISTS dbDiscoffee;
CREATE DATABASE dbDiscoffee;
USE dbDiscoffee;

-- TABELAS
CREATE TABLE tbPerfil(
Id_Perfil INT PRIMARY KEY auto_increment,
Tipo ENUM('Cliente', 'Funcionario', 'Administrador') NOT NULL UNIQUE
);

/* CRIA OS 3 TIPOS DE PERFIS NA TABELA PERFIL */
insert into tbPerfil(Tipo) values(1);
insert into tbPerfil(Tipo) values(2);
insert into tbPerfil(Tipo) values(3);

select * from tbPerfil;

CREATE TABLE tbCEP(
Id_CEP INT PRIMARY KEY auto_increment,
CEP VARCHAR(9) UNIQUE
);

CREATE TABLE tbCupom(
Id_Cupom INT PRIMARY KEY auto_increment,
Nome VARCHAR(50),
Codigo_Cupom VARCHAR(10) NOT NULL UNIQUE,
Desconto DECIMAL(10,2) NOT NULL,
DataValidade DATETIME
);

CREATE TABLE tbCategoria(
Id_Categoria INT PRIMARY KEY auto_increment,
Categoria VARCHAR(50)
);

CREATE TABLE tbUsuario(
Id_Usuario INT PRIMARY KEY auto_increment,
NomeCompleto VARCHAR(150) NOT NULL,
Email VARCHAR(100) NOT NULL UNIQUE,
Senha VARCHAR(100) NOT NULL,
CPF VARCHAR(15) NOT NULL UNIQUE,
Stats ENUM('Ativo','Inativo') default 'Ativo',
Telefone VARCHAR(15),
DataNasc DATE,
Numero_Casa VARCHAR(10) NOT NULL,
Complemento VARCHAR(50) default 'Sem Complemento',
Id_Perfil INT,
Id_CEP INT,
CONSTRAINT FK_Id_Perfil FOREIGN KEY(Id_Perfil) REFERENCES tbPerfil(Id_Perfil),
CONSTRAINT FK_Id_CEP FOREIGN KEY(Id_CEP) REFERENCES tbCEP(Id_CEP) 
);


CREATE TABLE tbPedido(
Id_Pedido INT PRIMARY KEY auto_increment,
Tipo ENUM('Retirada','Entrega') NOT NULL,
Stats ENUM('Pendente', 'Preparando', 'Finalizado', 'Em Entrega', 'Entregue', 'Cancelado') NOT NULL,
SubTotal DECIMAL(10,2),
Total DECIMAL(10,2),
DataHora DATETIME DEFAULT CURRENT_TIMESTAMP,
Frete DECIMAL(10,2),
Id_Usuario INT,
CONSTRAINT FK_Id_USusario_Pedido FOREIGN KEY(Id_Usuario) REFERENCES tbUsuario(Id_Usuario)
);

CREATE TABLE tbProduto(
Id_Produto INT PRIMARY KEY auto_increment,
Nome VARCHAR(50) NOT NULL,	
Preco DECIMAL(10,2) NOT NULL,
Descricao VARCHAR(200) NOT NULL,
Tamanho VARCHAR(5) NOT NULL,
Imagem VARCHAR(100) NOT NULL,
Disponibilidade BOOLEAN,
Id_Categoria INT,
CONSTRAINT FK_Id_Categoria_Produto FOREIGN KEY(Id_Categoria) REFERENCES tbCategoria(Id_Categoria)
);

CREATE TABLE tbAvaliacao(
Id_Avaliacao INT PRIMARY KEY auto_increment,
Comentario VARCHAR(100) NOT NULL,
Nota INT CHECK(Nota BETWEEN 1 AND 5) NOT NULL,
DataAval DATETIME DEFAULT CURRENT_TIMESTAMP,
Id_Produto INT,
Id_Usuario INT,
CONSTRAINT fk_Avaliacao_tbUsuario FOREIGN KEY (Id_Usuario) REFERENCES tbUsuario(Id_Usuario),
CONSTRAINT fk_Avaliacao_tbProduto FOREIGN KEY (Id_Produto) REFERENCES tbProduto(Id_Produto)
);

CREATE TABLE tbCartao(
Id_Cartao INT PRIMARY KEY auto_increment,
Nome_Titular VARCHAR(75) NOT NULL,
Apelido VARCHAR(50),
Numero_Mascarado VARCHAR(100) NOT NULL,
DataVal DATE NOT NULL,
Id_Usuario INT,
CONSTRAINT FK_Cartao_tbUsuario FOREIGN KEY (Id_Usuario) REFERENCES tbUsuario(Id_Usuario)
);

CREATE TABLE tbItem_Pedido(
Id_Item INT PRIMARY KEY auto_increment,
Preco_Unitario DECIMAL(10,2),
Quantidade SMALLINT,
Id_Pedido INT,
Id_Produto INT,
CONSTRAINT FK_Item_Pedido_tbPedido FOREIGN KEY (Id_Pedido) REFERENCES tbPedido(Id_Pedido),
CONSTRAINT FK_Item_Pedido_tbProduto FOREIGN KEY (Id_Produto) REFERENCES tbProduto(Id_Produto)
);

CREATE TABLE tbOpcao_Personalizacao(
Id_Opcao INT PRIMARY KEY auto_increment,
Tipo ENUM('Leite', 'Acucar', 'Adicional') NOT NULL,
Opcao VARCHAR(50) NOT NULL UNIQUE,
Preco_Extra DECIMAL(10,2) NOT NULL,
Id_Categoria INT,
CONSTRAINT fk_Opcao_Personalizacao FOREIGN KEY (Id_Categoria) REFERENCES tbCategoria(Id_Categoria)
);

CREATE TABLE tbItemPedido_Opcao(
Id_Opcao INT,
Id_Item INT,
Preco DECIMAL(10,2),
PRIMARY KEY(Id_Opcao, Id_Item),
CONSTRAINT FK_ItemPedido_Opcao_tbOpcao_Personalizacao FOREIGN KEY (Id_Opcao) REFERENCES tbOpcao_Personalizacao(Id_Opcao),
CONSTRAINT FK_ItemPedido_Opcao_tbItem_Pedido FOREIGN KEY (Id_Item) REFERENCES tbItem_Pedido(Id_Item)
);

CREATE TABLE tbPagamento(
Id_Pagamento INT PRIMARY KEY auto_increment,
ValorPago DECIMAL(10,2) NOT NULL,
DataHora DATETIME DEFAULT CURRENT_TIMESTAMP,
FormaPagamento ENUM('PIX','Débito','Crédito') NOT NULL,
Status ENUM('Fnaliazado', 'Em andamento', 'Á Pagar') NOT NULL,
Id_Pedido INT,
Id_Cupom INT,
Id_Cartao INT,
CONSTRAINT fk_Pagamento_tbCartao FOREIGN KEY (Id_Cartao) REFERENCES tbCartao(Id_Cartao),
CONSTRAINT fk_Pagamento_tbPedido FOREIGN KEY (Id_Pedido) REFERENCES tbPedido(Id_Pedido),
CONSTRAINT fk_Pagamento_tbCupom FOREIGN KEY (Id_Cupom) REFERENCES tbCupom(Id_Cupom)
);

CREATE TABLE tbNF(
Id_NF INT PRIMARY KEY auto_increment,
Numeracao INT NOT NULL,	
ValorTotal DECIMAL(10,2) NOT NULL,
DataEmissao DATETIME DEFAULT CURRENT_TIMESTAMP,
CNPJ_Discoffe VARCHAR(15) NOT NULL,
Id_Pagamento INT,
CONSTRAINT fk_NF_tbPagamento FOREIGN KEY (Id_Pagamento) REFERENCES tbPagamento(Id_Pagamento)
);

/*
--------------------------------------------------------------------------------------------
------------------------------ PROCEDURES DE INSERÇÃO --------------------------------------
--------------------------------------------------------------------------------------------
*/

-- CADASTRA O CEP DO USUÁRIO
delimiter $$
create procedure insertTbCEP(
	vCEP VARCHAR(15),
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
create procedure insertTbUsuario(
	vNomeCompleto VARCHAR(150),
	vEmail VARCHAR(100),
	vSenha VARCHAR(100),
	vCPF VARCHAR(15),
	vTelefone VARCHAR(15),
	vDataNasc DATE,
	vNumero_Casa VARCHAR(10),
	vComplemento VARCHAR(50),
    vCEP VARCHAR(9),
	vPerfil INT
)
begin
	declare vId_CEP int; -- VARIÁVEL PARA RECEBER O ID DO CEP
    declare vId_Perfil int; -- VARIÁVEL PARA RECEBER O ID DO PERFIL
    
    if(not exists(select * from tbUsuario where Email = vEmail limit 1))
    then
		call insertTbCEP(vCEP, vId_CEP); -- CHAMA A PROCEDURE DE CADASTRO DE CEP E ATRIBUI O NOVO CEP À VARIÁVEL "vId_CEP"
        select Id_Perfil into vId_Perfil from tbPerfil where Tipo = vPerfil limit 1;
    
		insert into tbUsuario(NomeCompleto, Email, Senha, CPF, Telefone, DataNasc, Numero_Casa, Complemento, Id_Perfil, Id_CEP)
			values (vNomeCompleto, vEmail, vSenha, vCPF, vTelefone, vDataNasc, vNumero_Casa, vComplemento, vId_Perfil, vId_CEP);
    end if;

end;
$$

select * from tbCEP;
select * from tbPerfil;
select * from tbUsuario;

-- INSERE A CATEGORIA
delimiter $$
create procedure insertTbCategoria(
    vCategoria VARCHAR(50),
    out vId_Categoria INT
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
select * from tbCategoria;
 
-- INSERE O PRODUTO
DELIMITER $$
CREATE PROCEDURE insertTbProduto(
    IN  vNome            VARCHAR(50),
    IN  vPreco           DECIMAL(10,2),
    IN  vDescricao       VARCHAR(200),
    IN  vTamanho         VARCHAR(5),
    IN  vImagem          VARCHAR(100),
    IN  vDisponibilidade BOOLEAN,
    IN  vId_Categoria    INT,
    OUT vId_Produto      INT
)
BEGIN
    SET vId_Produto = 0;

    IF NOT EXISTS (SELECT 1 FROM tbProduto WHERE Nome = vNome AND Tamanho = vTamanho) THEN
        INSERT INTO tbProduto(Nome, Preco, Descricao, Tamanho, Imagem, Disponibilidade, Id_Categoria)
        VALUES (vNome, vPreco, vDescricao, vTamanho, vImagem, vDisponibilidade, vId_Categoria);

        SET vId_Produto = LAST_INSERT_ID();
    END IF;
END $$
DELIMITER ;

-- EDIÇÃO: recebe o ID da categoria.
DELIMITER $$
CREATE PROCEDURE updateTbProduto(
    IN  vId_Produto      INT,
    IN  vNome            VARCHAR(50),
    IN  vPreco           DECIMAL(10,2),
    IN  vDescricao       VARCHAR(200),
    IN  vTamanho         VARCHAR(5),
    IN  vImagem          VARCHAR(100),
    IN  vDisponibilidade BOOLEAN,
    IN  vId_Categoria    INT,
    OUT vAtualizado      INT
)
BEGIN
    IF EXISTS (SELECT 1 FROM tbProduto
               WHERE Nome = vNome AND Tamanho = vTamanho AND Id_Produto <> vId_Produto) THEN
        SET vAtualizado = 0;
    ELSE
        UPDATE tbProduto
        SET Nome = vNome,
            Preco = vPreco,
            Descricao = vDescricao,
            Tamanho = vTamanho,
            Imagem = vImagem,
            Disponibilidade = vDisponibilidade,
            Id_Categoria = vId_Categoria
        WHERE Id_Produto = vId_Produto;

        SET vAtualizado = 1;
    END IF;
END $$
DELIMITER ;
 
-- INSERE AS OPÇÕES DE PERSONALIZAÇÃO
delimiter $$
create procedure insertTbOpcao_Personalizacao(
	vTipo INT,
    vOpcao VARCHAR(50),
    vPreco_Extra DECIMAL(10,2),
    vCategoria Varchar(50)
)
begin
	declare vId_Categoria INT;
 
	-- CADASTRA CASO A OPÇÃO AINDA NÃO EXISTA (Opcao É UNIQUE)
	if(not exists(select * from tbOpcao_Personalizacao where Opcao = vOpcao limit 1))
    then
    
		call insertTbCategoria(vCategoria, vId_Categoria);
    
		insert into tbOpcao_Personalizacao(Tipo, Opcao, Preco_Extra, Id_Categoria)
			values(vTipo, vOpcao, vPreco_Extra, vId_Categoria);
    end if;
 
end $$
delimiter ;
 
CALL insertTbOpcao_Personalizacao(1, 'Integral', 0, 'Tradicional');
CALL insertTbOpcao_Personalizacao(1, 'Desnatado', 1.00, 'Tradicional');
CALL insertTbOpcao_Personalizacao(1, 'Vegetal', 5.00, 'Tradicional');
CALL insertTbOpcao_Personalizacao(2, 'Sem Açucar', 0, 'Tradicional');
CALL insertTbOpcao_Personalizacao(2, 'Com Sache', 1, 'Tradicional');
CALL insertTbOpcao_Personalizacao(2, 'Adoçante', 3, 'Tradicional');
CALL insertTbOpcao_Personalizacao(3, 'Canela', 2.50, 'Tradicional');
CALL insertTbOpcao_Personalizacao(3, 'Chantilly', 2, 'Tradicional');
CALL insertTbOpcao_Personalizacao(3, 'Achocolatado', 4.90, 'Tradicional');

select * from tbOpcao_Personalizacao;

-- PEDIDOS
delimiter $$
create procedure criarPedido(
    vTipo enum('Retirada', 'Entrega'),
    vStats varchar(75),
    vFrete decimal(10,2),
    vId_Usuario int
)
begin

    -- CRIAR O PEDIDO COM OS VALORES INICIAIS
    insert into tbPedido(Tipo, Stats, SubTotal, Total, Frete, Id_Usuario)
		values(vTipo, vStats, 0, vFrete, vFrete, vId_Usuario);

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
    update tbPedido set Total = SubTotal + Frete where Id_Pedido = vId_Pedido;
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

    -- IMPEDIR OPÇÕES INEXISTENTES OU DE OUTRA CATEGORIA
    if vPreco is null then
        signal sqlstate '45000'
            set message_text = 'Opcao invalida para a categoria do produto.';
    end if;

    -- REGISTRAR A OPÇÃO ESCOLHIDA
    insert into tbItemPedido_Opcao(Id_Opcao, Id_Item, Preco)
		values(vId_Opcao, vId_Item, vPreco);
        

end $$
delimiter ;


-- ==========================================
-- 1. CONSULTAR OS PRODUTOS E SUAS CATEGORIAS
-- ==========================================
select
    Id_Produto,
    Nome,
    Preco,
    Id_Categoria
from tbProduto;


-- ==========================================
-- 2. CONSULTAR AS OPÇÕES E SUAS CATEGORIAS
-- ==========================================
select
    Id_Opcao,
    Tipo,
    Opcao,
    Preco_Extra,
    Id_Categoria
from tbOpcao_Personalizacao;


-- ==========================================
-- 3. CRIAR UM PEDIDO DE TESTE
-- ==========================================
call criarPedido('Retirada', 'Pendente', 0.00, 1);


-- ==========================================
-- 4. CONSULTAR O ID DO PEDIDO CRIADO
-- ==========================================
select *
from tbPedido;
order by Id_Pedido desc
limit 1;


-- ==========================================
-- 5. INSERIR UM ITEM NO PEDIDO
-- SUBSTITUA OS IDS PELOS VALORES CORRETOS
-- ==========================================
call inserirItemPedido(2, 1, 1);
call inserirItemPedido(2, 2, 1);

-- ==========================================
-- 6. CONSULTAR O ITEM INSERIDO
-- ==========================================
select *
from tbItem_Pedido
where Id_Pedido = 2
order by Id_Item desc;


-- ==========================================
-- 7. INSERIR UMA OPÇÃO DE PERSONALIZAÇÃO
-- USE UMA OPÇÃO DA MESMA CATEGORIA DO PRODUTO
-- ==========================================
call inserirOpcaoItemPedido(1, 3);
call inserirOpcaoItemPedido(2, 6);
call inserirOpcaoItemPedido(3, 6);

-- ==========================================
-- 8. CONFERIR A OPÇÃO VINCULADA AO ITEM
-- ==========================================
select
    ip.Id_Item,
    p.Nome as Produto,
    op.Tipo,
    op.Opcao as Personalizacao,
    ipo.Preco as Preco_Extra
from tbItemPedido_Opcao as ipo
inner join tbItem_Pedido as ip
    on ip.Id_Item = ipo.Id_Item
inner join tbProduto as p
    on p.Id_Produto = ip.Id_Produto
inner join tbOpcao_Personalizacao as op
    on op.Id_Opcao = ipo.Id_Opcao
where ip.Id_Pedido = 2;

-- ==========================================
-- 9. TESTAR UMA OPÇÃO DE OUTRA CATEGORIA
-- ESTA CHAMADA DEVE RETORNAR UM ERRO
-- ==========================================


-- ==========================================
-- 10. ATUALIZAR O SUBTOTAL E O TOTAL DO PEDIDO
-- ==========================================

call atualizarTotalPedido(2);

-- ==========================================
-- 11. CONFERIR OS VALORES FINAIS
-- ==========================================

select
    Id_Pedido,
    SubTotal,
    Frete,
    Total
from tbPedido
where Id_Pedido = 2;


-- ==========================================
-- 12. CONFERIR TODOS OS ITENS DO PEDIDO
-- ==========================================

select
    ip.Id_Item,
    p.Nome as Produto,
    ip.Preco_Unitario,
    ip.Quantidade,
    coalesce(sum(ipo.Preco), 0) as Total_Personalizacoes,
    (ip.Preco_Unitario + coalesce(sum(ipo.Preco), 0))
        * ip.Quantidade as Total_Item
from tbItem_Pedido as ip
inner join tbProduto as p
    on p.Id_Produto = ip.Id_Produto
left join tbItemPedido_Opcao as ipo
    on ipo.Id_Item = ip.Id_Item
where ip.Id_Pedido = 2
group by
    ip.Id_Item,
    p.Nome,
    ip.Preco_Unitario,
    ip.Quantidade;

 
select * from tbOpcao_Personalizacao;
select * from tbProduto;
select * from tbCategoria;
select * from tbPedido;
select * from tbItem_Pedido;



delimiter $$
create procedure insertTbCartao(
	vNome_Titular VARCHAR(75),
    vApelido VARCHAR(50),
    vNumero_Mascarado VARCHAR(100),
    vDataVal DATE,
    vId_Usuario INT
    -- out vId_Cartao int -- VARIÁVEL PARA RETORNAR O ID DO CARTÃO CADASTRADO
)
begin
	
    if(not exists(select * from tbCartao where Numero_Mascarado = vNumero_Mascarado limit 1))
	then
		insert into tbCartao(Nome_Titular, Apelido, Numero_Mascarado, DataVal, Id_Usuario)
			values(vNome_Titular, vApelido, vNumero_Mascarado, vDataVal, vId_Usuario);
    end if;
 
	-- set vId_Cartao = last_insert_id();
 
end;
$$
delimiter ;

CALL insertTbCartao('Guilherme', 'Sarue', '9090 8908 0900', '2027-09-29', 1);
SELECT @idCartao;

delimiter $$
create procedure insertTbAvaliacao(
	vComentario VARCHAR(100),
    vNota INT,
    vId_Produto INT,
    vId_Usuario INT
)
begin
	
	insert into tbAvaliacao(Comentario, Nota, Id_Produto, Id_Usuario)
		values(vComentario, vNota, vId_Produto, vId_Usuario);
 
end;
$$
delimiter ;
 
CALL insertTbAvaliacao('Café muito bom, cremoso e encorpado.', 5, 1, 1);
select * from tbAvaliacao;
 
 delimiter $$
create procedure insertTbCupom(
	vNome VARCHAR(50),
    vCodigo_Cupom VARCHAR(10),
    vDesconto DECIMAL(10,2),
    vDataValidade DATETIME
)
begin
 
	-- CADASTRA CASO O CÓDIGO AINDA NÃO ESTEJA REGISTRADO
	if(not exists(select * from tbCupom where Codigo_Cupom = vCodigo_Cupom limit 1))
    then
		insert into tbCupom(Nome, Codigo_Cupom, Desconto, DataValidade)
			values(vNome, vCodigo_Cupom, vDesconto, STR_TO_DATE(vDataValidade, '%d/%m/%Y %H:%i:%s'));
    end if;
 
end;
$$
delimiter ;
 
CALL insertTbCupom('Descontaço', 'DESC15', 15.00, '2026-11-09 23:59:59');
select * from tbCupom;

/*
--------------------------------------------------------------------------------------------
--------------------------------- PROCEDURES DE BUSCA --------------------------------------
--------------------------------------------------------------------------------------------
*/

-- BUSCA USUARIO
DELIMITER $$
CREATE PROCEDURE selectUsuario(
	vId_Usuario INT
    -- IN vFiltro VARCHAR(150) -- FILTRO DE QUALQUER DADO DO USUARIO
)
BEGIN

    SELECT
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
        p.Tipo AS Perfil
    FROM tbUsuario u
    LEFT JOIN tbPerfil p ON p.Id_Perfil = u.Id_Perfil
    LEFT JOIN tbCEP c ON c.Id_CEP = u.Id_CEP
    WHERE u.Id_Usuario = vId_Usuario;
END $$
DELIMITER ;

-- BUSCA TODOS OS PRODUTOS (CARDAPIO)
DELIMITER $$
CREATE PROCEDURE selectTbProdutos()
BEGIN

    SELECT
        p.Id_Produto,
        p.Nome,
        p.Preco,
        p.Descricao,
        p.Tamanho,
        p.Imagem,
        p.Disponibilidade,
        c.Id_Categoria,
        c.Categoria
    FROM tbProduto p
    INNER JOIN tbCategoria c
        ON p.Id_Categoria = c.Id_Categoria
    ORDER BY c.Categoria, p.Nome;

END $$
DELIMITER ;

CALL selectTbProdutos();

-- BUSCA PRODUTO POR ID
DELIMITER $$
CREATE PROCEDURE selectTbProdutoPorId(
    IN vId_Produto INT
)
BEGIN

    SELECT
        p.Id_Produto,
        p.Nome,
        p.Preco,
        p.Descricao,
        p.Tamanho,
        p.Imagem,
        p.Disponibilidade,
        c.Id_Categoria,
        c.Categoria,
        o.Id_Opcao,
        o.Tipo,
        o.Opcao,
        o.Preco_Extra
    FROM tbProduto p
    INNER JOIN tbCategoria c
        ON p.Id_Categoria = c.Id_Categoria
    LEFT JOIN tbOpcao_Personalizacao o
        ON c.Id_Categoria = o.Id_Categoria
    WHERE p.Id_Produto = vId_Produto
    ORDER BY o.Tipo, o.Id_Opcao;

END $$
DELIMITER ;
CALL selectTbProdutoPorId(1);

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
    order by Tipo, Opcao;

end $$
delimiter ;

select * from tbOpcao_Personalizacao; 
call buscarOpcoesPersonalizacaoPorProduto('1');


DELIMITER $$
CREATE PROCEDURE selectTbPedidos()
BEGIN
    SELECT
        -- PEDIDO
        pe.Id_Pe
        dido,
        pe.DataHora,
        pe.Tipo,
        pe.Stats,
        pe.SubTotal,
        pe.Frete,
        pe.Total,
        pe.Id_Usuario,

        -- ITEM
        ip.Id_Item,
        ip.Id_Produto,
        ip.Quantidade,
        ip.Preco_Unitario,

        -- PRODUTO
        p.Nome AS Nome_Produto,
        p.Tamanho,

        -- PERSONALIZAÇÃO
        op.Id_Opcao,
        op.Opcao AS Nome_Opcao,
        op.Tipo AS Tipo_Personalizacao,
        ipo.Preco AS Preco_Personalizacao

    FROM tbPedido AS pe

    INNER JOIN tbItem_Pedido AS ip
        ON pe.Id_Pedido = ip.Id_Pedido

    INNER JOIN tbProduto AS p
        ON ip.Id_Produto = p.Id_Produto

    LEFT JOIN tbItemPedido_Opcao AS ipo
        ON ip.Id_Item = ipo.Id_Item

    LEFT JOIN tbOpcao_Personalizacao AS op
        ON ipo.Id_Opcao = op.Id_Opcao

    ORDER BY
        pe.Id_Pedido DESC,
        ip.Id_Item,
        op.Id_Opcao;
END $$
DELIMITER ;

call selectTbPedidos();

/*
--------------------------------------------------------------------------------------------
--------------------------------- PROCEDURES DE EDIÇÃO --------------------------------------
--------------------------------------------------------------------------------------------
*/

-- ATUALIZA DADOS DO USUARIO
DELIMITER $$
CREATE PROCEDURE updateTbUsuario(
	vId_Usuario INT,
	vNomeCompleto VARCHAR(150),
	vEmail VARCHAR(100),
	vSenha VARCHAR(100),
	vCPF VARCHAR(15),
	vTelefone VARCHAR(15),
	vDataNasc DATE,
	vNumero_Casa VARCHAR(10),
	vComplemento VARCHAR(50),
	vCEP VARCHAR(9)
)
BEGIN
    DECLARE vId_CEP INT;

    -- VERIFICA SE JA EXISTE UM USUARIO
    IF (EXISTS (SELECT 1 FROM tbUsuario WHERE Id_Usuario = vId_Usuario))
    THEN

        -- PROCURA OU CADASTRA O CEP CASO UM TENHA SIDO ENVIADO
        IF vCEP IS NOT NULL AND vCEP <> '' THEN

            SELECT Id_CEP INTO vId_CEP FROM tbCEP WHERE CEP = vCEP LIMIT 1;

            -- CADASTRA CASO  O CEP AINDA NÃO EXISTA
            IF vId_CEP IS NULL THEN
                CALL insertTbCEP(vCEP, vId_CEP);
            END IF;
        END IF;

        UPDATE tbUsuario
        SET
            NomeCompleto = COALESCE(vNomeCompleto, NomeCompleto),
            Email = COALESCE(vEmail, Email),
            Senha = COALESCE(vSenha, Senha),
            CPF = COALESCE(vCPF, CPF),
            Telefone = COALESCE(vTelefone, Telefone),
            DataNasc = COALESCE(vDataNasc, DataNasc),
            Numero_Casa = COALESCE(vNumero_Casa, Numero_Casa),
            Complemento = COALESCE(vComplemento, Complemento),
            Id_CEP = COALESCE(vId_CEP, Id_CEP)
        WHERE Id_Usuario = vId_Usuario;

    END IF;
END $$
DELIMITER 