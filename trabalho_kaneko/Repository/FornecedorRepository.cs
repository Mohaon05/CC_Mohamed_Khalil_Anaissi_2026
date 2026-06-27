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

        public List<FornecedorModel> ListarTodos()
        {
            List<FornecedorModel> fornecedores = new List<FornecedorModel>();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    // INNER JOIN para pegar o nome da cidade
                    string query = @"
                        SELECT f.*, c.cidade AS nome_cidade 
                        FROM fornecedores f 
                        INNER JOIN cidades c ON f.id_cidade = c.id_cidade 
                        ORDER BY f.fornecedor ASC";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
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
                                DataInclusao = Convert.ToDateTime(reader["data_inclusao"])
                            });
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

        public bool Inserir(FornecedorModel f)
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
                        command.Parameters.AddWithValue("@fornecedor", f.Fornecedor);
                        command.Parameters.AddWithValue("@nome_fantasia", string.IsNullOrEmpty(f.NomeFantasia) ? (object)DBNull.Value : f.NomeFantasia);
                        command.Parameters.AddWithValue("@tipo_pessoa", f.TipoPessoa);
                        command.Parameters.AddWithValue("@cpf_cnpj", f.CpfCnpj);
                        command.Parameters.AddWithValue("@rg_inscest", string.IsNullOrEmpty(f.RgInscest) ? (object)DBNull.Value : f.RgInscest);
                        command.Parameters.AddWithValue("@email", string.IsNullOrEmpty(f.Email) ? (object)DBNull.Value : f.Email);
                        command.Parameters.AddWithValue("@celular", string.IsNullOrEmpty(f.Celular) ? (object)DBNull.Value : f.Celular);
                        command.Parameters.AddWithValue("@site", string.IsNullOrEmpty(f.Site) ? (object)DBNull.Value : f.Site);
                        command.Parameters.AddWithValue("@logradouro", string.IsNullOrEmpty(f.Logradouro) ? (object)DBNull.Value : f.Logradouro);
                        command.Parameters.AddWithValue("@numero", string.IsNullOrEmpty(f.Numero) ? (object)DBNull.Value : f.Numero);
                        command.Parameters.AddWithValue("@complemento", string.IsNullOrEmpty(f.Complemento) ? (object)DBNull.Value : f.Complemento);
                        command.Parameters.AddWithValue("@bairro", string.IsNullOrEmpty(f.Bairro) ? (object)DBNull.Value : f.Bairro);
                        command.Parameters.AddWithValue("@cep", string.IsNullOrEmpty(f.Cep) ? (object)DBNull.Value : f.Cep);
                        command.Parameters.AddWithValue("@id_cidade", f.IdCidade);

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

        public FornecedorModel BuscarPorId(int id)
        {
            FornecedorModel f = null;
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
                                f = new FornecedorModel
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
                Console.WriteLine("Erro ao buscar fornecedor: " + ex.Message);
            }
            return f;
        }

        public bool Atualizar(FornecedorModel f)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = @"UPDATE fornecedores SET 
                        fornecedor = @fornecedor, nome_fantasia = @nome_fantasia, tipo_pessoa = @tipo_pessoa, 
                        cpf_cnpj = @cpf_cnpj, rg_inscest = @rg_inscest, email = @email, celular = @celular, 
                        site = @site, logradouro = @logradouro, numero = @numero, complemento = @complemento, 
                        bairro = @bairro, cep = @cep, id_cidade = @id_cidade, data_alteracao = NOW() 
                        WHERE id_fornecedor = @id";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", f.IdFornecedor);
                        command.Parameters.AddWithValue("@fornecedor", f.Fornecedor);
                        command.Parameters.AddWithValue("@nome_fantasia", string.IsNullOrEmpty(f.NomeFantasia) ? (object)DBNull.Value : f.NomeFantasia);
                        command.Parameters.AddWithValue("@tipo_pessoa", f.TipoPessoa);
                        command.Parameters.AddWithValue("@cpf_cnpj", f.CpfCnpj);
                        command.Parameters.AddWithValue("@rg_inscest", string.IsNullOrEmpty(f.RgInscest) ? (object)DBNull.Value : f.RgInscest);
                        command.Parameters.AddWithValue("@email", string.IsNullOrEmpty(f.Email) ? (object)DBNull.Value : f.Email);
                        command.Parameters.AddWithValue("@celular", string.IsNullOrEmpty(f.Celular) ? (object)DBNull.Value : f.Celular);
                        command.Parameters.AddWithValue("@site", string.IsNullOrEmpty(f.Site) ? (object)DBNull.Value : f.Site);
                        command.Parameters.AddWithValue("@logradouro", string.IsNullOrEmpty(f.Logradouro) ? (object)DBNull.Value : f.Logradouro);
                        command.Parameters.AddWithValue("@numero", string.IsNullOrEmpty(f.Numero) ? (object)DBNull.Value : f.Numero);
                        command.Parameters.AddWithValue("@complemento", string.IsNullOrEmpty(f.Complemento) ? (object)DBNull.Value : f.Complemento);
                        command.Parameters.AddWithValue("@bairro", string.IsNullOrEmpty(f.Bairro) ? (object)DBNull.Value : f.Bairro);
                        command.Parameters.AddWithValue("@cep", string.IsNullOrEmpty(f.Cep) ? (object)DBNull.Value : f.Cep);
                        command.Parameters.AddWithValue("@id_cidade", f.IdCidade);

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