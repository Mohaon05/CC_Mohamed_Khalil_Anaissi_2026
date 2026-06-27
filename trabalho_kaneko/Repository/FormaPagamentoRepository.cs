using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using trabalho_kaneko.Data;
using trabalho_kaneko.Models;

namespace trabajo_kaneko.Repository
{
    public class FormaPagamentoRepository
    {
        private readonly DbContext _context;

        public FormaPagamentoRepository(DbContext context)
        {
            _context = context;
        }

        public List<FormaPagamentoModel> ListarTodos()
        {
            List<FormaPagamentoModel> formas = new List<FormaPagamentoModel>();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM formas_pagamento ORDER BY forma_pagamento ASC";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            formas.Add(new FormaPagamentoModel
                            {
                                IdFormaPagamento = Convert.ToInt32(reader["id_forma_pagamento"]),
                                FormaPagamento = reader["forma_pagamento"].ToString(),
                                Ativo = Convert.ToBoolean(reader["ativo"]),
                                DataInclusao = Convert.ToDateTime(reader["data_inclusao"]),
                                DataAlteracao = reader["data_alteracao"] != DBNull.Value ? Convert.ToDateTime(reader["data_alteracao"]) : (DateTime?)null
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao listar formas de pagamento: " + ex.Message);
            }
            return formas;
        }

        public bool Inserir(FormaPagamentoModel forma)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "INSERT INTO formas_pagamento (forma_pagamento, ativo) VALUES (@forma, @ativo)";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@forma", forma.FormaPagamento);
                        command.Parameters.AddWithValue("@ativo", forma.Ativo);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao inserir forma de pagamento: " + ex.Message);
                return false;
            }
        }

        public FormaPagamentoModel BuscarPorId(int id)
        {
            FormaPagamentoModel forma = null;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM formas_pagamento WHERE id_forma_pagamento = @id";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                forma = new FormaPagamentoModel
                                {
                                    IdFormaPagamento = Convert.ToInt32(reader["id_forma_pagamento"]),
                                    FormaPagamento = reader["forma_pagamento"].ToString(),
                                    Ativo = Convert.ToBoolean(reader["ativo"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao buscar forma de pagamento por ID: " + ex.Message);
            }
            return forma;
        }

        public bool Atualizar(FormaPagamentoModel forma)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "UPDATE formas_pagamento SET forma_pagamento = @forma, ativo = @ativo, data_alteracao = NOW() WHERE id_forma_pagamento = @id";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", forma.IdFormaPagamento);
                        command.Parameters.AddWithValue("@forma", forma.FormaPagamento);
                        command.Parameters.AddWithValue("@ativo", forma.Ativo);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar forma de pagamento: " + ex.Message);
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
                    string query = "DELETE FROM formas_pagamento WHERE id_forma_pagamento = @id";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao excluir forma de pagamento: " + ex.Message);
                return false;
            }
        }
    }
}