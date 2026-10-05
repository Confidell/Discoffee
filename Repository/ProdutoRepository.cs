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

        public void AdicionarProduto(Produto produto)
        {
            throw new NotImplementedException();
        }

        public Produto BuscarProdutoPorId(int id)
        {
            throw new NotImplementedException();
        }

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

        public void EditarProduto(Produto produto)
        {
            throw new NotImplementedException();
        }

        public void ExcluirProduto(int id)
        {
            throw new NotImplementedException();
        }
    }
}
