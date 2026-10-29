using Discoffee.Repository.Contract;
using Discoffee.Models;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.AspNetCore.Http.Connections;

namespace Discoffee.Repository
{
    public class CartaoRepository : ICartaoRepository
    {
        private readonly string _conexaoMySQL;
        public CartaoRepository(IConfiguration conf)
        {
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
        }

        public void AdicionarCartao(Cartao cartao)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                using (var cmd = new MySqlCommand("insertTbCartao", conexao))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@vNome_Titular", cartao.NomeTitular);
                    cmd.Parameters.AddWithValue("@vApelido", cartao.Apelido);
                    cmd.Parameters.AddWithValue("@vNumero_Mascarado", cartao.NumeroCartao);
                    cmd.Parameters.AddWithValue("@vDataVal", cartao.DataValidade);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Cartao BuscarCartaoPorId(int id)
        {
            throw new NotImplementedException();
        }

        public List<Cartao> BuscarTodosPorUsuario()
        {
            var cartao = new List<Cartao>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                using (var cmd = new MySqlCommand("selectTbCartao", conexao))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cartao.Add(new Cartao
                            {
                                IdCartao = reader.GetInt32("Id_Cartao"),
                                NomeTitular = reader.GetString("Nome_Titular"),
                                Apelido = reader.GetString("Apelido"),
                                NumeroCartao = reader.GetString("Numero_Mascarado"),
                                DataValidade = reader.GetString("DataVal"),
                                Usuario = new Usuario
                                {
                                    IdUsuario = reader.GetInt32("Id_Usuario"),
                                }
                            });
                        }
                    }
                }
            }
            return cartao;
        }

        public void EditarCartao(Cartao cartao)
        {
            throw new NotImplementedException();
        }
    }
}
