using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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

        public List<PaisModel> ListarTodos()
        {
            List<PaisModel> paises = new List<PaisModel>();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM paises ORDER BY pais ASC";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            paises.Add(new PaisModel
                            {
                                IdPais = Convert.ToInt32(reader["id_pais"]),
                                Pais = reader["pais"].ToString(),
                                Sigla = reader["sigla"].ToString(),
                                Ddi = reader["ddi"].ToString(),
                                Moeda = reader["moeda"].ToString(),
                                DataInclusao = Convert.ToDateTime(reader["data_inclusao"]),
                                DataAlteracao = reader["data_alteracao"] != DBNull.Value ? Convert.ToDateTime(reader["data_alteracao"]) : (DateTime?)null
                            });
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

        public bool Inserir(PaisModel pais)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "INSERT INTO paises (pais, sigla, ddi, moeda) VALUES (@pais, @sigla, @ddi, @moeda)";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@pais", pais.Pais);
                        command.Parameters.AddWithValue("@sigla", pais.Sigla);
                        command.Parameters.AddWithValue("@ddi", pais.Ddi);
                        command.Parameters.AddWithValue("@moeda", pais.Moeda);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao inserir país: " + ex.Message);
                return false;
            }
        }

        public PaisModel BuscarPorId(int id)
        {
            PaisModel pais = null;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM paises WHERE id_pais = @id";
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
                                    Ddi = reader["ddi"].ToString(),
                                    Moeda = reader["moeda"].ToString()
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

        public bool Atualizar(PaisModel pais)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "UPDATE paises SET pais = @pais, sigla = @sigla, ddi = @ddi, moeda = @moeda, data_alteracao = NOW() WHERE id_pais = @id";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", pais.IdPais);
                        command.Parameters.AddWithValue("@pais", pais.Pais);
                        command.Parameters.AddWithValue("@sigla", pais.Sigla);
                        command.Parameters.AddWithValue("@ddi", pais.Ddi);
                        command.Parameters.AddWithValue("@moeda", pais.Moeda);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar país: " + ex.Message);
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
                    string query = "DELETE FROM paises WHERE id_pais = @id";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao excluir país: " + ex.Message);
                return false;
            }
        }

        // Adicione junto com os outros métodos no PaisRepository.cs
        public int InserirRetornandoId(PaisModel pais)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    // A mágica do MySQL: Insere e já busca o ID gerado na mesma hora
                    string query = @"INSERT INTO paises (pais, sigla, ddi, moeda) 
                             VALUES (@pais, @sigla, @ddi, @moeda); 
                             SELECT LAST_INSERT_ID();";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@pais", pais.Pais);
                        command.Parameters.AddWithValue("@sigla", pais.Sigla);
                        command.Parameters.AddWithValue("@ddi", pais.Ddi);
                        command.Parameters.AddWithValue("@moeda", pais.Moeda);

                        // ExecuteScalar retorna a primeira coluna da primeira linha (o nosso ID)
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao inserir país via Pop-up: " + ex.Message);
                return 0;
            }
        }

    }
}