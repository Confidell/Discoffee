using Discoffee.Models;
using Discoffee.Repository.Contract;
using MySql.Data.MySqlClient;

namespace Discoffee.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly string _conexaoMySQL;

        public CategoriaRepository(IConfiguration conf)
        {
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
        }

        public List<Categoria> BuscarTodas()
        {
            var categorias = new List<Categoria>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                const string sql = "SELECT Id_Categoria, Categoria FROM tbCategoria ORDER BY Categoria;";

                using (var cmd = new MySqlCommand(sql, conexao))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categorias.Add(new Categoria
                        {
                            IdCategoria = reader.GetInt32("Id_Categoria"),
                            NomeCategoria = reader.GetString("Categoria")
                        });
                    }
                }
            }

            return categorias;
        }

        public CategoriaRepository BuscarCategoria(int id)
        {
            throw new NotImplementedException();
        }

        public void CadastrarCategoria(CategoriaRepository categoria)
        {
            throw new NotImplementedException();
        }

        public void EditarCategoria(CategoriaRepository categoria)
        {
            throw new NotImplementedException();
        }
    }
}
