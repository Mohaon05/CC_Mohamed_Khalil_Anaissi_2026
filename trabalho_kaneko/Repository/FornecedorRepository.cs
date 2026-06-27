using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using trabalho_kaneko.Data;
using trabalho_kaneko.Models;

namespace trabalho_kaneko.Repository
{
    public class FornecedorRepository
    {
        private readonly DbContext _context;

        public FornecedorRepository(DbContext context)
        {
            _context = context;
        }

        // 1. LISTAR TODOS
        public List<FornecedorModel> ListarTodos()
        {
            List<FornecedorModel> fornecedores = new List<FornecedorModel>();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = @"
                        SELECT f.id_fornecedor, f.fornecedor, f.nome_fantasia, f.tipo_pessoa, 
                               f.cpf_cnpj, f.rg_inscest, f.email, f.celular, f.site, 
                               f.logradouro, f.numero, f.complemento, f.bairro, f.cep, 
                               f.id_cidade, f.data_inclusao, f.data_alteracao, 
                               c.cidade AS nome_cidade 
                        FROM fornecedores f 
                        INNER JOIN cidades c ON f.id_cidade = c.id_cidade 
                        ORDER BY f.fornecedor ASC";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                fornecedores.Add(new FornecedorModel
                                {
                                    IdFornecedor = Convert.ToInt32(reader["id_fornecedor"]),
                                    Fornecedor = reader["fornecedor"].ToString(),
                                    NomeFantasia = reader["nome_fantasia"].ToString(),
                                    TipoPessoa = reader["tipo_pessoa"].ToString(),
                                    CpfCnpj = reader["cpf_cnpj"].ToString(),
                                    RgInscest = reader["rg_inscest"].ToString(),
                                    Email = reader["email"].ToString(),
                                    Celular = reader["celular"].ToString(),
                                    Site = reader["site"].ToString(),
                                    Logradouro = reader["logradouro"].ToString(),
                                    Numero = reader["numero"].ToString(),
                                    Complemento = reader["complemento"].ToString(),
                                    Bairro = reader["bairro"].ToString(),
                                    Cep = reader["cep"].ToString(),
                                    IdCidade = Convert.ToInt32(reader["id_cidade"]),
                                    NomeCidade = reader["nome_cidade"].ToString(),
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
                Console.WriteLine("Erro ao listar fornecedores: " + ex.Message);
            }
            return fornecedores;
        }

        // 2. INSERIR
        public bool Inserir(FornecedorModel fornecedor)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = @"INSERT INTO fornecedores 
                                     (fornecedor, nome_fantasia, tipo_pessoa, cpf_cnpj, rg_inscest, email, celular, site, logradouro, numero, complemento, bairro, cep, id_cidade) 
                                     VALUES 
                                     (@fornecedor, @nome_fantasia, @tipo_pessoa, @cpf_cnpj, @rg_inscest, @email, @celular, @site, @logradouro, @numero, @complemento, @bairro, @cep, @id_cidade)";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@fornecedor", fornecedor.Fornecedor);
                        command.Parameters.AddWithValue("@nome_fantasia", string.IsNullOrEmpty(fornecedor.NomeFantasia) ? (object)DBNull.Value : fornecedor.NomeFantasia);
                        command.Parameters.AddWithValue("@tipo_pessoa", fornecedor.TipoPessoa);
                        command.Parameters.AddWithValue("@cpf_cnpj", fornecedor.CpfCnpj);
                        command.Parameters.AddWithValue("@rg_inscest", string.IsNullOrEmpty(fornecedor.RgInscest) ? (object)DBNull.Value : fornecedor.RgInscest);
                        command.Parameters.AddWithValue("@email", string.IsNullOrEmpty(fornecedor.Email) ? (object)DBNull.Value : fornecedor.Email);
                        command.Parameters.AddWithValue("@celular", string.IsNullOrEmpty(fornecedor.Celular) ? (object)DBNull.Value : fornecedor.Celular);
                        command.Parameters.AddWithValue("@site", string.IsNullOrEmpty(fornecedor.Site) ? (object)DBNull.Value : fornecedor.Site);
                        command.Parameters.AddWithValue("@logradouro", string.IsNullOrEmpty(fornecedor.Logradouro) ? (object)DBNull.Value : fornecedor.Logradouro);
                        command.Parameters.AddWithValue("@numero", string.IsNullOrEmpty(fornecedor.Numero) ? (object)DBNull.Value : fornecedor.Numero);
                        command.Parameters.AddWithValue("@complemento", string.IsNullOrEmpty(fornecedor.Complemento) ? (object)DBNull.Value : fornecedor.Complemento);
                        command.Parameters.AddWithValue("@bairro", string.IsNullOrEmpty(fornecedor.Bairro) ? (object)DBNull.Value : fornecedor.Bairro);
                        command.Parameters.AddWithValue("@cep", string.IsNullOrEmpty(fornecedor.Cep) ? (object)DBNull.Value : fornecedor.Cep);
                        command.Parameters.AddWithValue("@id_cidade", fornecedor.IdCidade);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao inserir fornecedor: " + ex.Message);
                return false;
            }
        }

        // 3. BUSCAR POR ID
        public FornecedorModel BuscarPorId(int id)
        {
            FornecedorModel fornecedor = null;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM fornecedores WHERE id_fornecedor = @id";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                fornecedor = new FornecedorModel
                                {
                                    IdFornecedor = Convert.ToInt32(reader["id_fornecedor"]),
                                    Fornecedor = reader["fornecedor"].ToString(),
                                    NomeFantasia = reader["nome_fantasia"].ToString(),
                                    TipoPessoa = reader["tipo_pessoa"].ToString(),
                                    CpfCnpj = reader["cpf_cnpj"].ToString(),
                                    RgInscest = reader["rg_inscest"].ToString(),
                                    Email = reader["email"].ToString(),
                                    Celular = reader["celular"].ToString(),
                                    Site = reader["site"].ToString(),
                                    Logradouro = reader["logradouro"].ToString(),
                                    Numero = reader["numero"].ToString(),
                                    Complemento = reader["complemento"].ToString(),
                                    Bairro = reader["bairro"].ToString(),
                                    Cep = reader["cep"].ToString(),
                                    IdCidade = Convert.ToInt32(reader["id_cidade"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao buscar fornecedor por ID: " + ex.Message);
            }
            return fornecedor;
        }

        // 4. ATUALIZAR
        public bool Atualizar(FornecedorModel fornecedor)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = @"UPDATE fornecedores SET 
                                        fornecedor = @fornecedor, nome_fantasia = @nome_fantasia, tipo_pessoa = @tipo_pessoa, 
                                        cpf_cnpj = @cpf_cnpj, rg_inscest = @rg_inscest, email = @email, celular = @celular, 
                                        site = @site, logradouro = @logradouro, numero = @numero, 
                                        complemento = @complemento, bairro = @bairro, cep = @cep, id_cidade = @id_cidade, 
                                        data_alteracao = NOW() 
                                     WHERE id_fornecedor = @id";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", fornecedor.IdFornecedor);
                        command.Parameters.AddWithValue("@fornecedor", fornecedor.Fornecedor);
                        command.Parameters.AddWithValue("@nome_fantasia", string.IsNullOrEmpty(fornecedor.NomeFantasia) ? (object)DBNull.Value : fornecedor.NomeFantasia);
                        command.Parameters.AddWithValue("@tipo_pessoa", fornecedor.TipoPessoa);
                        command.Parameters.AddWithValue("@cpf_cnpj", fornecedor.CpfCnpj);
                        command.Parameters.AddWithValue("@rg_inscest", string.IsNullOrEmpty(fornecedor.RgInscest) ? (object)DBNull.Value : fornecedor.RgInscest);
                        command.Parameters.AddWithValue("@email", string.IsNullOrEmpty(fornecedor.Email) ? (object)DBNull.Value : fornecedor.Email);
                        command.Parameters.AddWithValue("@celular", string.IsNullOrEmpty(fornecedor.Celular) ? (object)DBNull.Value : fornecedor.Celular);
                        command.Parameters.AddWithValue("@site", string.IsNullOrEmpty(fornecedor.Site) ? (object)DBNull.Value : fornecedor.Site);
                        command.Parameters.AddWithValue("@logradouro", string.IsNullOrEmpty(fornecedor.Logradouro) ? (object)DBNull.Value : fornecedor.Logradouro);
                        command.Parameters.AddWithValue("@numero", string.IsNullOrEmpty(fornecedor.Numero) ? (object)DBNull.Value : fornecedor.Numero);
                        command.Parameters.AddWithValue("@complemento", string.IsNullOrEmpty(fornecedor.Complemento) ? (object)DBNull.Value : fornecedor.Complemento);
                        command.Parameters.AddWithValue("@bairro", string.IsNullOrEmpty(fornecedor.Bairro) ? (object)DBNull.Value : fornecedor.Bairro);
                        command.Parameters.AddWithValue("@cep", string.IsNullOrEmpty(fornecedor.Cep) ? (object)DBNull.Value : fornecedor.Cep);
                        command.Parameters.AddWithValue("@id_cidade", fornecedor.IdCidade);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar fornecedor: " + ex.Message);
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
                    string query = "DELETE FROM fornecedores WHERE id_fornecedor = @id";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao excluir fornecedor: " + ex.Message);
                return false;
            }
        }
    }
}