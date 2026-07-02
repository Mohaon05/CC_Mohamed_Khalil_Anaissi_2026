using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using trabalho_kaneko.Data;
using trabalho_kaneko.Models;

namespace trabalho_kaneko.Repository
{
    public class EstadoRepository
    {
        private readonly DbContext _context;

        public EstadoRepository(DbContext context)
        {
            _context = context;
        }

        public bool Inserir(EstadoModel estado)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();

                    // NOVO: A query agora inclui o id_pais, que é obrigatório na sua tabela
                    string query = @"INSERT INTO estados (estado, uf, id_pais) VALUES (@estado, @uf, @id_pais)";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@estado", estado.Estado);
                        command.Parameters.AddWithValue("@uf", estado.Uf.ToUpper());
                        command.Parameters.AddWithValue("@id_pais", estado.IdPais);

                        int linhasAfetadas = command.ExecuteNonQuery();
                        return linhasAfetadas > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao inserir estado: " + ex.Message);
                return false;
            }
        }

        public List<EstadoModel> ListarTodos()
        {
            List<EstadoModel> estados = new List<EstadoModel>();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();

                    string query = @"
                    SELECT e.id_estado, e.estado, e.uf, e.id_pais, e.data_inclusao, e.data_alteracao, p.pais AS nome_pais 
                    FROM estados e 
                    INNER JOIN paises p ON e.id_pais = p.id_pais 
                    ORDER BY e.estado ASC";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                estados.Add(new EstadoModel
                                {
                                    IdEstado = Convert.ToInt32(reader["id_estado"]),
                                    Estado = reader["estado"].ToString(),
                                    Uf = reader["uf"].ToString(),
                                    IdPais = Convert.ToInt32(reader["id_pais"]),
                                    DataInclusao = Convert.ToDateTime(reader["data_inclusao"]),
                                    DataAlteracao = reader["data_alteracao"] != DBNull.Value ? Convert.ToDateTime(reader["data_alteracao"]) : (DateTime?)null,
                                    NomePais = reader["nome_pais"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao listar estados: " + ex.Message);
            }
            return estados;
        }

        public EstadoModel BuscarPorId(int id)
        {
            EstadoModel estado = null;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    // O SELECT puxa os dados básicos do estado para preencher o formulário
                    string query = "SELECT id_estado, estado, uf, id_pais, data_inclusao, data_alteracao FROM estados WHERE id_estado = @id";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                estado = new EstadoModel
                                {
                                    IdEstado = Convert.ToInt32(reader["id_estado"]),
                                    Estado = reader["estado"].ToString(),
                                    Uf = reader["uf"].ToString(),
                                    IdPais = Convert.ToInt32(reader["id_pais"]),
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
                Console.WriteLine("Erro ao buscar estado por ID: " + ex.Message);
            }
            return estado;
        }

        public bool Atualizar(EstadoModel estado)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();

                    // Faz o UPDATE dos três campos que vêm da tela (nome, uf e a chave do país)
                    string query = @"UPDATE estados SET estado = @estado, uf = @uf, id_pais = @id_pais WHERE id_estado = @id";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@estado", estado.Estado);
                        command.Parameters.AddWithValue("@uf", estado.Uf.ToUpper());
                        command.Parameters.AddWithValue("@id_pais", estado.IdPais);
                        command.Parameters.AddWithValue("@id", estado.IdEstado);

                        int linhasAfetadas = command.ExecuteNonQuery();
                        return linhasAfetadas > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar estado: " + ex.Message);
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
                    string query = "DELETE FROM estados WHERE id_estado = @id";

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
                Console.WriteLine("Erro ao excluir estado: " + ex.Message);
                return false;
            }
        }

        public bool ExisteEstadoNoPais(string nomeEstado, string uf, int idPais, int idEstadoAtual = 0)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    // Verifica se existe o MESMO nome ou a MESMA sigla (UF) no MESMO país.
                    // O "id_estado != @idAtual" serve para não dar erro quando formos Editar o próprio estado.
                    string query = @"
                    SELECT COUNT(1) 
                    FROM estados 
                    WHERE (estado = @nomeEstado OR uf = @uf) 
                      AND id_pais = @idPais 
                      AND id_estado != @idAtual";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@nomeEstado", nomeEstado);
                        command.Parameters.AddWithValue("@uf", uf);
                        command.Parameters.AddWithValue("@idPais", idPais);
                        command.Parameters.AddWithValue("@idAtual", idEstadoAtual);

                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0; // Retorna true se encontrou alguma duplicata
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao verificar duplicidade de estado: " + ex.Message);
                return false;
            }
        }

    }
}

