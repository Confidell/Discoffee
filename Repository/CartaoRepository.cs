using Discoffee.Repository.Contract;
using Discoffee.Models;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.AspNetCore.Http.Connections;
using System.Globalization;

namespace Discoffee.Repository
{
    public class CartaoRepository : ICartaoRepository
    {
        private readonly string _conexaoMySQL;
        public CartaoRepository(IConfiguration conf)
        {
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
        }

        public void AdicionarCartao(Cartao cartao, int IdUsuario)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                DateTime dataValidade = DateTime.ParseExact(
                    cartao.DataValidade,
                    "MM/yy",
                    CultureInfo.InvariantCulture
                );

                using (var cmd = new MySqlCommand("cadastrarCartao", conexao))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@vNome_Titular", cartao.NomeTitular);
                    cmd.Parameters.AddWithValue("@vApelido", cartao.Apelido);
                    cmd.Parameters.AddWithValue("@vNumero_Mascarado", cartao.NumeroCartao);
                    cmd.Parameters.AddWithValue("@vDataVal", dataValidade);
                    cmd.Parameters.AddWithValue("@vId_Usuario", IdUsuario);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Cartao BuscarCartaoPorId(int id)
        {
            throw new NotImplementedException();
        }

        public List<Cartao> BuscarTodosPorUsuario(int IdUsuario)
        {
            var cartoes = new List<Cartao>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                using (var cmd = new MySqlCommand("select * from tbCartao where Id_Usuario = @vId_Usuario", conexao))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@vId_Usuario", IdUsuario);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cartoes.Add(new Cartao
                            {
                                IdCartao = reader.GetInt32("Id_Cartao"),
                                NomeTitular = reader.GetString("Nome_Titular"),
                                Apelido = reader.GetString("Apelido"),
                                NumeroCartao = reader.GetString("Numero_Mascarado"),
                                DataValidade = reader.GetDateTime("DataVal").ToString("MM/yy"),
                                Usuario = new Usuario
                                {
                                    IdUsuario = reader.GetInt32("Id_Usuario"),
                                }
                            });
                        }
                    }
                }
            }
            return cartoes;
        }

        public void EditarCartao(Cartao cartao)
        {
            throw new NotImplementedException();
        }
    }
}
