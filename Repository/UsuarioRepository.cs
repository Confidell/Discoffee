using System.Data;
using Discoffee.Models;
using Discoffee.Models.Constants;
using Discoffee.Repository.Contract;
using MySql.Data.MySqlClient;

namespace Discoffee.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _conexaoMySql;
        public UsuarioRepository(IConfiguration config)
        {
            _conexaoMySql = config.GetConnectionString("ConexaoMySQL");
        }

        public List<Usuario> BuscarTodos()
        {
            throw new NotImplementedException();
        }

        public Usuario BuscarUsuarioPorEmail(string Email)
        {
            using var conexao = new MySqlConnection(_conexaoMySql);
            conexao.Open();

                string join = @"
                    SELECT
                        u.Id_Usuario,
                        u.NomeCompleto,
                        u.Email,
                        u.Senha,
                        u.CPF,
                        u.Telefone,
                        u.DataNasc,
                        u.Stats,
                        u.Numero_Casa,
                        u.Complemento,
                        c.CEP,
                        p.Tipo AS Perfil
                    FROM tbUsuario u
                    LEFT JOIN tbPerfil p ON p.Id_Perfil = u.Id_Perfil
                    LEFT JOIN tbCEP c ON c.Id_CEP = u.Id_CEP
                    WHERE u.Email = @Email;";

            using var cmd = new MySqlCommand(join, conexao);
            cmd.Parameters.AddWithValue("@Email", Email);

            using MySqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                return new Usuario
                {
                    IdUsuario = Convert.ToInt32(dr["Id_Usuario"]),
                    NomeCompleto = Convert.ToString(dr["NomeCompleto"]),
                    Email = Convert.ToString(dr["Email"]),
                    Senha = Convert.ToString(dr["Senha"]),
                    CPF = Convert.ToString(dr["CPF"]),
                    Telefone = Convert.ToString(dr["Telefone"]),
                    DataNascimento = Convert.ToDateTime(dr["DataNasc"]),
                    Status = Convert.ToString(dr["Stats"]),
                    Complemento = Convert.ToString(dr["Complemento"]),
                    NumeroCasa = Convert.ToString(dr["Numero_Casa"]),
                    CEP = Convert.ToString(dr["CEP"]),
                    Perfil = Enum.Parse<EnumPerfil>(Convert.ToString(dr["Perfil"]))
                };
            }

            return null;
        }

        public Usuario? BuscarUsuarioPorId(int Id)
        {
            using var conexao = new MySqlConnection(_conexaoMySql);
            conexao.Open();

            using var comando = new MySqlCommand("selectUsuario", conexao);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("vId_Usuario", Id);

            using var reader = comando.ExecuteReader();
            if (reader.Read())
            {
                return new Usuario
                {
                    // IdUsuario = reader.GetInt32("Id_Usuario"),
                    NomeCompleto = reader.GetString("NomeCompleto"),
                    Email = reader.GetString("Email"),
                    // Senha = reader.GetString("Senha"),
                    CPF = reader.GetString("CPF"),
                    Telefone = reader.GetString("Telefone"),
                    DataNascimento = reader.GetDateTime("DataNasc"),
                    Status = reader.GetString("Stats"),
                    Complemento = reader.GetString("Complemento"),
                    NumeroCasa = reader.GetString("Numero_Casa"),
                    CEP = reader.GetString("CEP"),
                    Perfil = Enum.Parse<EnumPerfil>(reader.GetString("Perfil"))
                };
            }

            return null;
        }

        public bool BuscaUsuarioPorCPF(string CPF)
        {
            using var conexao = new MySqlConnection(_conexaoMySql);
            conexao.Open();

            using var cmd = new MySqlCommand(@"SELECT * FROM tbUsuario WHERE CPF = @CPF;", conexao);
            cmd.Parameters.AddWithValue("@CPF", CPF);

            using MySqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                return true;
            }

            return false;
        }

        public void CadastrarUsuario(Usuario usuario)
        {
            using var conexao = new MySqlConnection(_conexaoMySql);
            conexao.Open();

            using var comando = new MySqlCommand("insertTbUsuario", conexao);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@vNomeCompleto", usuario.NomeCompleto);
            comando.Parameters.AddWithValue("@vEmail", usuario.Email);
            comando.Parameters.AddWithValue("@vSenha", usuario.Senha);
            comando.Parameters.AddWithValue("@vCPF", usuario.CPF);
            comando.Parameters.AddWithValue("@vTelefone", usuario.Telefone);
            comando.Parameters.AddWithValue("@vDataNasc", usuario.DataNascimento);
            comando.Parameters.AddWithValue("@vNumero_Casa", usuario.NumeroCasa);
            comando.Parameters.AddWithValue("@vComplemento", usuario.Complemento);
            comando.Parameters.AddWithValue("@vCEP", usuario.CEP);
            comando.Parameters.AddWithValue("@vPerfil", usuario.Perfil);

            comando.ExecuteNonQuery();
        }

        public void EditarUsuario(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public Usuario LoginUsuario(string email, string senha)
        {
            throw new NotImplementedException();
        }
    }
}
