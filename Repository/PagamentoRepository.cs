using System.Data;
using Discoffee.Models;
using Discoffee.Models.Constants;
using Discoffee.Repository.Contract;
using MySql.Data.MySqlClient;

namespace Discoffee.Repository
{
    public class PagamentoRepository : IPagamentoRepository
    {  
        private readonly string _conexaoMySQL;
        public PagamentoRepository(IConfiguration config)
        {
            _conexaoMySQL = config.GetConnectionString("ConexaoMySQL");
        }

        public void AlterarStatusPagamento(int idPagamento, EnumStatusPagamento statusPagamento)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            using var comando = 
                new MySqlCommand("update tbPagamento set Stats = @vStats where Id_Pagamento = @vIdPagamento and Stats <> @vStats", conexao);

            comando.Parameters.AddWithValue("@vStats", statusPagamento);
            comando.Parameters.AddWithValue("@vIdPagamento", idPagamento);

            comando.ExecuteNonQuery();
        }

        public void EfetuarPagamento(Pagamento pagamento)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            using var comando = new MySqlCommand("registrarPagamento", conexao);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@vId_Usuario", pagamento?.Pedido?.Usuario.IdUsuario);
            comando.Parameters.AddWithValue("@vFormaPagamento", pagamento?.FormaPagamento);
            comando.Parameters.AddWithValue("@vStats", pagamento?.Status);
            comando.Parameters.AddWithValue("@vId_Pedido", pagamento?.Pedido?.IdPedido);
            comando.Parameters.AddWithValue("@vId_Cartao", (object?)pagamento?.Cartao?.IdCartao ?? DBNull.Value);
            comando.Parameters.AddWithValue("@vId_Cupom", (object?)pagamento?.Cupom?.IdCupom ?? DBNull.Value);

            comando.ExecuteNonQuery();
        }

        public List<Pagamento> VerPagamentosPorUsuario(int usuarioId)
        {
            throw new NotImplementedException();
        }

        public List<Pagamento> VerTodos()
        {   
            var pagamentos = new List<Pagamento>();

            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();
            
            string sql = @"
                select * from tbPagamento
                where Stats = @vStats;
            ";

            using var comando = new MySqlCommand(sql, conexao);
            comando.Parameters.AddWithValue("@vStats", EnumStatusPagamento.Pago.ToString());
            using var reader = comando.ExecuteReader();

            while (reader.Read())
            {
                int idPagamento = Convert.ToInt32(reader["Id_Pagamento"]);

                var pagamento = pagamentos.FirstOrDefault(
                    pag => pag.IdPagamento == idPagamento
                );

                if(pagamento == null)
                {
                    pagamento = new Pagamento
                    {
                        IdPagamento = idPagamento,
                        ValorPago = Convert.ToDecimal(reader["ValorPago"]),
                        DataHora = Convert.ToDateTime(reader["DataHora"]),
                        FormaPagamento = Enum.Parse<EnumFormaPagamento>(reader["FormaPagamento"].ToString(), true),
                        Status = Enum.Parse<EnumStatusPagamento>(reader["Stats"].ToString(), true),
                        Pedido = new Pedido
                        {
                            IdPedido = Convert.ToInt32(reader["Id_Pedido"]),
                        },
                    };

                    pagamentos.Add(pagamento);
                }
            }

            Console.WriteLine($"PAGAMENTOS ENCONTRADOS {pagamentos[0].IdPagamento} {pagamentos[0].ValorPago}");

            return pagamentos;
        }
    }
}
