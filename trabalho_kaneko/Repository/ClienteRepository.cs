using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using trabalho_kaneko.Data;
using trabalho_kaneko.Models;

namespace trabalho_kaneko.Repository
{
    public class ClienteRepository
    {
        private readonly DbContext _context;

        public ClienteRepository(DbContext context)
        {
            _context = context;
        }

        public List<ClienteModel> ListarTodos()
        {
            List<ClienteModel> clientes = new List<ClienteModel>();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();

                    // Fazemos o JOIN apenas com a tabela de cidades
                    string query = @"
                        SELECT c.id_cliente, c.cliente, c.tipo_pessoa, c.cpf_cnpj, c.rg_inscest, 
                               c.data_nascimento, c.logradouro, c.numero, c.complemento, c.bairro, 
                               c.cep, c.email, c.id_cidade, c.data_inclusao, c.data_alteracao, 
                               cid.cidade AS nome_cidade 
                        FROM clientes c 
                        INNER JOIN cidades cid ON c.id_cidade = cid.id_cidade 
                        ORDER BY c.cliente ASC";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                clientes.Add(new ClienteModel
                                {
                                    IdCliente = Convert.ToInt32(reader["id_cliente"]),
                                    Cliente = reader["cliente"].ToString(),
                                    TipoPessoa = reader["tipo_pessoa"].ToString(),
                                    CpfCnpj = reader["cpf_cnpj"].ToString(),
                                    RgInscest = reader["rg_inscest"].ToString(),
                                    DataNascimento = reader["data_nascimento"] != DBNull.Value ? Convert.ToDateTime(reader["data_nascimento"]) : (DateTime?)null,
                                    Logradouro = reader["logradouro"].ToString(),
                                    Numero = reader["numero"].ToString(),
                                    Complemento = reader["complemento"].ToString(),
                                    Bairro = reader["bairro"].ToString(),
                                    Cep = reader["cep"].ToString(),
                                    Email = reader["email"].ToString(),
                                    IdCidade = Convert.ToInt32(reader["id_cidade"]),
                                    NomeCidade = reader["nome_cidade"].ToString(), // Veio do JOIN
                                    DataInclusao = Convert.ToDateTime(reader["data_inclusao"]),
                                    DataAlteracao = reader["data_alteracao"] != DBNull.Value ? Convert.ToDateTime(reader["data_alteracao"]) : (DateTime?)null
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao listar clientes: " + ex.Message);
            }
            return clientes;
        }

        // 1. INSERIR
        public bool Inserir(ClienteModel cliente)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = @"INSERT INTO clientes 
                                     (cliente, tipo_pessoa, cpf_cnpj, rg_inscest, data_nascimento, logradouro, numero, complemento, bairro, cep, id_cidade, email) 
                                     VALUES 
                                     (@cliente, @tipo_pessoa, @cpf_cnpj, @rg_inscest, @data_nascimento, @logradouro, @numero, @complemento, @bairro, @cep, @id_cidade, @email)";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@cliente", cliente.Cliente);
                        command.Parameters.AddWithValue("@tipo_pessoa", cliente.TipoPessoa);
                        command.Parameters.AddWithValue("@cpf_cnpj", cliente.CpfCnpj);
                        command.Parameters.AddWithValue("@rg_inscest", string.IsNullOrEmpty(cliente.RgInscest) ? (object)DBNull.Value : cliente.RgInscest);
                        command.Parameters.AddWithValue("@data_nascimento", cliente.DataNascimento.HasValue ? cliente.DataNascimento.Value.Date : (object)DBNull.Value);
                        command.Parameters.AddWithValue("@logradouro", string.IsNullOrEmpty(cliente.Logradouro) ? (object)DBNull.Value : cliente.Logradouro);
                        command.Parameters.AddWithValue("@numero", string.IsNullOrEmpty(cliente.Numero) ? (object)DBNull.Value : cliente.Numero);
                        command.Parameters.AddWithValue("@complemento", string.IsNullOrEmpty(cliente.Complemento) ? (object)DBNull.Value : cliente.Complemento);
                        command.Parameters.AddWithValue("@bairro", string.IsNullOrEmpty(cliente.Bairro) ? (object)DBNull.Value : cliente.Bairro);
                        command.Parameters.AddWithValue("@cep", string.IsNullOrEmpty(cliente.Cep) ? (object)DBNull.Value : cliente.Cep);
                        command.Parameters.AddWithValue("@id_cidade", cliente.IdCidade);
                        command.Parameters.AddWithValue("@email", string.IsNullOrEmpty(cliente.Email) ? (object)DBNull.Value : cliente.Email);

                        int linhasAfetadas = command.ExecuteNonQuery();
                        return linhasAfetadas > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao inserir cliente: " + ex.Message);
                return false;
            }
        }

        // 2. BUSCAR POR ID (Para a tela de Edição)
        public ClienteModel BuscarPorId(int id)
        {
            ClienteModel cliente = null;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = @"SELECT id_cliente, cliente, tipo_pessoa, cpf_cnpj, rg_inscest, data_nascimento, 
                                            logradouro, numero, complemento, bairro, cep, email, id_cidade, 
                                            data_inclusao, data_alteracao 
                                     FROM clientes WHERE id_cliente = @id";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cliente = new ClienteModel
                                {
                                    IdCliente = Convert.ToInt32(reader["id_cliente"]),
                                    Cliente = reader["cliente"].ToString(),
                                    TipoPessoa = reader["tipo_pessoa"].ToString(),
                                    CpfCnpj = reader["cpf_cnpj"].ToString(),
                                    RgInscest = reader["rg_inscest"].ToString(),
                                    DataNascimento = reader["data_nascimento"] != DBNull.Value ? Convert.ToDateTime(reader["data_nascimento"]) : (DateTime?)null,
                                    Logradouro = reader["logradouro"].ToString(),
                                    Numero = reader["numero"].ToString(),
                                    Complemento = reader["complemento"].ToString(),
                                    Bairro = reader["bairro"].ToString(),
                                    Cep = reader["cep"].ToString(),
                                    Email = reader["email"].ToString(),
                                    IdCidade = Convert.ToInt32(reader["id_cidade"]),
                                    DataInclusao = Convert.ToDateTime(reader["data_inclusao"]),
                                    DataAlteracao = reader["data_alteracao"] != DBNull.Value ? Convert.ToDateTime(reader["data_alteracao"]) : (DateTime?)null
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao buscar cliente por ID: " + ex.Message);
            }
            return cliente;
        }

        // 3. ATUALIZAR
        public bool Atualizar(ClienteModel cliente)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    // Adicionamos o data_alteracao = NOW() por segurança, igual fizemos nas cidades
                    string query = @"UPDATE clientes SET 
                                        cliente = @cliente, cpf_cnpj = @cpf_cnpj, 
                                        rg_inscest = @rg_inscest, data_nascimento = @data_nascimento, logradouro = @logradouro, 
                                        numero = @numero, complemento = @complemento, bairro = @bairro, cep = @cep, 
                                        id_cidade = @id_cidade, email = @email, data_alteracao = NOW() 
                                     WHERE id_cliente = @id";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", cliente.IdCliente);
                        command.Parameters.AddWithValue("@cliente", cliente.Cliente);
                        //tirado para não poder atualizar o tipo de pessoa
                        //command.Parameters.AddWithValue("@tipo_pessoa", cliente.TipoPessoa);
                        command.Parameters.AddWithValue("@cpf_cnpj", cliente.CpfCnpj);
                        command.Parameters.AddWithValue("@rg_inscest", string.IsNullOrEmpty(cliente.RgInscest) ? (object)DBNull.Value : cliente.RgInscest);
                        command.Parameters.AddWithValue("@data_nascimento", cliente.DataNascimento.HasValue ? cliente.DataNascimento.Value.Date : (object)DBNull.Value);
                        command.Parameters.AddWithValue("@logradouro", string.IsNullOrEmpty(cliente.Logradouro) ? (object)DBNull.Value : cliente.Logradouro);
                        command.Parameters.AddWithValue("@numero", string.IsNullOrEmpty(cliente.Numero) ? (object)DBNull.Value : cliente.Numero);
                        command.Parameters.AddWithValue("@complemento", string.IsNullOrEmpty(cliente.Complemento) ? (object)DBNull.Value : cliente.Complemento);
                        command.Parameters.AddWithValue("@bairro", string.IsNullOrEmpty(cliente.Bairro) ? (object)DBNull.Value : cliente.Bairro);
                        command.Parameters.AddWithValue("@cep", string.IsNullOrEmpty(cliente.Cep) ? (object)DBNull.Value : cliente.Cep);
                        command.Parameters.AddWithValue("@id_cidade", cliente.IdCidade);
                        command.Parameters.AddWithValue("@email", string.IsNullOrEmpty(cliente.Email) ? (object)DBNull.Value : cliente.Email);

                        int linhasAfetadas = command.ExecuteNonQuery();
                        return linhasAfetadas > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar cliente: " + ex.Message);
                return false;
            }
        }

        // 4. EXCLUIR
        public bool Excluir(int id)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "DELETE FROM clientes WHERE id_cliente = @id";
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
                Console.WriteLine("Erro ao excluir cliente: " + ex.Message);
                return false;
            }
        }
    }
}