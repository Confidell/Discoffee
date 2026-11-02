-- ATUALIZA O BANCO JÁ EXISTENTE PARA O CADASTRO/EDIÇÃO DE PRODUTO SEM TAMANHO
-- Execute no MySQL Workbench (com o schema do Discoffee selecionado).

-- 1. REMOVE A COLUNA TAMANHO
-- Se der erro "Can't DROP 'Tamanho'", a coluna já não existe: pule este comando.
ALTER TABLE tbProduto DROP COLUMN Tamanho;

-- 2. PRODUTO NOVO NASCE ATIVO POR PADRÃO
UPDATE tbProduto SET Disponibilidade = TRUE WHERE Disponibilidade IS NULL;
ALTER TABLE tbProduto MODIFY Disponibilidade BOOLEAN NOT NULL DEFAULT TRUE;

-- 3. RECRIA A PROCEDURE DE CADASTRO
DROP PROCEDURE IF EXISTS inserirProduto;
DELIMITER $$
CREATE PROCEDURE inserirProduto(
    vNome VARCHAR(50),
    vPreco DECIMAL(10,2),
    vDescricao VARCHAR(200),
    vImagem VARCHAR(100),
    vDisponibilidade BOOLEAN,
    vId_Categoria INT,
    OUT vId_Produto INT
)
BEGIN
    -- 0 = NÃO CADASTROU (JÁ EXISTE UM PRODUTO COM ESSE NOME)
    SET vId_Produto = 0;

    IF NOT EXISTS (SELECT 1 FROM tbProduto WHERE Nome = vNome) THEN
        INSERT INTO tbProduto(Nome, Preco, Descricao, Imagem, Disponibilidade, Id_Categoria)
        VALUES (vNome, vPreco, vDescricao, vImagem, vDisponibilidade, vId_Categoria);

        SET vId_Produto = LAST_INSERT_ID();
    END IF;
END $$
DELIMITER ;

-- 4. RECRIA A PROCEDURE DE EDIÇÃO (SEM TAMANHO)
DROP PROCEDURE IF EXISTS updateTbProduto;
DELIMITER $$
CREATE PROCEDURE updateTbProduto(
    IN  vId_Produto INT,
    IN  vNome VARCHAR(50),
    IN  vPreco DECIMAL(10,2),
    IN  vDescricao VARCHAR(200),
    IN  vImagem VARCHAR(100),
    IN  vDisponibilidade BOOLEAN,
    IN  vId_Categoria INT,
    OUT vAtualizado INT
)
BEGIN
    IF EXISTS (SELECT 1 FROM tbProduto
               WHERE Nome = vNome AND Id_Produto <> vId_Produto) THEN
        SET vAtualizado = 0;
    ELSE
        UPDATE tbProduto
        SET Nome = vNome,
            Preco = vPreco,
            Descricao = vDescricao,
            Imagem = vImagem,
            Disponibilidade = vDisponibilidade,
            Id_Categoria = vId_Categoria
        WHERE Id_Produto = vId_Produto;

        SET vAtualizado = 1;
    END IF;
END $$
DELIMITER ;

-- 5. CRIA A PROCEDURE QUE ATIVA/DESATIVA O PRODUTO (ALTERA SÓ A DISPONIBILIDADE)
DROP PROCEDURE IF EXISTS alterarDisponibilidadeProduto;
DELIMITER $$
CREATE PROCEDURE alterarDisponibilidadeProduto(
    vId_Produto INT,
    vDisponibilidade BOOLEAN
)
BEGIN
    UPDATE tbProduto
    SET Disponibilidade = vDisponibilidade
    WHERE Id_Produto = vId_Produto;
END $$
DELIMITER ;
