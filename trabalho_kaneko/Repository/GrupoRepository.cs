using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using trabalho_kaneko.Data;
using trabalho_kaneko.Models;

namespace trabalho_kaneko.Repository
{
    public class GrupoRepository
    {
        private readonly DbContext _context;

        public GrupoRepository(DbContext context)
        {
            _context = context;
        }

        public List<GrupoModel> ListarTodos()
        {
            List<GrupoModel> grupos = new List<GrupoModel>();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM grupos ORDER BY grupo ASC";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            grupos.Add(new GrupoModel
                            {
                                IdGrupo = Convert.ToInt32(reader["id_grupo"]),
                                Grupo = reader["grupo"].ToString(),
                                DataInclusao = Convert.ToDateTime(reader["data_inclusao"]),
                                DataAlteracao = reader["data_alteracao"] != DBNull.Value ? Convert.ToDateTime(reader["data_alteracao"]) : (DateTime?)null
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao listar grupos: " + ex.Message);
            }
            return grupos;
        }

        public bool Inserir(GrupoModel grupo)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "INSERT INTO grupos (grupo) VALUES (@grupo)";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@grupo", grupo.Grupo);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao inserir grupo: " + ex.Message);
                return false;
            }
        }

        public GrupoModel BuscarPorId(int id)
        {
            GrupoModel grupo = null;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM grupos WHERE id_grupo = @id";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                grupo = new GrupoModel
                                {
                                    IdGrupo = Convert.ToInt32(reader["id_grupo"]),
                                    Grupo = reader["grupo"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao buscar grupo por ID: " + ex.Message);
            }
            return grupo;
        }

        public bool Atualizar(GrupoModel grupo)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "UPDATE grupos SET grupo = @grupo, data_alteracao = NOW() WHERE id_grupo = @id";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", grupo.IdGrupo);
                        command.Parameters.AddWithValue("@grupo", grupo.Grupo);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar grupo: " + ex.Message);
                return false;
            }
        }

        public bool Excluir(int id)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "DELETE FROM grupos WHERE id_grupo = @id";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao excluir grupo: " + ex.Message);
                return false;
            }
        }
    }
}