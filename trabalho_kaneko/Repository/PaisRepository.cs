using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using trabalho_kaneko.Data;
using trabalho_kaneko.Models;

namespace trabalho_kaneko.Repository
{
    public class PaisRepository
    {
        private readonly DbContext _context;

        public PaisRepository(DbContext context)
        {
            _context = context;
        }

        public bool Inserir(PaisModel pais)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();

                    // Não precisamos enviar data_inclusao e data_alteracao, o MySQL preenche sozinho
                    string query = @"INSERT INTO paises (pais, sigla) VALUES (@pais, @sigla)";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@pais", pais.Pais);
                        command.Parameters.AddWithValue("@sigla", pais.Sigla.ToUpper()); // Força a sigla a ficar maiúscula

                        int linhasAfetadas = command.ExecuteNonQuery();
                        return linhasAfetadas > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao inserir país: " + ex.Message);
                return false;
            }
        }

        public List<PaisModel> ListarTodos()
        {
            List<PaisModel> paises = new List<PaisModel>();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "SELECT id_pais, pais, sigla, data_inclusao, data_alteracao FROM paises ORDER BY pais ASC";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                paises.Add(new PaisModel
                                {
                                    IdPais = Convert.ToInt32(reader["id_pais"]),
                                    Pais = reader["pais"].ToString(),
                                    Sigla = reader["sigla"].ToString(),
                                    DataInclusao = Convert.ToDateTime(reader["data_inclusao"]),
                                    // ADICIONADO: Lê a data de alteração do banco (tratando caso ela esteja nula)
                                    DataAlteracao = reader["data_alteracao"] != DBNull.Value ? Convert.ToDateTime(reader["data_alteracao"]) : null
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao listar países: " + ex.Message);
            }
            return paises;
        }
        // NOVO: Método para buscar os dados de apenas UM país pelo seu ID (para preencher a tela)
        public PaisModel BuscarPorId(int id)
        {
            PaisModel pais = null;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "SELECT id_pais, pais, sigla, data_inclusao FROM paises WHERE id_pais = @id";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                pais = new PaisModel
                                {
                                    IdPais = Convert.ToInt32(reader["id_pais"]),
                                    Pais = reader["pais"].ToString(),
                                    Sigla = reader["sigla"].ToString(),
                                    DataInclusao = Convert.ToDateTime(reader["data_inclusao"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao buscar país por ID: " + ex.Message);
            }
            return pais;
        }

        // NOVO: Método que faz o UPDATE de fato no banco de dados
        public bool Atualizar(PaisModel pais)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();

                    // O MySQL atualiza a data_alteracao sozinho graças ao ON UPDATE CURRENT_TIMESTAMP
                    string query = @"UPDATE paises SET pais = @pais, sigla = @sigla WHERE id_pais = @id";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@pais", pais.Pais);
                        command.Parameters.AddWithValue("@sigla", pais.Sigla.ToUpper());
                        command.Parameters.AddWithValue("@id", pais.IdPais);

                        int linhasAfetadas = command.ExecuteNonQuery();
                        return linhasAfetadas > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar país: " + ex.Message);
                return false;
            }
        }

        // NOVO: Método para deletar um registro pelo ID
        public bool Excluir(int id)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "DELETE FROM paises WHERE id_pais = @id";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        int linhasAfetadas = command.ExecuteNonQuery();
                        return linhasAfetadas > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Se cair aqui, provavelmente é porque o país está sendo usado em algum estado (chave estrangeira)
                Console.WriteLine("Erro ao excluir país: " + ex.Message);
                return false;
            }
        }

    }
}