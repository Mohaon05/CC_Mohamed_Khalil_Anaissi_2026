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
                        SELECT c.id_cidade, c.cidade, c.id_estado, c.data_inclusao, e.estado AS nome_estado 
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
    }
}