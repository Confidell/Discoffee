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
            throw new NotImplementedException();
        }

        public List<OpcaoPersonalizacao> BuscarOpcao()
        {
            throw new NotImplementedException();
        }

        public OpcaoPersonalizacao BuscarOpcaoPorId(int id)
        {
            throw new NotImplementedException();
        }

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
                    Tipo = Enum.Parse<TipoOpcao>(reader.GetString("Tipo")),
                    Opcao = reader.GetString("Opcao"),
                    PrecoExtra = reader.GetDecimal("Preco_Extra")
                });
            }

            return opcoes;
        }

        public void EditarOpcao(OpcaoPersonalizacao opcao)
        {
            throw new NotImplementedException();
        }

        public void ExcluirOpcao(int id)
        {
            throw new NotImplementedException();
        }
    }
}
