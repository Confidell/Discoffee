﻿using Discoffee.Repository.Contract;
using Discoffee.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace Discoffee.Repository
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly string _conexaoMySQL;
        public ProdutoRepository(IConfiguration conf)
        {
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
        }

         // CADASTRA UM PRODUTO. RETORNA false SE JÁ EXISTE UM COM O MESMO NOME E TAMANHO.
         public bool AdicionarProduto(Produto produto)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            using var cmd = new MySqlCommand("inserirProduto", conexao);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@vNome", produto.Nome);
            cmd.Parameters.AddWithValue("@vPreco", produto.Preco);
            cmd.Parameters.AddWithValue("@vDescricao", produto.Descricao);
            cmd.Parameters.AddWithValue("@vImagem", produto.Imagem);
            cmd.Parameters.AddWithValue("@vDisponibilidade", produto.Disponibilidade);
            cmd.Parameters.AddWithValue("@vId_Categoria", produto.Categoria.IdCategoria);

            // Parâmetro de SAÍDA: a procedure devolve o id criado (ou 0)
            var saida = new MySqlParameter("@vId_Produto", MySqlDbType.Int32) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(saida);

            cmd.ExecuteNonQuery();
            return Convert.ToInt32(saida.Value) > 0;
         }
    


        // BUSCA UM PRODUTO PELO SEU ID E RETORNA SEUS DADOS.
        public Produto BuscarProdutoPorId(int id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                const string sql = @"
                    SELECT
                        pr.Id_Produto,
                        pr.Nome,
                        pr.Preco,
                        pr.Descricao,
                        pr.Imagem,
                        pr.Disponibilidade,
                        pr.Id_Categoria,
                        c.Categoria
                    FROM tbProduto pr
                    LEFT JOIN tbCategoria c ON c.Id_Categoria = pr.Id_Categoria
                    WHERE pr.Id_Produto = @Id;";

                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Produto
                            {
                                IdProduto = reader.GetInt32("Id_Produto"),
                                Nome = reader.GetString("Nome"),
                                Preco = reader.GetDecimal("Preco"),
                                Descricao = reader.GetString("Descricao"),
                                Imagem = reader.GetString("Imagem"),
                                Disponibilidade = reader.GetBoolean("Disponibilidade"),
                                Categoria = new Categoria
                                {
                                    IdCategoria = reader.GetInt32("Id_Categoria"),
                                    NomeCategoria = reader.GetString("Categoria")
                                }
                            };
                        }
                    }
                }
            }
            return null;
        }

        // BUSCA TODOS OS PRODUTOS CADASTRADOS NO BANCO DE DADOS.
        public List<Produto> BuscarTodos()
        {
            var produtos = new List<Produto>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                using (var cmd = new MySqlCommand("buscarTodosProdutos", conexao))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            produtos.Add(new Produto
                            {
                                IdProduto = reader.GetInt32("Id_Produto"),
                                Nome = reader.GetString("Nome"),
                                Preco = reader.GetDecimal("Preco"),
                                Descricao = reader.GetString("Descricao"),
                                Imagem = reader.GetString("Imagem"),
                                Disponibilidade = reader.GetBoolean("Disponibilidade"),
                                Categoria = new Categoria
                                {
                                    IdCategoria = reader.GetInt32("Id_Categoria"),
                                    NomeCategoria = reader.GetString("Categoria")
                                }
                            });
                        }
                    }
                }
            }
            return produtos;
        }

        // ATUALIZA UM PRODUTO. RETORNA false SE OUTRO PRODUTO JÁ USA ESSE NOME E TAMANHO.
        public bool EditarProduto(Produto produto)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            using var cmd = new MySqlCommand("updateTbProduto", conexao);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@vId_Produto", produto.IdProduto);
            cmd.Parameters.AddWithValue("@vNome", produto.Nome);
            cmd.Parameters.AddWithValue("@vPreco", produto.Preco);
            cmd.Parameters.AddWithValue("@vDescricao", produto.Descricao);
            cmd.Parameters.AddWithValue("@vImagem", produto.Imagem);
            cmd.Parameters.AddWithValue("@vDisponibilidade", produto.Disponibilidade);
            cmd.Parameters.AddWithValue("@vId_Categoria", produto.Categoria.IdCategoria);

            var saida = new MySqlParameter("@vAtualizado", MySqlDbType.Int32) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(saida);

            cmd.ExecuteNonQuery();
            return Convert.ToInt32(saida.Value) == 1;
        }

        // ATIVA OU DESATIVA UM PRODUTO, SEM ALTERAR OS OUTROS DADOS.
        public void AlterarDisponibilidade(int id, bool disponivel)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            using var cmd = new MySqlCommand("alterarDisponibilidadeProduto", conexao);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@vId_Produto", id);
            cmd.Parameters.AddWithValue("@vDisponibilidade", disponivel);

            cmd.ExecuteNonQuery();
        }
    }
}