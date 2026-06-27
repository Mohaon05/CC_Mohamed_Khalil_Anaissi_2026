using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using trabalho_kaneko.Data;
using trabalho_kaneko.Models;

namespace trabalho_kaneko.Repository
{
    public class MarcaRepository
    {
        private readonly DbContext _context;

        public MarcaRepository(DbContext context)
        {
            _context = context;
        }

        public List<MarcaModel> ListarTodos()
        {
            List<MarcaModel> marcas = new List<MarcaModel>();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    // Usamos LEFT JOIN para garantir que a marca apareça mesmo se o id_grupo for nulo
                    string query = @"
                        SELECT m.*, g.grupo AS nome_grupo 
                        FROM marcas m 
                        LEFT JOIN grupos g ON m.id_grupo = g.id_grupo 
                        ORDER BY m.marca ASC";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            marcas.Add(new MarcaModel
                            {
                                IdMarca = Convert.ToInt32(reader["id_marca"]),
                                Marca = reader["marca"].ToString(),
                                IdGrupo = reader["id_grupo"] != DBNull.Value ? Convert.ToInt32(reader["id_grupo"]) : (int?)null,
                                NomeGrupo = reader["nome_grupo"].ToString(),
                                DataInclusao = Convert.ToDateTime(reader["data_inclusao"]),
                                DataAlteracao = reader["data_alteracao"] != DBNull.Value ? Convert.ToDateTime(reader["data_alteracao"]) : (DateTime?)null
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao listar marcas: " + ex.Message);
            }
            return marcas;
        }

        public bool Inserir(MarcaModel marca)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "INSERT INTO marcas (marca, id_grupo) VALUES (@marca, @id_grupo)";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@marca", marca.Marca);
                        command.Parameters.AddWithValue("@id_grupo", marca.IdGrupo.HasValue ? (object)marca.IdGrupo.Value : DBNull.Value);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao inserir marca: " + ex.Message);
                return false;
            }
        }

        public MarcaModel BuscarPorId(int id)
        {
            MarcaModel marca = null;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM marcas WHERE id_marca = @id";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                marca = new MarcaModel
                                {
                                    IdMarca = Convert.ToInt32(reader["id_marca"]),
                                    Marca = reader["marca"].ToString(),
                                    IdGrupo = reader["id_grupo"] != DBNull.Value ? Convert.ToInt32(reader["id_grupo"]) : (int?)null
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao buscar marca por ID: " + ex.Message);
            }
            return marca;
        }

        public bool Atualizar(MarcaModel marca)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "UPDATE marcas SET marca = @marca, id_grupo = @id_grupo, data_alteracao = NOW() WHERE id_marca = @id";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", marca.IdMarca);
                        command.Parameters.AddWithValue("@marca", marca.Marca);
                        command.Parameters.AddWithValue("@id_grupo", marca.IdGrupo.HasValue ? (object)marca.IdGrupo.Value : DBNull.Value);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar marca: " + ex.Message);
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
                    string query = "DELETE FROM marcas WHERE id_marca = @id";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao excluir marca: " + ex.Message);
                return false;
            }
        }
    }
}