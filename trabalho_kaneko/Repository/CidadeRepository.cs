using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using trabalho_kaneko.Data;
using trabalho_kaneko.Models;

namespace trabalho_kaneko.Repository
{
    public class CidadeRepository
    {
        private readonly DbContext _context;

        public CidadeRepository(DbContext context)
        {
            _context = context;
        }

        public bool Inserir(CidadeModel cidade)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();

                    // NOVO: A query de inserção recebe o nome da cidade e o ID do estado escolhido
                    string query = @"INSERT INTO cidades (cidade, id_estado) VALUES (@cidade, @id_estado)";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@cidade", cidade.Cidade);
                        command.Parameters.AddWithValue("@id_estado", cidade.IdEstado);

                        int linhasAfetadas = command.ExecuteNonQuery();
                        return linhasAfetadas > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao inserir cidade: " + ex.Message);
                return false;
            }
        }

        public List<CidadeModel> ListarTodos()
        {
            List<CidadeModel> cidades = new List<CidadeModel>();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();

                    // NOVO: INNER JOIN com a tabela 'estados' para puxar o nome do estado em vez de só mostrar o número
                    string query = @"
                    SELECT c.id_cidade, c.cidade, c.id_estado, c.data_inclusao, c.data_alteracao, e.estado AS nome_estado 
                    FROM cidades c 
                    INNER JOIN estados e ON c.id_estado = e.id_estado 
                    ORDER BY c.cidade ASC";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cidades.Add(new CidadeModel
                                {
                                    IdCidade = Convert.ToInt32(reader["id_cidade"]),
                                    Cidade = reader["cidade"].ToString(),
                                    IdEstado = Convert.ToInt32(reader["id_estado"]),
                                    DataInclusao = Convert.ToDateTime(reader["data_inclusao"]),
                                    DataAlteracao = reader["data_alteracao"] != DBNull.Value ? Convert.ToDateTime(reader["data_alteracao"]) : (DateTime?)null,
                                    NomeEstado = reader["nome_estado"].ToString() // Puxa o nome vindo do JOIN
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao listar cidades: " + ex.Message);
            }
            return cidades;
        }

        public CidadeModel BuscarPorId(int id)
        {
            CidadeModel cidade = null;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "SELECT id_cidade, cidade, id_estado, data_inclusao, data_alteracao FROM cidades WHERE id_cidade = @id";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cidade = new CidadeModel
                                {
                                    IdCidade = Convert.ToInt32(reader["id_cidade"]),
                                    Cidade = reader["cidade"].ToString(),
                                    IdEstado = Convert.ToInt32(reader["id_estado"]),
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
                Console.WriteLine("Erro ao buscar cidade por ID: " + ex.Message);
            }
            return cidade;
        }

        public bool Atualizar(CidadeModel cidade)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = @"UPDATE cidades SET cidade = @cidade, id_estado = @id_estado WHERE id_cidade = @id";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@cidade", cidade.Cidade);
                        command.Parameters.AddWithValue("@id_estado", cidade.IdEstado);
                        command.Parameters.AddWithValue("@id", cidade.IdCidade);

                        int linhasAfetadas = command.ExecuteNonQuery();
                        return linhasAfetadas > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar cidade: " + ex.Message);
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
                    string query = "DELETE FROM cidades WHERE id_cidade = @id";

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
                Console.WriteLine("Erro ao excluir cidade: " + ex.Message);
                return false;
            }
        }

        public int InserirRetornandoId(CidadeModel cidade)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();

                    // Atenção: Confirme se os nomes das colunas (cidade, id_estado) estão iguais aos do seu banco de dados
                    string query = @"
                        INSERT INTO cidades (cidade, id_estado, data_inclusao) 
                        VALUES (@cidade, @id_estado, NOW());
                        
                        SELECT LAST_INSERT_ID();";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@cidade", cidade.Cidade);
                        command.Parameters.AddWithValue("@id_estado", cidade.IdEstado);

                        // O ExecuteScalar executa o INSERT e já puxa o SELECT LAST_INSERT_ID() que colocamos ali em cima
                        int idGerado = Convert.ToInt32(command.ExecuteScalar());
                        return idGerado;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao inserir cidade retornando ID: " + ex.Message);
                return 0; // Retorna 0 em caso de erro para o frontend saber que falhou
            }
        }

    }
}