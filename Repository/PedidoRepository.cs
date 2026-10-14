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

        public List<Pedido> BuscarPedidosPorUsuario(int IdPedido)
        {
            var pedidos = new List<Pedido>();

            using var conexao = new MySqlConnection(_conexaoMySql);
            conexao.Open();

            using var comando = new MySqlCommand("buscarPedidosPorUsuario", conexao);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@vId_Usuario", IdPedido);

            using var reader = comando.ExecuteReader();

            while (reader.Read())
            {
                // OBTÉM O ID DO PEDIDO DA LINHA ATUAL
                int idPedido = Convert.ToInt32(reader["Id_Pedido"]);

                // VERIFICA SE ESSE PEDIDO JÁ FOI CRIADO NA LISTA
                var pedido = pedidos.FirstOrDefault(p => p.IdPedido == idPedido);

                // SE O PEDIDO AINDA NÃO EXISTIR, CRIA UM NOVO OBJETO
                if (pedido == null)
                {
                    pedido = new Pedido
                    {
                        IdPedido = idPedido,
                        Subtotal = Convert.ToDecimal(reader["SubTotal"]),
                        Frete = Convert.ToDecimal(reader["Frete"]),
                        Total = Convert.ToDecimal(reader["Total"]),
                        UsuarioId = Convert.ToInt32(reader["Id_Usuario"]),
                        Tipo = (TipoPedido)Convert.ToInt32(reader["Tipo"]),
                        Status = (StatusPedido)Convert.ToInt32(reader["Stats"])
                    };

                    // ADICIONA O NOVO PEDIDO À LISTA PRINCIPAL
                    pedidos.Add(pedido);
                }

                // OBTÉM O ID DO ITEM DO PEDIDO NA LINHA ATUAL
                int idItem = Convert.ToInt32(reader["Id_Item"]);

                // PROCURA ESSE ITEM DENTRO DO PEDIDO ATUAL
                var item = pedido.Itens.FirstOrDefault(i => i.IdItem == idItem);

                // SE O ITEM AINDA NÃO EXISTIR, CRIA UM NOVO OBJETO
                if (item == null)
                {
                    item = new ItemPedido
                    {
                        IdItem = idItem,
                        PedidoId = idPedido,
                        ProdutoId = Convert.ToInt32(reader["Id_Produto"]),
                        Quantidade = Convert.ToInt32(reader["Quantidade"]),
                        PrecoUnitario = Convert.ToDecimal(reader["Preco_Unitario"]),
                    };

                    // ADICIONA O ITEM À LISTA DE ITENS DO PEDIDO
                    pedido.Itens.Add(item);
                }

                // VERIFICA SE A LINHA POSSUI UMA OPÇÃO DE PERSONALIZAÇÃO
                if (reader["Id_Opcao"] != DBNull.Value)
                {
                    // CRIA A PERSONALIZAÇÃO E A ADICIONA AO ITEM CORRESPONDENTE
                    item.Opcoes.Add(new OpcaoPersonalizacao
                    {
                        IdOpcao = Convert.ToInt32(reader["Id_Opcao"]),
                        Opcao = reader["Nome_Opcao"]?.ToString(),
                        Tipo = Enum.Parse<TipoOpcao>(reader["Tipo_Personalizacao"].ToString()!),
                        PrecoExtra = Convert.ToDecimal(reader["Preco_Personalizacao"])
                    });
                }
            }

            return pedidos;
        }

        public List<Pedido> BuscarTodos()
        {
            var pedidos = new List<Pedido>();

            using var conexao = new MySqlConnection(_conexaoMySql);
            conexao.Open();

            using var comando = new MySqlCommand("selectTbPedidos", conexao);
            comando.CommandType = CommandType.StoredProcedure;

            using var reader = comando.ExecuteReader();

            while (reader.Read())
            {
                // OBTÉM O ID DO PEDIDO DA LINHA ATUAL
                int idPedido = Convert.ToInt32(reader["Id_Pedido"]);

                // VERIFICA SE ESSE PEDIDO JÁ FOI CRIADO NA LISTA
                var pedido = pedidos.FirstOrDefault(p => p.IdPedido == idPedido);

                // SE O PEDIDO AINDA NÃO EXISTIR, CRIA UM NOVO OBJETO
                if (pedido == null)
                {
                    pedido = new Pedido
                    {
                        IdPedido = idPedido,
                        Subtotal = Convert.ToDecimal(reader["SubTotal"]),
                        Frete = Convert.ToDecimal(reader["Frete"]),
                        Total = Convert.ToDecimal(reader["Total"]),
                        UsuarioId = Convert.ToInt32(reader["Id_Usuario"]),
                        Tipo = (TipoPedido)Convert.ToInt32(reader["Tipo"]),
                        Status = (StatusPedido)Convert.ToInt32(reader["Stats"])
                    };

                    // ADICIONA O NOVO PEDIDO À LISTA PRINCIPAL
                    pedidos.Add(pedido);
                }

                // OBTÉM O ID DO ITEM DO PEDIDO NA LINHA ATUAL
                int idItem = Convert.ToInt32(reader["Id_Item"]);

                // PROCURA ESSE ITEM DENTRO DO PEDIDO ATUAL
                var item = pedido.Itens.FirstOrDefault(i => i.IdItem == idItem);

                // SE O ITEM AINDA NÃO EXISTIR, CRIA UM NOVO OBJETO
                if (item == null)
                {
                    item = new ItemPedido
                    {
                        IdItem = idItem,
                        PedidoId = idPedido,
                        ProdutoId = Convert.ToInt32(reader["Id_Produto"]),
                        Quantidade = Convert.ToInt32(reader["Quantidade"]),
                        PrecoUnitario = Convert.ToDecimal(reader["Preco_Unitario"]),
                    };

                    // ADICIONA O ITEM À LISTA DE ITENS DO PEDIDO
                    pedido.Itens.Add(item);
                }

                // VERIFICA SE A LINHA POSSUI UMA OPÇÃO DE PERSONALIZAÇÃO
                if (reader["Id_Opcao"] != DBNull.Value)
                {
                    // CRIA A PERSONALIZAÇÃO E A ADICIONA AO ITEM CORRESPONDENTE
                    item.Opcoes.Add(new OpcaoPersonalizacao
                    {
                        IdOpcao = Convert.ToInt32(reader["Id_Opcao"]),
                        Opcao = reader["Nome_Opcao"]?.ToString(),
                        Tipo = Enum.Parse<TipoOpcao>(reader["Tipo_Personalizacao"].ToString()!),
                        PrecoExtra = Convert.ToDecimal(reader["Preco_Personalizacao"])
                    });
                }
            }

            return pedidos;
        }

        public void CancelarPedido(int IdPedido)
        {
            
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
