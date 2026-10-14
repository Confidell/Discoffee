using Discoffee.Repository.Contract;
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

        // CADASTRA UM NOVO PRODUTO NO BANCO DE DADOS.
        public void AdicionarProduto(Produto produto)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                using (var cmd = new MySqlCommand("insertTbProduto", conexao))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@vNome", produto.Nome);
                    cmd.Parameters.AddWithValue("@vPreco", produto.Preco);
                    cmd.Parameters.AddWithValue("@vDescricao", produto.Descricao);
                    cmd.Parameters.AddWithValue("@vTamanho", produto.Tamanho);
                    cmd.Parameters.AddWithValue("@vImagem", produto.Imagem);
                    cmd.Parameters.AddWithValue("@vDisponibilidade", produto.Disponibilidade);
                    cmd.Parameters.AddWithValue("@vCategoria", produto.Categoria?.NomeCategoria);

                    cmd.ExecuteNonQuery();  
                }
            }
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
                        pr.Tamanho,
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
                                Tamanho = reader.GetString("Tamanho"),
                                Imagem = reader.GetString("Imagem"),
                                Disponibilidade = reader.GetBoolean("Disponibilidade"),
                                CategoriaId = reader.GetInt32("Id_Categoria"),
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

                using (var cmd = new MySqlCommand("selectTbProdutos", conexao))
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
                                Tamanho = reader.GetString("Tamanho"),
                                Imagem = reader.GetString("Imagem"),
                                Disponibilidade = reader.GetBoolean("Disponibilidade"),
                                CategoriaId = reader.GetInt32("Id_Categoria"),
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

        // ATUALIZA OS DADOS DE UM PRODUTO EXISTENTE NO BANCO DE DADOS.
        public void EditarProduto(Produto produto)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                int idCategoria = ObterOuCriarCategoriaId(produto.Categoria?.NomeCategoria, conexao);

                const string sql = @"
                    UPDATE tbProduto
                    SET
                        Nome = @Nome,
                        Preco = @Preco,
                        Descricao = @Descricao,
                        Tamanho = @Tamanho,
                        Imagem = @Imagem,
                        Disponibilidade = @Disponibilidade,
                        Id_Categoria = @IdCategoria
                    WHERE Id_Produto = @Id;";

                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@Id", produto.IdProduto);
                    cmd.Parameters.AddWithValue("@Nome", produto.Nome);
                    cmd.Parameters.AddWithValue("@Preco", produto.Preco);
                    cmd.Parameters.AddWithValue("@Descricao", produto.Descricao);
                    cmd.Parameters.AddWithValue("@Tamanho", produto.Tamanho);
                    cmd.Parameters.AddWithValue("@Imagem", produto.Imagem);
                    cmd.Parameters.AddWithValue("@Disponibilidade", produto.Disponibilidade);
                    cmd.Parameters.AddWithValue("@IdCategoria", idCategoria);

                    cmd.ExecuteNonQuery();
                }
            }

        }

        //MODIFIQUEI DE EXCLUIR PARA DISPONIBILIDADE

        // DESATIVA A DISPONIBILIDADE DO PRODUTO SEM REMOVÊ-LO DO BANCO DE DADOS.

        //public void DisponibilidadeProduto(int id)
        //{
        //    using (var conexao = new MySqlConnection(_conexaoMySQL))
        //    {
        //        conexao.Open();
        //        const string sql = "UPDATE tbProduto SET Disponibilidade = 0 WHERE Id_Produto = @Id;";
        //        using (var cmd = new MySqlCommand(sql, conexao))
        //        {
        //            cmd.Parameters.AddWithValue("@Id", id);
        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //}

        // OBTÉM O ID DE UMA CATEGORIA OU CRIA A CATEGORIA E RETORNA SEU ID.
        private int ObterOuCriarCategoriaId(string? nomeCategoria, MySqlConnection conexaoAberta)
        {
            using (var cmd = new MySqlCommand("insertTbCategoria", conexaoAberta))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("vCategoria", nomeCategoria);
                var paramSaida = new MySqlParameter("vId_Categoria", MySqlDbType.Int32) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(paramSaida);
                cmd.ExecuteNonQuery();
                return Convert.ToInt32(paramSaida.Value);
            }
        }
    }
}
