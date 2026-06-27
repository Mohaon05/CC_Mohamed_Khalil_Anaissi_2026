using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using trabalho_kaneko.Data;
using trabalho_kaneko.Models;

namespace trabalho_kaneko.Repository
{
    public class CargoRepository
    {
        private readonly DbContext _context;

        public CargoRepository(DbContext context)
        {
            _context = context;
        }

        public List<CargoModel> ListarTodos()
        {
            List<CargoModel> cargos = new List<CargoModel>();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM cargos ORDER BY cargo ASC";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cargos.Add(new CargoModel
                            {
                                IdCargo = Convert.ToInt32(reader["id_cargo"]),
                                Cargo = reader["cargo"].ToString(),
                                Descricao = reader["descricao"].ToString(),
                                DataInclusao = Convert.ToDateTime(reader["data_inclusao"]),
                                DataAlteracao = reader["data_alteracao"] != DBNull.Value ? Convert.ToDateTime(reader["data_alteracao"]) : (DateTime?)null
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao listar cargos: " + ex.Message);
            }
            return cargos;
        }

        public bool Inserir(CargoModel cargo)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "INSERT INTO cargos (cargo, descricao) VALUES (@cargo, @descricao)";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@cargo", cargo.Cargo);
                        command.Parameters.AddWithValue("@descricao", string.IsNullOrEmpty(cargo.Descricao) ? (object)DBNull.Value : cargo.Descricao);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao inserir cargo: " + ex.Message);
                return false;
            }
        }

        public CargoModel BuscarPorId(int id)
        {
            CargoModel cargo = null;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM cargos WHERE id_cargo = @id";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cargo = new CargoModel
                                {
                                    IdCargo = Convert.ToInt32(reader["id_cargo"]),
                                    Cargo = reader["cargo"].ToString(),
                                    Descricao = reader["descricao"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao buscar cargo por ID: " + ex.Message);
            }
            return cargo;
        }

        public bool Atualizar(CargoModel cargo)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "UPDATE cargos SET cargo = @cargo, descricao = @descricao, data_alteracao = NOW() WHERE id_cargo = @id";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", cargo.IdCargo);
                        command.Parameters.AddWithValue("@cargo", cargo.Cargo);
                        command.Parameters.AddWithValue("@descricao", string.IsNullOrEmpty(cargo.Descricao) ? (object)DBNull.Value : cargo.Descricao);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar cargo: " + ex.Message);
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
                    string query = "DELETE FROM cargos WHERE id_cargo = @id";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao excluir cargo: " + ex.Message);
                return false;
            }
        }
    }
}