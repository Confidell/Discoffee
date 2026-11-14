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
            foreach (var propriedade in pagamento.GetType().GetProperties())
            {
                Console.WriteLine($"{propriedade.Name}: {propriedade.GetValue(pagamento)}");
            }
            Console.WriteLine($"Pedido: {pagamento.Pedido.IdPedido}");

            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            using var comando = new MySqlCommand("registrarPagamento", conexao);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@vFormaPagamento", pagamento.FormaPagamento);
            comando.Parameters.AddWithValue("@vStats", pagamento.Status);
            comando.Parameters.AddWithValue("@vId_Pedido", pagamento.Pedido.IdPedido);
            comando.Parameters.AddWithValue("@vId_Cartao", (object?)pagamento.Cartao?.IdCartao ?? DBNull.Value);
            comando.Parameters.AddWithValue("@vId_Cupom", (object?)pagamento.Cupom?.IdCupom ?? DBNull.Value);

            comando.ExecuteNonQuery();
        }

        public List<Pagamento> VerPagamentosPorUsuario(int usuarioId)
        {
            throw new NotImplementedException();
        }

        public List<Pagamento> VerTodos()
        {
            throw new NotImplementedException();
        }
    }
}
