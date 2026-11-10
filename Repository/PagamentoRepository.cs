using System.Data;
using Discoffee.Models;
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

        public void EfetuarPagamento(Pagamento pagamento)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            using var comando = new MySqlCommand("registrarPagamento", conexao);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@vId_Usuario", pagamento.ValorPago);
            comando.Parameters.AddWithValue("@vId_Usuario", pagamento.FormaPagamento);
            comando.Parameters.AddWithValue("@vId_Usuario", pagamento.Status);
            comando.Parameters.AddWithValue("@vId_Usuario", pagamento.Pedido.IdPedido);
            comando.Parameters.AddWithValue("@vId_Usuario", pagamento.Cartao.IdCartao);
            comando.Parameters.AddWithValue("@vId_Usuario", pagamento.Cupom.IdCupom);

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
