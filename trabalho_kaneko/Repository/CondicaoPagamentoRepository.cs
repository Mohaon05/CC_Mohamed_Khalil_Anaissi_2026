using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using trabalho_kaneko.Data;
using trabalho_kaneko.Models;

namespace trabalho_kaneko.Repository
{
    public class CondicaoPagamentoRepository
    {
        private readonly DbContext _context;

        public CondicaoPagamentoRepository(DbContext context)
        {
            _context = context;
        }

        public List<CondicaoPagamentoModel> ListarTodos()
        {
            List<CondicaoPagamentoModel> condicoes = new List<CondicaoPagamentoModel>();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM condicoes_pagamento ORDER BY condicao_pagamento ASC";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            condicoes.Add(new CondicaoPagamentoModel
                            {
                                IdCondicao = Convert.ToInt32(reader["id_condicao"]),
                                CondicaoPagamento = reader["condicao_pagamento"].ToString(),
                                QuantidadeParcelas = Convert.ToInt32(reader["quantidade_parcelas"]),
                                Juros = reader["juros"] != DBNull.Value ? Convert.ToDecimal(reader["juros"]) : (decimal?)null,
                                DataInclusao = Convert.ToDateTime(reader["data_inclusao"]),
                                DataAlteracao = reader["data_alteracao"] != DBNull.Value ? Convert.ToDateTime(reader["data_alteracao"]) : (DateTime?)null
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao listar condições de pagamento: " + ex.Message);
            }
            return condicoes;
        }

        public bool Inserir(CondicaoPagamentoModel condicao)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "INSERT INTO condicoes_pagamento (condicao_pagamento, quantidade_parcelas, juros) VALUES (@condicao, @quantidade, @juros)";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@condicao", condicao.CondicaoPagamento);
                        command.Parameters.AddWithValue("@quantidade", condicao.QuantidadeParcelas);
                        command.Parameters.AddWithValue("@juros", condicao.Juros.HasValue ? (object)condicao.Juros.Value : DBNull.Value);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao inserir condição de pagamento: " + ex.Message);
                return false;
            }
        }

        public CondicaoPagamentoModel BuscarPorId(int id)
        {
            CondicaoPagamentoModel condicao = null;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM condicoes_pagamento WHERE id_condicao = @id";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                condicao = new CondicaoPagamentoModel
                                {
                                    IdCondicao = Convert.ToInt32(reader["id_condicao"]),
                                    CondicaoPagamento = reader["condicao_pagamento"].ToString(),
                                    QuantidadeParcelas = Convert.ToInt32(reader["quantidade_parcelas"]),
                                    Juros = reader["juros"] != DBNull.Value ? Convert.ToDecimal(reader["juros"]) : (decimal?)null
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao buscar condição por ID: " + ex.Message);
            }
            return condicao;
        }

        public bool Atualizar(CondicaoPagamentoModel condicao)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "UPDATE condicoes_pagamento SET condicao_pagamento = @condicao, quantidade_parcelas = @quantidade, juros = @juros, data_alteracao = NOW() WHERE id_condicao = @id";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", condicao.IdCondicao);
                        command.Parameters.AddWithValue("@condicao", condicao.CondicaoPagamento);
                        command.Parameters.AddWithValue("@quantidade", condicao.QuantidadeParcelas);
                        command.Parameters.AddWithValue("@juros", condicao.Juros.HasValue ? (object)condicao.Juros.Value : DBNull.Value);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar condição de pagamento: " + ex.Message);
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
                    string query = "DELETE FROM condicoes_pagamento WHERE id_condicao = @id";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao excluir condição de pagamento: " + ex.Message);
                return false;
            }
        }
    }
}