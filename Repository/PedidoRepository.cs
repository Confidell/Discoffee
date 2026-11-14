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

        public List<PedidoClienteViewModel> BuscarPedidosPorUsuario(int IdUsuario)
        {
            var pedidos = new List<PedidoClienteViewModel>();

            using var conexao = new MySqlConnection(_conexaoMySql);
            conexao.Open();

            using var comando = new MySqlCommand("buscarPedidosPorUsuario", conexao);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@vId_Usuario", IdUsuario);

            using var reader = comando.ExecuteReader();

            while (reader.Read())
            {
                // OBTÉM O ID DO PEDIDO DA LINHA ATUAL
                int idPedido = Convert.ToInt32(reader["Id_Pedido"]);

                // VERIFICA SE ESSE PEDIDO JÁ FOI CRIADO NA LISTA
                var pedido = pedidos.FirstOrDefault(p => p.IdPedido == idPedido);

                // CRIA UM PEDIDO SE AINDA NÃO EXISTIR
                if (pedido == null)
                {
                    pedido = new PedidoClienteViewModel
                    {
                        IdPedido = idPedido,
                        DataHora = Convert.ToDateTime(reader["DataHora"]),
                        Subtotal = Convert.ToDecimal(reader["SubTotal"]),
                        Frete = Convert.ToDecimal(reader["Frete"]),
                        Total = Convert.ToDecimal(reader["Total"]),
                        Tipo = Enum.Parse<TipoPedido>(reader["Tipo_Pedido"].ToString()!, true),
                        Status = Enum.Parse<StatusPedido>(reader["Status_Pedido"].ToString().Replace(" ", "")!, true),
                        IdUsuario = Convert.ToInt32(reader["Id_Usuario"]),
                        Pagamento = new Pagamento
                        {
                            IdPagamento = reader["Id_Pagamento"] != DBNull.Value ? Convert.ToInt32(reader["Id_Pagamento"]) : null,
                            FormaPagamento = reader["FormaPagamento"] != DBNull.Value ? 
                                Enum.Parse<EnumFormaPagamento>(reader.GetString("FormaPagamento"), true) : null,
                            DataHora = reader["DataHora_Pagamento"] != DBNull.Value
                                ? Convert.ToDateTime(reader["DataHora_Pagamento"]) : null,
                            Status = reader["Status_Pagamento"] != DBNull.Value ? 
                                Enum.Parse<EnumStatusPagamento>(reader.GetString("Status_Pagamento"), true) : null
                        }
                    };

                    // ADICIONA O NOVO PEDIDO À LISTA PRINCIPAL
                    pedidos.Add(pedido);
                }

                // OBTÉM O ID DO ITEM DO PEDIDO NA LINHA ATUAL
                int idItem = Convert.ToInt32(reader["Id_Item"]);

                // PROCURA ESSE ITEM DENTRO DO PEDIDO ATUAL
                var item = pedido.Itens.FirstOrDefault(i => i.IdItem == idItem);

                // CRIA UM ITEM CASO AINDA NÃO EXISTA
                if (item == null)
                {
                    item = new ItemPedidoViewModel
                    {
                        IdItem = idItem,
                        IdPedido = idPedido,
                        NomeProduto = Convert.ToString(reader["Nome_Produto"]),
                        Quantidade = Convert.ToInt32(reader["Quantidade"]),
                        PrecoUnitario = Convert.ToDecimal(reader["Preco_Unitario"])
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
                        Opcao = reader["Opcao"]?.ToString(),
                        Tipo = Enum.Parse<TipoOpcao>(reader["Tipo_Personalizacao"].ToString()!),
                        PrecoExtra = Convert.ToDecimal(reader["Preco_Extra"])
                    });
                }
            }

            return pedidos;
        }

        public List<PedidoAdminViewModel> BuscarTodos()
        {
            var pedidos = new List<PedidoAdminViewModel>();

            using var conexao = new MySqlConnection(_conexaoMySql);
            conexao.Open();

            using var comando = new MySqlCommand("buscarTodosPedidos", conexao);
            comando.CommandType = CommandType.StoredProcedure;

            using var reader = comando.ExecuteReader();

            while (reader.Read())
            {
                int idPedido = Convert.ToInt32(reader["Id_Pedido"]);

                // PROCURA O PEDIDO NA LISTA
                var pedido = pedidos.FirstOrDefault(
                    p => p.IdPedido == idPedido
                );

                // CRIA O PEDIDO CASO AINDA NÃO EXISTA
                if (pedido == null)
                {
                    pedido = new PedidoAdminViewModel
                    {
                        IdPedido = idPedido,
                        DataHora = Convert.ToDateTime(reader["DataHora"]),
                        Subtotal = Convert.ToDecimal(reader["SubTotal"]),
                        Frete = Convert.ToDecimal(reader["Frete"]),
                        Total = Convert.ToDecimal(reader["Total"]),
                        UsuarioId = Convert.ToInt32(reader["Id_Usuario"]),
                        NomeCompleto = reader["NomeCompleto"].ToString()!,
                        CPF = reader["CPF"].ToString()!,
                        Tipo = Enum.Parse<TipoPedido>(reader["Tipo_Pedido"].ToString()!, true),
                        Status = Enum.Parse<StatusPedido>(reader["Status_Pedido"].ToString().Replace(" ", "")!, true),
                        Pagamento = new Pagamento
                        {
                            IdPagamento = reader["Id_Pagamento"] != DBNull.Value ? Convert.ToInt32(reader["Id_Pagamento"]) : null,
                            FormaPagamento = reader["FormaPagamento"] != DBNull.Value ? 
                                Enum.Parse<EnumFormaPagamento>(reader.GetString("FormaPagamento"), true) : null,
                            DataHora = reader["DataHora_Pagamento"] != DBNull.Value
                                ? Convert.ToDateTime(reader["DataHora_Pagamento"]) : null,
                            Status = reader["Status_Pagamento"] != DBNull.Value ? 
                                Enum.Parse<EnumStatusPagamento>(reader.GetString("Status_Pagamento"), true) : null
                        }
                        
                    };

                    pedidos.Add(pedido);
                }

                // IDENTIFICA O ITEM DO PEDIDO
                int idItem = Convert.ToInt32(reader["Id_Item"]);

                var item = pedido.Itens.FirstOrDefault(
                    i => i.IdItem == idItem
                );

                // CRIA O ITEM CASO AINDA NÃO EXISTA
                if (item == null)
                {
                    item = new ItemPedidoViewModel
                    {
                        IdItem = idItem,
                        IdPedido = idPedido,
                        IdProduto = Convert.ToInt32(reader["Id_Produto"]),
                        NomeProduto = reader["Nome_Produto"].ToString()!,
                        Quantidade = Convert.ToInt32(reader["Quantidade"]),
                        PrecoUnitario = Convert.ToDecimal(reader["Preco_Unitario"])
                    };

                    pedido.Itens.Add(item);
                }

                // VERIFICA SE JÁ EXISTE UMA OPÇÃO
                if (reader["Id_Opcao"] != DBNull.Value)
                {
                    int idOpcao = Convert.ToInt32(reader["Id_Opcao"]);

                    // EVITA DUPLICAR OPÇÃO NO ITEM
                    bool opcaoExiste = item.Opcoes.Any(o => o.IdOpcao == idOpcao);

                    if (!opcaoExiste)
                    {
                        item.Opcoes.Add(new OpcaoPersonalizacao
                        {
                            IdOpcao = idOpcao,
                            Tipo = Enum.Parse<TipoOpcao>(reader["Tipo_Personalizacao"].ToString()!,true),
                            Opcao = reader["Opcao"].ToString()!,
                            PrecoExtra = Convert.ToDecimal(reader["Preco_Extra"])
                        });
                    }
                }


            }

            return pedidos;
        }

        public void AlterarStatusPedido(int IdPedido, StatusPedido status)
        {
            using var conexao = new MySqlConnection(_conexaoMySql);
            conexao.Open();

            using var comando = 
                new MySqlCommand("update tbPedido set Stats = @vStats where Id_Pedido = @vIdPedido and Stats <> @vStats", conexao);

            comando.Parameters.AddWithValue("@vStats", status);
            comando.Parameters.AddWithValue("@vIdPedido", IdPedido);

            comando.ExecuteNonQuery();
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
            comando.Parameters.AddWithValue("@vId_Usuario", pedido.Usuario.IdUsuario);

            // EXECUTA PROCEDURE E RECEBE O ID DO PEDIDO RETORNADO COM O "EXECUTE SCALAR"
            int idPedido = Convert.ToInt32(comando.ExecuteScalar());
            return idPedido; // RETORNA ID DO PEDIDO CRIADO
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

            // RETORNA DIRETO O ID DO ITEM INSERIDO NO PEDIDO
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

        public PedidoClienteViewModel? BuscarPedidoPorId(int idPedido)
        {
            using var conexao = new MySqlConnection(_conexaoMySql);
            conexao.Open();

            string sql = @"
                SELECT
                    -- PEDIDO
                    pe.Id_Pedido,
                    pe.Id_Usuario,
                    pe.DataHora,
                    pe.Tipo AS Tipo_Pedido,
                    pe.Stats AS Status_Pedido,
                    pe.SubTotal,
                    pe.Frete,
                    pe.Total,
                    pe.Id_Usuario,

                    -- ITEM
                    ip.Id_Item,
                    ip.Id_Produto,
                    p.Nome AS Nome_Produto,
                    ip.Quantidade,
                    ip.Preco_Unitario,

                    -- PERSONALIZACAO
                    ipo.Id_Opcao,
                    op.Tipo AS Tipo_Personalizacao,
                    op.Opcao,
                    ipo.Preco AS Preco_Extra,

                    -- PAGAMENTO
                    pg.Id_Pagamento,
                    pg.FormaPagamento,
                    pg.DataHora AS DataHora_Pagamento,
                    pg.Stats AS Status_Pagamento

                FROM tbPedido AS pe

                INNER JOIN tbUsuario AS u
                    ON pe.Id_Usuario = u.Id_Usuario

                INNER JOIN tbItem_Pedido AS ip
                    ON pe.Id_Pedido = ip.Id_Pedido

                INNER JOIN tbProduto AS p
                    ON ip.Id_Produto = p.Id_Produto

                LEFT JOIN tbItemPedido_Opcao AS ipo
                    ON ip.Id_Item = ipo.Id_Item

                LEFT JOIN tbOpcao_Personalizacao AS op
                    ON ipo.Id_Opcao = op.Id_Opcao

                LEFT JOIN tbPagamento AS pg
		            ON pe.Id_Pedido = pg.Id_Pedido

                WHERE pe.Id_Pedido = @IdPedido;
            ";

            using var comando = new MySqlCommand(sql, conexao);
            comando.Parameters.AddWithValue("@IdPedido", idPedido);

            using var reader = comando.ExecuteReader();
            PedidoClienteViewModel? pedido = null;

            while (reader.Read())
            {
                // CRIA O PEDIDO CASO AINDA NÃO EXISTA
                if (pedido == null)
                {
                    pedido = new PedidoClienteViewModel
                    {
                        IdPedido = Convert.ToInt32(reader["Id_Pedido"]),
                        DataHora = Convert.ToDateTime(reader["DataHora"]),
                        Subtotal = Convert.ToDecimal(reader["SubTotal"]),
                        Frete = Convert.ToDecimal(reader["Frete"]),
                        Total = Convert.ToDecimal(reader["Total"]),
                        Tipo = Enum.Parse<TipoPedido>(reader["Tipo_Pedido"].ToString()!, true),
                        Status = Enum.Parse<StatusPedido>(reader["Status_Pedido"].ToString()!.Replace(" ", ""), true),
                        IdUsuario = Convert.ToInt32(reader["Id_Usuario"]),
                        Pagamento = new Pagamento
                        {
                            IdPagamento = reader["Id_Pagamento"] != DBNull.Value ? Convert.ToInt32(reader["Id_Pagamento"]) : null,
                            FormaPagamento = reader["FormaPagamento"] != DBNull.Value ? 
                                Enum.Parse<EnumFormaPagamento>(reader.GetString("FormaPagamento"), true) : null,
                            DataHora = reader["DataHora_Pagamento"] != DBNull.Value
                                ? Convert.ToDateTime(reader["DataHora_Pagamento"]) : null,
                            Status = reader["Status_Pagamento"] != DBNull.Value ? 
                                Enum.Parse<EnumStatusPagamento>(reader.GetString("Status_Pagamento"), true) : null
                        }
                    };
                }

                // IDENTIFICA O ITEM DO PEDIDO
                int idItem = Convert.ToInt32(reader["Id_Item"]);

                var item = pedido.Itens.FirstOrDefault(
                    i => i.IdItem == idItem
                );

                // CRIA O ITEM CASO AINDA NÃO EXISTA
                if (item == null)
                {
                    item = new ItemPedidoViewModel
                    {
                        IdItem = idItem,
                        IdPedido = idPedido,
                        IdProduto = Convert.ToInt32(reader["Id_Produto"]),
                        NomeProduto = reader["Nome_Produto"].ToString()!,
                        Quantidade = Convert.ToInt32(reader["Quantidade"]),
                        PrecoUnitario = Convert.ToDecimal(reader["Preco_Unitario"])
                    };

                    pedido.Itens.Add(item);
                }

                // VERIFICA SE EXISTE UMA PERSONALIZAÇÃO
                if (reader["Id_Opcao"] != DBNull.Value)
                {
                    int idOpcao = Convert.ToInt32(reader["Id_Opcao"]);

                    // EVITA DUPLICAR OPÇÃO NO ITEM
                    bool opcaoExiste = item.Opcoes.Any(
                        o => o.IdOpcao == idOpcao
                    );

                    if (!opcaoExiste)
                    {
                        item.Opcoes.Add(new OpcaoPersonalizacao
                        {
                            IdOpcao = idOpcao,
                            Tipo = Enum.Parse<TipoOpcao>(
                                reader["Tipo_Personalizacao"].ToString()!, true
                            ),
                            Opcao = reader["Opcao"].ToString()!,
                            PrecoExtra = Convert.ToDecimal(reader["Preco_Extra"])
                        });
                    }
                }
            }

            return pedido;
        }

    }
}