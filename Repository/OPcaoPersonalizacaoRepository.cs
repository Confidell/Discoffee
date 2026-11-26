using System.Data;
using Discoffee.Models;
using Discoffee.Models.Constants;
using Discoffee.Repository.Contract;
using MySql.Data.MySqlClient;

namespace Discoffee.Repository
{
    public class OpcaoPersonalizacaoRepository : IOpcaoPersonalizacaoRepository
    {
        private readonly string _conexaoMySql;
        public OpcaoPersonalizacaoRepository(IConfiguration config)
        {
            _conexaoMySql = config.GetConnectionString("ConexaoMySQL");
        }

        public void AdicionarOpcao(OpcaoPersonalizacao opcao)
        {
            using var conexao = new MySqlConnection(_conexaoMySql);
            conexao.Open();

            using var cmd = new MySqlCommand("cadastrarOpcaoPersonalizacao", conexao);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("vTipo", (int)opcao.Tipo);
            cmd.Parameters.AddWithValue("vOpcao", opcao.Opcao);
            cmd.Parameters.AddWithValue("vPreco_Extra", opcao.PrecoExtra);
            cmd.Parameters.AddWithValue("vCategoria", opcao.Categoria?.NomeCategoria);

            cmd.ExecuteNonQuery();
        }

        public List<OpcaoPersonalizacao> BuscarOpcao()
        {
            var opcoes = new List<OpcaoPersonalizacao>();

            using var conexao = new MySqlConnection(_conexaoMySql);
            conexao.Open();

            const string sql = @"
                SELECT o.Id_Opcao, o.Tipo, o.Opcao, o.Preco_Extra, c.Id_Categoria, c.Categoria
                FROM tbOpcao_Personalizacao o
                INNER JOIN tbCategoria c ON c.Id_Categoria = o.Id_Categoria
                ORDER BY o.Id_Opcao;";

            using var cmd = new MySqlCommand(sql, conexao);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                opcoes.Add(new OpcaoPersonalizacao
                {
                    IdOpcao = reader.GetInt32("Id_Opcao"),
                    Tipo = Enum.Parse<TipoOpcao>(reader.GetString("Tipo")),
                    Opcao = reader.GetString("Opcao"),
                    PrecoExtra = reader.GetDecimal("Preco_Extra"),
                    Categoria = new Categoria
                    {
                        IdCategoria = reader.GetInt32("Id_Categoria"),
                        NomeCategoria = reader.GetString("Categoria")
                    }
                });
            }

            return opcoes;
        }

        public OpcaoPersonalizacao BuscarOpcaoPorId(int id)
        {
            throw new NotImplementedException();
        }

        // BUSCA AS OPÇÕES DE PERSONALIZAÇÃO DISPONÍVEIS PARA UMA CATEGORIA.
        public List<OpcaoPersonalizacao> BuscarOpcoesPorCategoria(int idCategoria)
        {
            var opcoes = new List<OpcaoPersonalizacao>();

            using var conexao = new MySqlConnection(_conexaoMySql);
            using var comando = new MySqlCommand("buscarOpcoesPersonalizacaoPorProduto", conexao);

            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("vId_Categoria", idCategoria);

            conexao.Open();
            using var reader = comando.ExecuteReader();

            while (reader.Read())
            {
                opcoes.Add(new OpcaoPersonalizacao
                {
                    IdOpcao = reader.GetInt32("Id_Opcao"),
                    Tipo = Enum.Parse<TipoOpcao>(reader.GetString("Tipo")),
                    Opcao = reader.GetString("Opcao"),
                    PrecoExtra = reader.GetDecimal("Preco_Extra")
                });
            }

            return opcoes;
        }

        public void EditarOpcao(OpcaoPersonalizacao opcao)
        {
            using var conexao = new MySqlConnection(_conexaoMySql);
            conexao.Open();

            const string sql = @"
                UPDATE tbOpcao_Personalizacao
                SET Tipo = @Tipo, Opcao = @Opcao, Preco_Extra = @Preco, Id_Categoria = @IdCategoria
                WHERE Id_Opcao = @Id;";

            using var cmd = new MySqlCommand(sql, conexao);
            cmd.Parameters.AddWithValue("@Id", opcao.IdOpcao);
            cmd.Parameters.AddWithValue("@Tipo", opcao.Tipo.ToString());
            cmd.Parameters.AddWithValue("@Opcao", opcao.Opcao);
            cmd.Parameters.AddWithValue("@Preco", opcao.PrecoExtra);
            cmd.Parameters.AddWithValue("@IdCategoria", opcao.Categoria?.IdCategoria);
            cmd.ExecuteNonQuery();
        }

        public void ExcluirOpcao(int id)
        {
            using var conexao = new MySqlConnection(_conexaoMySql);
            conexao.Open();

            const string sql = "DELETE FROM tbOpcao_Personalizacao WHERE Id_Opcao = @Id;";

            using var cmd = new MySqlCommand(sql, conexao);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
        }

        public void InserirOpcaoPedido(OpcaoPersonalizacao opcao, int idPedido)
        {
            
        }
    }
}
