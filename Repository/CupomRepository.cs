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

        public List<Cupom> BuscarTodosCupons()
        {
            var cupons = new List<Cupom>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                const string sql = @"
                   SELECT Id_Cupom, Codigo_Cupom, Desconto, DataValidade
                   FROM tbCupom
                   ORDER BY Id_Cupom ASC;";

                using (var cmd = new MySqlCommand(sql, conexao))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cupons.Add(new Cupom
                        {
                            IdCupom = reader.GetInt32("Id_Cupom"),
                            Codigo = reader.GetString("Codigo_Cupom"),
                            Desconto = reader.GetDecimal("Desconto"),
                            DataValidade = reader.IsDBNull(reader.GetOrdinal("DataValidade"))
                                ? (DateTime?)null
                                : reader.GetDateTime("DataValidade")
                        });
                    }
                }
            }

            return cupons;
        }

        public void AdicionarCupom(Cupom cupom)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                using (var cmd = new MySqlCommand("cadastrarCupom", conexao))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@vCodigo_Cupom", cupom.Codigo?.ToUpper());
                    cmd.Parameters.AddWithValue("@vDesconto", cupom.Desconto);
                    cmd.Parameters.AddWithValue("@vDataValidade",
                        cupom.DataValidade?.ToString("dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture));

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
                        Codigo_Cupom = @Codigo,
                        Desconto = @Desconto,
                        DataValidade = @DataValidade
                    WHERE Id_Cupom = @Id;";

                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@Id", cupom.IdCupom);
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
                    SELECT Id_Cupom, Codigo_Cupom, Desconto, DataValidade
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

        public bool CupomUtilizado(int IdUsuario, int IdCupom)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                const string sql = @"select 1 from tbCupom_Utilizado 
                    where Id_Usuario = @vId_Usuario and Id_cupom = @vId_Cupom limit 1;";

                var cmd = new MySqlCommand(sql, conexao);
                cmd.Parameters.AddWithValue("@vId_Usuario", IdUsuario);
                cmd.Parameters.AddWithValue("@vId_Cupom", IdCupom);

                return cmd.ExecuteScalar() != null;
            }
        }
    }
}
