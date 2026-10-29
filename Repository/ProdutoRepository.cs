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

        // CADASTRA UM PRODUTO. RETORNA false SE JÁ EXISTE UM COM O MESMO NOME E TAMANHO.
        public bool AdicionarProduto(Produto produto)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            using var cmd = new MySqlCommand("insertTbProduto", conexao);
            cmd.CommandType = CommandType.StoredProcedure;

<<<<<<< HEAD
                    cmd.Parameters.AddWithValue("@vNome", produto.Nome);
                    cmd.Parameters.AddWithValue("@vPreco", produto.Preco);
                    cmd.Parameters.AddWithValue("@vDescricao", produto.Descricao);
                    cmd.Parameters.AddWithValue("@vImagem", produto.Imagem);
                    cmd.Parameters.AddWithValue("@vDisponibilidade", produto.Disponibilidade);
                    cmd.Parameters.AddWithValue("@vCategoria", produto.Categoria?.NomeCategoria);
=======
            cmd.Parameters.AddWithValue("@vNome", produto.Nome);
            cmd.Parameters.AddWithValue("@vPreco", produto.Preco);
            cmd.Parameters.AddWithValue("@vDescricao", produto.Descricao);
            cmd.Parameters.AddWithValue("@vTamanho", produto.Tamanho);
            cmd.Parameters.AddWithValue("@vImagem", produto.Imagem);
            cmd.Parameters.AddWithValue("@vDisponibilidade", produto.Disponibilidade);
            cmd.Parameters.AddWithValue("@vId_Categoria", produto.CategoriaId);
>>>>>>> e255ec04ae59d36fa9e437dc1ed89dd335622091

            // Parâmetro de SAÍDA: a procedure devolve o id criado (ou 0)
            var saida = new MySqlParameter("@vId_Produto", MySqlDbType.Int32) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(saida);

            cmd.ExecuteNonQuery();
            return Convert.ToInt32(saida.Value) > 0;
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
            cmd.Parameters.AddWithValue("@vTamanho", produto.Tamanho);
            cmd.Parameters.AddWithValue("@vImagem", produto.Imagem);
            cmd.Parameters.AddWithValue("@vDisponibilidade", produto.Disponibilidade);
            cmd.Parameters.AddWithValue("@vId_Categoria", produto.CategoriaId);

            var saida = new MySqlParameter("@vAtualizado", MySqlDbType.Int32) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(saida);

            cmd.ExecuteNonQuery();
            return Convert.ToInt32(saida.Value) == 1;
        }

        // BUSCA UM PRODUTO PELO ID. RETORNA null SE NÃO EXISTIR.
        public Produto BuscarProdutoPorId(int id)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            const string sql = @"
                SELECT pr.Id_Produto, pr.Nome, pr.Preco, pr.Descricao, pr.Tamanho,
                       pr.Imagem, pr.Disponibilidade, pr.Id_Categoria, c.Categoria
                FROM tbProduto pr
                INNER JOIN tbCategoria c ON c.Id_Categoria = pr.Id_Categoria
                WHERE pr.Id_Produto = @Id;";

            using var cmd = new MySqlCommand(sql, conexao);
            cmd.Parameters.AddWithValue("@Id", id);

<<<<<<< HEAD
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
=======
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? LerProduto(reader) : null;
>>>>>>> e255ec04ae59d36fa9e437dc1ed89dd335622091
        }

        // BUSCA TODOS OS PRODUTOS.
        public List<Produto> BuscarTodos()
        {
            var produtos = new List<Produto>();

            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            using var cmd = new MySqlCommand("buscarTodosProdutos", conexao);
            cmd.CommandType = CommandType.StoredProcedure;

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                produtos.Add(LerProduto(reader));

<<<<<<< HEAD
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
=======
>>>>>>> e255ec04ae59d36fa9e437dc1ed89dd335622091
            return produtos;
        }

        // Converte a linha atual do reader em Produto (evita repetir o código)
        private static Produto LerProduto(MySqlDataReader reader) => new()
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
<<<<<<< HEAD
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
                    cmd.Parameters.AddWithValue("@Imagem", produto.Imagem);
                    cmd.Parameters.AddWithValue("@Disponibilidade", produto.Disponibilidade);
                    cmd.Parameters.AddWithValue("@IdCategoria", idCategoria);

                    cmd.ExecuteNonQuery();
                }
=======
                IdCategoria = reader.GetInt32("Id_Categoria"),
                NomeCategoria = reader.GetString("Categoria")
>>>>>>> e255ec04ae59d36fa9e437dc1ed89dd335622091
            }
        };
    }
}