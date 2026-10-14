using System.Data;
using Discoffee.Models;
using Discoffee.Models.Constants;
using Discoffee.Repository.Contract;
using Discoffee.ViewModels;
using MySql.Data.MySqlClient;

namespace Discoffee.Repository
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly string _conexaoMySql;
        public PedidoRepository(IConfiguration config)
        {
            _conexaoMySql = config.GetConnectionString("ConexaoMySQL");
        }
        
        public Pedido BuscarPedidoPorId(int id)
        {
            throw new NotImplementedException();
        }

        public List<Pedido> BuscarPedidoPorStatus(StatusPedido status)
        {
            throw new NotImplementedException();
        }

        public List<Pedido> BuscarPedidoPorTipo(TipoPedido tipo)
        {
            throw new NotImplementedException();
        }

        public List<Pedido> BuscarPedidosPorUsuario(int usuarioId)
        {
            throw new NotImplementedException();
        }

        public List<Pedido> BuscarTodos()
        {
            throw new NotImplementedException();
        }

        public void CancelarPedido(int id)
        {
            throw new NotImplementedException();
        }

        public int CriarPedido(Pedido pedido)
        {
            using var conexao = new MySqlConnection(_conexaoMySql);
            conexao.Open();

            using var comando = new MySqlCommand("criarPedido", conexao);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@vTipo", pedido.Tipo);
            comando.Parameters.AddWithValue("@vStats", StatusPedido.Pendente);
            comando.Parameters.AddWithValue("@vFrete", pedido.Frete);
            comando.Parameters.AddWithValue("@vId_Usuario", pedido.UsuarioId);

            int idPedido = Convert.ToInt32(comando.ExecuteScalar());
            return idPedido;
        }

        public void EditarPedido(Pedido pedido)
        {
            throw new NotImplementedException();
        }

        public int InserirItemPedido(ItemPedidoViewModel itemPedido)
        {
            using var conexao = new MySqlConnection(_conexaoMySql);
            conexao.Open();

            using var comando = new MySqlCommand("inserirItemPedido", conexao);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@vId_Pedido", itemPedido.IdPedido);
            comando.Parameters.AddWithValue("@vId_Produto", itemPedido.IdProduto);
            comando.Parameters.AddWithValue("@vQuantidade", itemPedido.Quantidade);

            return Convert.ToInt32(comando.ExecuteScalar());
        }

        public void InserirOpcaoItemPedido(OpcaoPedidoViewModel opcaoPedido)
        {
            using var conexao = new MySqlConnection(_conexaoMySql);
            conexao.Open();

            using var comando = new MySqlCommand(
                "inserirOpcaoItemPedido", conexao);

            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@vId_Item", opcaoPedido.IdItem);
            comando.Parameters.AddWithValue("@vId_Opcao", opcaoPedido.IdOpcao);

            comando.ExecuteNonQuery();
        }

        public void AtualizarTotalPedido(int IdPedido)
        {
            using var conexao = new MySqlConnection(_conexaoMySql);
            conexao.Open();

            using var comando = new MySqlCommand(
                "atualizarTotalPedido", conexao);

            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@vId_Pedido", IdPedido);

            comando.ExecuteNonQuery();
        }
    }
}
