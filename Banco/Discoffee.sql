DROP DATABASE IF EXISTS dbDiscoffee;
CREATE DATABASE dbDiscoffee;
USE dbDiscoffee;

CREATE TABLE tbPerfil(
Id_Perfil INT PRIMARY KEY,
Tipo ENUM('CLiente', 'Funcionario', 'Administrador')
);

CREATE TABLE CEP(
Id_CEP INT PRIMARY KEY,
CEP VARCHAR(15)
);

CREATE TABLE tbCupom(
Id_Cupom INT PRIMARY KEY auto_increment,
Nome VARCHAR(50),
Codigo_Cupom VARCHAR(10) NOT NULL UNIQUE,
Desconto DECIMAL(10,2) NOT NULL,
DataValidade DATETIME
);

CREATE TABLE tbCategoria(
Id_Categoria INT PRIMARY KEY,
Categoria VARCHAR(50)
);

CREATE TABLE tbUsuario(
Id_Usuario INT PRIMARY KEY auto_increment,
NomeComple VARCHAR(30),
Email VARCHAR(100) UNIQUE,
Senha VARCHAR(100) NOT NULL,
CPF VARCHAR(15) NOT NULL UNIQUE,
Stats ENUM('Ativo','Inativo'),
Telefone VARCHAR(15),
DataNasc DATE,
Numero_Casa VARCHAR(10),
Complemento VARCHAR(50),
Id_Perfil INT,
CONSTRAINT FK_Id_Perfil FOREIGN KEY(Id_Perfil) REFERENCES tbPerfil(Id_Perfil) 
);

CREATE TABLE tbPedido(
Id_Pedido INT PRIMARY KEY auto_increment,
Tipo ENUM('Retirada','Entrega') NOT NULL,
Stats ENUM('Finalizado','Preparando','Cancelado') NOT NULL,
SubTotal DECIMAL(10,2),
Total DECIMAL(10,2),
DataHora DATETIME,
Frete DECIMAL(10,2),
Id_Usuario INT,
CONSTRAINT FK_Id_USusario_Pedido FOREIGN KEY(Id_Usuario) REFERENCES tbUsuario(Id_Usuario)
);

CREATE TABLE tbProduto(
CodProduto INT PRIMARY KEY,-- Tem que confirmar esse CodProduto, pois se for o codigo de barra do produto tudo bem o codigo que esta, agora se for o id do produto, faz mais sentido tem um auto_increment
Nome VARCHAR(15),	
Preco DECIMAL(10,2),
Descricao VARCHAR(200),
Tamanho VARCHAR(5),
Imagem VARCHAR(100),
Disponibilidade BOOLEAN,
Id_Categoria INT,
CONSTRAINT FK_Id_Categoria_Produto FOREIGN KEY(Id_Categoria) REFERENCES tbCategoria(Id_Categoria)
);

CREATE TABLE tbAvaliacao(
Id_Avaliacao INT PRIMARY KEY auto_increment,
Comentario VARCHAR(100),
Nota INT CHECK (Nota BETWEEN 1 AND 5),
DataAval DATETIME NOT NULL,
CodProduto INT,
Id_Usuario INT,
CONSTRAINT fk_Avaliacao_tbUsuario FOREIGN KEY (Id_Usuario) REFERENCES tbUsuario(Id_Usuario),
CONSTRAINT fk_Avaliacao_tbProduto FOREIGN KEY (CodProduto) REFERENCES tbProduto(CodProduto)
);

CREATE TABLE tbCartao(
Id_Cartao INT PRIMARY KEY,
Nome_Titular VARCHAR(75),
Apelido VARCHAR(50),
Numero_Mascarado VARCHAR(100),
DataVal DATE,
Id_Usuario INT,
CONSTRAINT FK_Cartao_tbUsuario FOREIGN KEY (Id_Usuario) REFERENCES tbUsuario(Id_Usuario)
);

CREATE TABLE tbItem_Pedido(
Id_Item INT PRIMARY KEY,
Preco_Unitario DECIMAL(10,2),
Quantidade SMALLINT,
Id_Pedido INT,
CodProduto INT,
CONSTRAINT FK_Item_Pedido_tbPedido FOREIGN KEY (Id_Pedido) REFERENCES tbPedido(Id_Pedido),
CONSTRAINT FK_Item_Pedido_tbProduto FOREIGN KEY (CodProduto) REFERENCES tbProduto(CodProduto)
);

CREATE TABLE tbOpcao_Personalizacao(
Id_Opcao INT PRIMARY KEY,
Tipo ENUM('Leite', 'Açucar', 'Adicionais'),
Opcao VARCHAR(50),
Preco_Extra DECIMAL(10,2),
Id_Categoria INT,
CONSTRAINT fk_Opcao_Personalizacao FOREIGN KEY (Id_Categoria) REFERENCES tbCategoria(Id_Categoria)
);

CREATE TABLE tbItemCarrinho_Opcao(
Id_Opcao INT,
Id_Item INT,
Preco DECIMAL(10,2),
PRIMARY KEY(Id_Opcao, Id_Item),
CONSTRAINT FK_ItemCarrinho_Opcao_tbOpcao_Personalizacao FOREIGN KEY (Id_Opcao) REFERENCES tbOpcao_Personalizacao(Id_Opcao),
CONSTRAINT FK_ItemCarrinho_Opcao_tbItem_Pedido FOREIGN KEY (Id_Item) REFERENCES tbItem_Pedido(Id_Item)
);

CREATE TABLE tbPagamento(
Id_Pagamento INT PRIMARY KEY auto_increment,
ValorPago DECIMAL(10,2) NOT NULL,
DataHora DATETIME NOT NULL,
FormaPagamento ENUM('PIX','Débito','Crédito') NOT NULL,
Status ENUM('Fnaliazado', 'Em andamento', '´Á Pagar'),
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
DataEmissao DATETIME NOT NULL,
CNPJ_Discoffe VARCHAR(15) NOT NULL,
Id_Pagamento INT,
CONSTRAINT fk_NF_tbPagamento FOREIGN KEY (Id_Pagamento) REFERENCES tbPagamento(Id_Pagamento)
);


DROP PROCEDURE IF EXISTS insertTbUsuario;

delimiter $$ 
create procedure insertTbUsuario(vNomeComple VARCHAR(30), vEmail VARCHAR(100), vSenha VARCHAR(100), vCPF VARCHAR(15), vStats ENUM('Ativo','Inativo'), vTelefone VARCHAR(15), vDataNasc DATE, vRestricoes ENUM('Celiaco','Lactose','Caseina','Diabete'))
begin
	insert into tbUsuario(NomeComple, Email, Senha, CPF, Stats, Telefone, DataNasc, Restricoes)
	values (vNomeComple, vEmail, vSenha, vCPF, vStats, vTelefone, vDataNasc, vRestricoes);
end;
$$


DROP PROCEDURE IF EXISTS insertTbPedido;

delimiter $$
create procedure insertTbPedido(vTipo ENUM('Retirada','Entrega'), vStats ENUM('Finalizado','Preparando','Cancelado'), vSubTotal DECIMAL(10,2), vTotal DECIMAL(10,2))
begin
	insert into tbPedido(Tipo, Stats, SubTotal, Total, DataHora)
    values (vTipo, vStats, vSubTotal, vTotal, CURRENT_TIMESTAMP);
end;
$$


DROP PROCEDURE IF EXISTS insertTbProduto;

delimiter $$
create procedure insertTbProduto(vCodProduto INT, vNome VARCHAR(15), vPreco DECIMAL(10,2), vDescricao VARCHAR(200), vQuantidade SMALLINT, vCategoria ENUM('Drink','Salgado','Doce','Refresco','Shake') )
begin
	insert into tbProduto (CodProduto, Nome, Preco, Descricao, Quantidade, Categoria)
    values (vCodProduto, vNome, vPreco, vDescricao, vQuantidade, vCategoria);
end;
$$


DROP PROCEDURE IF EXISTS insertTbEndereco;

delimiter $$
create procedure insertTbEndereco(vCEP VARCHAR(12), vBairro VARCHAR(50), vCidade VARCHAR(30), vEstado CHAR(2), vRua VARCHAR(50), vComplemento VARCHAR(10), vNumero VARCHAR(7), vId_Usuario INT)
begin
    insert into tbEndereco(CEP, Bairro, Cidade, Estado, Rua, Complemento, Numero, Id_Usuario)
    values(vCEP, vBairro, vCidade, vEstado, vRua, vComplemento, vNumero, vId_Usuario); -- Aqui teria que pensar em uma logica pra linkar esse id_Usuario com o cadastro do usuario (Pelo menos na minha mente seria isso)
end;
$$


DROP PROCEDURE IF EXISTS insertTbFuncionario;

delimiter $$
create procedure insertTbFuncionario(vCargo ENUM('Administrador','Gerente','Cozinheiro','Entregador'), vDataAdmissao DATE, vId_Usuario INT)
begin
    insert into tbFuncionario (Cargo, DataAdmissao, Id_Usuario)
    values (vCargo, vDataAdmissao, vId_Usuario); -- nessa parte pra mim não faz sentido ser referenciado com o usuario
end;
$$


DROP PROCEDURE IF EXISTS insertTbCarrinho;

delimiter $$
create procedure insertTbCarrinho(vSubTotal DECIMAL(10,2), vTotal DECIMAL(10,2), vId_Usuario INT)
begin
    insert into tbCarrinho (SubTotal, Total, Id_Usuario)
    values (vSubTotal, vTotal, vId_Usuario);
end;
$$


DROP PROCEDURE IF EXISTS insertTbAvaliacao;

delimiter $$
create procedure insertTbAvaliacao(vComentario VARCHAR(100), vNota INT, vCodProduto INT, vId_Usuario INT)
begin
    insert into tbAvaliacao (Comentario, Nota, DataAvaliacao, CodProduto, Id_Usuario)
    values (vComentario, vNota, CURRENT_TIMESTAMP, vCodProduto, vId_Usuario);
end;
$$


DROP PROCEDURE IF EXISTS insertTbCarrinhoProduto;

delimiter $$
create procedure insertTbCarrinhoProduto(vPreco_Unitario DECIMAL(10,2), vQuantidade SMALLINT, vId_Carrinho INT, vCodProduto INT)
begin
    insert into tbCarrinho_Produto (Preco_Unitario, Quantidade, Id_Carrinho, CodProduto)
    values (vPreco_Unitario, vQuantidade, vId_Carrinho, vCodProduto);
end;
$$


DROP PROCEDURE IF EXISTS insertTbCupom;

delimiter $$
create procedure insertTbCupom(vNome VARCHAR(10), vCodigo_Cupom VARCHAR(10), vDesconto DECIMAL(10,2), vDataValidade DATETIME)
begin
    insert into tbCupom (Nome, Codigo_Cupom, Desconto, DataValidade)
    values (vNome, vCodigo_Cupom, vDesconto, vDataValidade);
end;
$$


DROP PROCEDURE IF EXISTS insertTbPagamento;

delimiter $$
create procedure insertTbPagamento(vValorPago DECIMAL(10,2), vFormaPagamento ENUM('PIX','Débito','Crédito'), vId_Pedido INT, vId_Cupom INT)
begin
    insert into tbPagamento (ValorPago, DataHora, FormaPagamento, Id_Pedido, Id_Cupom)
    values (vValorPago, CURRENT_TIMESTAMP, vFormaPagamento, vId_Pedido, vId_Cupom);
end;
$$


DROP PROCEDURE IF EXISTS insertTbNF;

DELIMITER $$
create procedure insertTbNF(vValorTotal DECIMAL(10,2), vId_Pagamento INT)
begin
    insert into tbNF (ValorTotal, DataEmissao, Id_Pagamento)
    values (vValorTotal, CURRENT_TIMESTAMP, vId_Pagamento);
end;
$$


DROP PROCEDURE IF EXISTS insertTbPedidoProduto;

delimiter $$
create procedure insertTbPedidoProduto(vPreco_Unitario DECIMAL(10,2), vQuantidade SMALLINT, vId_Pedido INT, vCodProduto INT)
begin
    insert into tbPedido_Produto (Preco_Unitario, Total, Quantidade, Id_Pedido, CodProduto)
    VALUES (vPreco_Unitario, vPreco_Unitario * vQuantidade, vQuantidade, vId_Pedido, vCodProduto);
end;
$$


CALL insertTbUsuario('Samuel', 'sam1219barbosa@gmail.com', 'Teste01', '49381583889', 'Ativo', '11981863899', '2008-10-30', 'Celiaco');
CALL insertTbPedido('Retirada', 'Finalizado', 12.99, 12.99);
CALL insertTbProduto(1, 'Café Coado', 24.99, 'Café Coado', 35, 'Doce');
CALL insertTbEndereco('12345-678', 'Centro', 'São Paulo', 'SP', 'Rua das Flores', 'Apto 12', '150', 1);
CALL insertTbFuncionario('Cozinheiro', '2026-04-30', 1);
CALL insertTbCarrinho(35.90, 39.90, 1);
CALL insertTbAvaliacao('Muito bom, entrega rápida!', 5, 1, 1);
CALL insertTbCarrinhoProduto(12.99, 2, 1, 1);
CALL insertTbCupom('PROMO10', 'DESC10', 10.00, '2026-12-31 23:59:59');
CALL insertTbPagamento(59.90, 'PIX', 1, 1);
CALL insertTbNF(59.90, 1);
CALL insertTbPedidoProduto(12.99, 2, 1, 1);
