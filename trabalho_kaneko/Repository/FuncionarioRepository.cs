using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using trabalho_kaneko.Data;
using trabalho_kaneko.Models;

namespace trabalho_kaneko.Repository
{
    public class FuncionarioRepository
    {
        private readonly DbContext _context;

        public FuncionarioRepository(DbContext context)
        {
            _context = context;
        }

        // 1. LISTAR TODOS (Com duplo INNER JOIN)
        public List<FuncionarioModel> ListarTodos()
        {
            List<FuncionarioModel> funcionarios = new List<FuncionarioModel>();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = @"
                        SELECT f.*, c.cidade AS nome_cidade, car.cargo AS nome_cargo
                        FROM funcionarios f
                        INNER JOIN cidades c ON f.id_cidade = c.id_cidade
                        INNER JOIN cargos car ON f.id_cargo = car.id_cargo
                        ORDER BY f.funcionario ASC";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            funcionarios.Add(new FuncionarioModel
                            {
                                IdFuncionario = Convert.ToInt32(reader["id_funcionario"]),
                                Funcionario = reader["funcionario"].ToString(),
                                Apelido = reader["apelido"].ToString(),
                                Cpf = reader["cpf"].ToString(),
                                Rg = reader["rg"].ToString(),
                                DataNascimento = reader["data_nascimento"] != DBNull.Value ? Convert.ToDateTime(reader["data_nascimento"]) : (DateTime?)null,
                                Email = reader["email"].ToString(),
                                Telefone = reader["telefone"].ToString(),
                                IdCargo = Convert.ToInt32(reader["id_cargo"]),
                                Salario = reader["salario"] != DBNull.Value ? Convert.ToDecimal(reader["salario"]) : (decimal?)null,
                                Logradouro = reader["logradouro"].ToString(),
                                Numero = reader["numero"].ToString(),
                                Complemento = reader["complemento"].ToString(),
                                Bairro = reader["bairro"].ToString(),
                                Cep = reader["cep"].ToString(),
                                IdCidade = Convert.ToInt32(reader["id_cidade"]),
                                DataContratado = reader["data_contratado"] != DBNull.Value ? Convert.ToDateTime(reader["data_contratado"]) : (DateTime?)null,
                                DataDemitido = reader["data_demitido"] != DBNull.Value ? Convert.ToDateTime(reader["data_demitido"]) : (DateTime?)null,
                                NomeCidade = reader["nome_cidade"].ToString(),
                                NomeCargo = reader["nome_cargo"].ToString(),
                                DataInclusao = Convert.ToDateTime(reader["data_inclusao"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao listar funcionários: " + ex.Message);
            }
            return funcionarios;
        }

        // 2. INSERIR
        public bool Inserir(FuncionarioModel f)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = @"INSERT INTO funcionarios 
                        (funcionario, apelido, cpf, rg, data_nascimento, email, telefone, id_cargo, salario, logradouro, numero, complemento, bairro, cep, id_cidade, data_contratado, data_demitido) 
                        VALUES 
                        (@funcionario, @apelido, @cpf, @rg, @data_nascimento, @email, @telefone, @id_cargo, @salario, @logradouro, @numero, @complemento, @bairro, @cep, @id_cidade, @data_contratado, @data_demitido)";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@funcionario", f.Funcionario);
                        command.Parameters.AddWithValue("@apelido", string.IsNullOrEmpty(f.Apelido) ? (object)DBNull.Value : f.Apelido);
                        command.Parameters.AddWithValue("@cpf", f.Cpf);
                        command.Parameters.AddWithValue("@rg", string.IsNullOrEmpty(f.Rg) ? (object)DBNull.Value : f.Rg);
                        command.Parameters.AddWithValue("@data_nascimento", f.DataNascimento.HasValue ? (object)f.DataNascimento.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@email", string.IsNullOrEmpty(f.Email) ? (object)DBNull.Value : f.Email);
                        command.Parameters.AddWithValue("@telefone", string.IsNullOrEmpty(f.Telefone) ? (object)DBNull.Value : f.Telefone);
                        command.Parameters.AddWithValue("@id_cargo", f.IdCargo);
                        command.Parameters.AddWithValue("@salario", f.Salario.HasValue ? (object)f.Salario.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@logradouro", string.IsNullOrEmpty(f.Logradouro) ? (object)DBNull.Value : f.Logradouro);
                        command.Parameters.AddWithValue("@numero", string.IsNullOrEmpty(f.Numero) ? (object)DBNull.Value : f.Numero);
                        command.Parameters.AddWithValue("@complemento", string.IsNullOrEmpty(f.Complemento) ? (object)DBNull.Value : f.Complemento);
                        command.Parameters.AddWithValue("@bairro", string.IsNullOrEmpty(f.Bairro) ? (object)DBNull.Value : f.Bairro);
                        command.Parameters.AddWithValue("@cep", string.IsNullOrEmpty(f.Cep) ? (object)DBNull.Value : f.Cep);
                        command.Parameters.AddWithValue("@id_cidade", f.IdCidade);
                        command.Parameters.AddWithValue("@data_contratado", f.DataContratado.HasValue ? (object)f.DataContratado.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@data_demitido", f.DataDemitido.HasValue ? (object)f.DataDemitido.Value : DBNull.Value);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao inserir funcionário: " + ex.Message);
                return false;
            }
        }

        // 3. BUSCAR POR ID
        public FuncionarioModel BuscarPorId(int id)
        {
            FuncionarioModel f = null;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM funcionarios WHERE id_funcionario = @id";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                f = new FuncionarioModel
                                {
                                    IdFuncionario = Convert.ToInt32(reader["id_funcionario"]),
                                    Funcionario = reader["funcionario"].ToString(),
                                    Apelido = reader["apelido"].ToString(),
                                    Cpf = reader["cpf"].ToString(),
                                    Rg = reader["rg"].ToString(),
                                    DataNascimento = reader["data_nascimento"] != DBNull.Value ? Convert.ToDateTime(reader["data_nascimento"]) : (DateTime?)null,
                                    Email = reader["email"].ToString(),
                                    Telefone = reader["telefone"].ToString(),
                                    IdCargo = Convert.ToInt32(reader["id_cargo"]),
                                    Salario = reader["salario"] != DBNull.Value ? Convert.ToDecimal(reader["salario"]) : (decimal?)null,
                                    Logradouro = reader["logradouro"].ToString(),
                                    Numero = reader["numero"].ToString(),
                                    Complemento = reader["complemento"].ToString(),
                                    Bairro = reader["bairro"].ToString(),
                                    Cep = reader["cep"].ToString(),
                                    IdCidade = Convert.ToInt32(reader["id_cidade"]),
                                    DataContratado = reader["data_contratado"] != DBNull.Value ? Convert.ToDateTime(reader["data_contratado"]) : (DateTime?)null,
                                    DataDemitido = reader["data_demitido"] != DBNull.Value ? Convert.ToDateTime(reader["data_demitido"]) : (DateTime?)null
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao buscar funcionário por ID: " + ex.Message);
            }
            return f;
        }

        // 4. ATUALIZAR
        public bool Modificar(FuncionarioModel f)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = @"UPDATE funcionarios SET 
                        funcionario = @funcionario, apelido = @apelido, cpf = @cpf, rg = @rg, 
                        data_nascimento = @data_nascimento, email = @email, telefone = @telefone, 
                        id_cargo = @id_cargo, salario = @salario, logradouro = @logradouro, numero = @numero, 
                        complemento = @complemento, bairro = @bairro, cep = @cep, id_cidade = @id_cidade, 
                        data_contratado = @data_contratado, data_demitido = @data_demitido,
                        data_alteracao = NOW() 
                        WHERE id_funcionario = @id";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", f.IdFuncionario);
                        command.Parameters.AddWithValue("@funcionario", f.Funcionario);
                        command.Parameters.AddWithValue("@apelido", string.IsNullOrEmpty(f.Apelido) ? (object)DBNull.Value : f.Apelido);
                        command.Parameters.AddWithValue("@cpf", f.Cpf);
                        command.Parameters.AddWithValue("@rg", string.IsNullOrEmpty(f.Rg) ? (object)DBNull.Value : f.Rg);
                        command.Parameters.AddWithValue("@data_nascimento", f.DataNascimento.HasValue ? (object)f.DataNascimento.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@email", string.IsNullOrEmpty(f.Email) ? (object)DBNull.Value : f.Email);
                        command.Parameters.AddWithValue("@telefone", string.IsNullOrEmpty(f.Telefone) ? (object)DBNull.Value : f.Telefone);
                        command.Parameters.AddWithValue("@id_cargo", f.IdCargo);
                        command.Parameters.AddWithValue("@salario", f.Salario.HasValue ? (object)f.Salario.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@logradouro", string.IsNullOrEmpty(f.Logradouro) ? (object)DBNull.Value : f.Logradouro);
                        command.Parameters.AddWithValue("@numero", string.IsNullOrEmpty(f.Numero) ? (object)DBNull.Value : f.Numero);
                        command.Parameters.AddWithValue("@complemento", string.IsNullOrEmpty(f.Complemento) ? (object)DBNull.Value : f.Complemento);
                        command.Parameters.AddWithValue("@bairro", string.IsNullOrEmpty(f.Bairro) ? (object)DBNull.Value : f.Bairro);
                        command.Parameters.AddWithValue("@cep", string.IsNullOrEmpty(f.Cep) ? (object)DBNull.Value : f.Cep);
                        command.Parameters.AddWithValue("@id_cidade", f.IdCidade);
                        command.Parameters.AddWithValue("@data_contratado", f.DataContratado.HasValue ? (object)f.DataContratado.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@data_demitido", f.DataDemitido.HasValue ? (object)f.DataDemitido.Value : DBNull.Value);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar funcionário: " + ex.Message);
                return false;
            }
        }

        // 5. EXCLUIR
        public bool Excluir(int id)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "DELETE FROM funcionarios WHERE id_funcionario = @id";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao excluir funcionário: " + ex.Message);
                return false;
            }
        }
    }
}