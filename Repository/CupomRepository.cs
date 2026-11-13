using System.Data;
using Discoffee.Models;
using Discoffee.Repository.Contract;
using MySql.Data.MySqlClient;

namespace Discoffee.Repository
{
    public class CupomRepository : ICupomRepository
    {
        private readonly string _conexaoMySQL;

        public CupomRepository(IConfiguration config)
        {
            _conexaoMySQL = config.GetConnectionString("ConexaoMySQL");
        }

        public void AdicionarCupom(Cupom cupom)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                using (var cmd = new MySqlCommand("insertTbCupom", conexao))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@vNome", cupom.Nome);
                    cmd.Parameters.AddWithValue("@vCodigo_Cupom", cupom.Codigo);
                    cmd.Parameters.AddWithValue("@vDesconto", cupom.Desconto);
                    cmd.Parameters.AddWithValue("@vDataValidade", cupom.DataValidade);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void EditarCupom(Cupom cupom)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                const string sql = @"
                    UPDATE tbCupom
                    SET
                        Nome = @Nome,
                        Codigo_Cupom = @Codigo,
                        Desconto = @Desconto,
                        DataValidade = @DataValidade
                    WHERE Id_Cupom = @Id;";

                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@Id", cupom.IdCupom);
                    cmd.Parameters.AddWithValue("@Nome", cupom.Nome);
                    cmd.Parameters.AddWithValue("@Codigo", cupom.Codigo);
                    cmd.Parameters.AddWithValue("@Desconto", cupom.Desconto);
                    cmd.Parameters.AddWithValue("@DataValidade", cupom.DataValidade);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void ExcluirCupom(int id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                const string sql = "UPDATE tbCupom SET DataValidade = NOW() WHERE Id_Cupom = @Id;";

                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Cupom? BuscarCupomPorCodigo(string codigo)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                const string sql = @"
                    SELECT Id_Cupom, Nome, Codigo_Cupom, Desconto, DataValidade
                    FROM tbCupom
                    WHERE Codigo_Cupom = @Codigo;";

                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@Codigo", codigo);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Cupom
                            {
                                IdCupom = reader.GetInt32("Id_Cupom"),
                                Nome = reader.IsDBNull(reader.GetOrdinal("Nome")) ? null : reader.GetString("Nome"),
                                Codigo = reader.GetString("Codigo_Cupom"),
                                Desconto = reader.GetDecimal("Desconto"),
                                DataValidade = reader.IsDBNull(reader.GetOrdinal("DataValidade"))
                                    ? DateTime.MinValue
                                    : reader.GetDateTime("DataValidade")
                            };
                        }
                    }
                }
            }

            return null;
        }
    }
}
