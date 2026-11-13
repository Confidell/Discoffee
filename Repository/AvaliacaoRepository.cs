using Discoffee.Repository.Contract;
using MySql.Data.MySqlClient;
using System.Data;
using Discoffee.Models;

namespace Discoffee.Repository
{
    public class AvaliacaoRepository : IAvaliacaoRepository
    {
        private readonly string _conexaoMySQL;

        public AvaliacaoRepository(IConfiguration config)
        {
            _conexaoMySQL = config.GetConnectionString("ConexaoMySQL");
        }

        public void AdicionarAvaliacao(Avaliacao avaliacao)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                using (var cmd = new MySqlCommand("insertTbAvaliacao", conexao))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@vComentario", avaliacao.Comentario);
                    cmd.Parameters.AddWithValue("@vNota", avaliacao.Nota);
                    cmd.Parameters.AddWithValue("@vId_Produto", avaliacao.Produto.IdProduto);
                    cmd.Parameters.AddWithValue("@vId_Usuario", avaliacao.Usuario.IdUsuario);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Avaliacao> BuscarAvaliacoes()
        {
            var avaliacoes = new List<Avaliacao>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                const string sql = @"
                    SELECT Id_Avaliacao, Comentario, Nota, DataAval, Id_Produto, Id_Usuario
                    FROM tbAvaliacao
                    ORDER BY DataAval DESC;";

                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            avaliacoes.Add(new Avaliacao
                            {
                                IdAvaliacao = reader.GetInt32("Id_Avaliacao"),
                                Comentario = reader.GetString("Comentario"),
                                Nota = reader.GetInt32("Nota"),
                                Data = reader.GetDateTime("DataAval"),
                                Produto = new Produto { IdProduto = reader.GetInt32("Id_Produto") },
                                Usuario = new Usuario { IdUsuario = reader.GetInt32("Id_Usuario") }

                            });
                        }
                    }
                }
            }

            return avaliacoes;
        }

        public List<Avaliacao> BuscarAvaliacoesPorProduto(int produtoId)
        {
            var avaliacoes = new List<Avaliacao>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                const string sql = @"
                    SELECT Id_Avaliacao, Comentario, Nota, DataAval, Id_Produto, Id_Usuario
                    FROM tbAvaliacao
                    WHERE Id_Produto = @IdProduto
                    ORDER BY DataAval DESC;";

                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@IdProduto", produtoId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            avaliacoes.Add(new Avaliacao
                            {
                                IdAvaliacao = reader.GetInt32("Id_Avaliacao"),
                                Comentario = reader.GetString("Comentario"),
                                Nota = reader.GetInt32("Nota"),
                                Data = reader.GetDateTime("DataAval"),
                                Produto = new Produto { IdProduto = reader.GetInt32("Id_Produto") },
                                Usuario = new Usuario { IdUsuario = reader.GetInt32("Id_Usuario") }

                            });
                        }
                    }
                }
            }

            return avaliacoes;
        }

        public List<Avaliacao> BuscarAvaliacoesPorUsuario(int usuarioId)
        {
            var avaliacoes = new List<Avaliacao>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                const string sql = @"
                    SELECT Id_Avaliacao, Comentario, Nota, DataAval, Id_Produto, Id_Usuario
                    FROM tbAvaliacao
                    WHERE Id_Usuario = @IdUsuario
                    ORDER BY DataAval DESC;";

                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@IdUsuario", usuarioId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            avaliacoes.Add(new Avaliacao
                            {
                                IdAvaliacao = reader.GetInt32("Id_Avaliacao"),
                                Comentario = reader.GetString("Comentario"),
                                Nota = reader.GetInt32("Nota"),
                                Data = reader.GetDateTime("DataAval"),
                                Produto = new Produto { IdProduto = reader.GetInt32("Id_Produto") },
                                Usuario = new Usuario { IdUsuario = reader.GetInt32("Id_Usuario") }

                            });
                        }
                    }
                }
            }

            return avaliacoes;
        }

        public void EditarAvaliacao(Avaliacao avaliacao)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                const string sql = @"
                    UPDATE tbAvaliacao
                    SET
                        Comentario = @Comentario,
                        Nota = @Nota
                    WHERE Id_Avaliacao = @Id;";

                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@Id", avaliacao.IdAvaliacao);
                    cmd.Parameters.AddWithValue("@Comentario", avaliacao.Comentario);
                    cmd.Parameters.AddWithValue("@Nota", avaliacao.Nota);

                    cmd.ExecuteNonQuery();
                }
            }

        }

        public void ExcluirAvaliacao(int id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                const string sql = "DELETE FROM tbAvaliacao WHERE Id_Avaliacao = @Id;";

                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
