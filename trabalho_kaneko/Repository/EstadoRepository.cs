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

                    // NOVO: Usamos INNER JOIN para juntar as tabelas "estados" e "paises". 
                    // Assim conseguimos puxar o p.pais (nome do país) direto do banco.
                    string query = @"
                        SELECT e.id_estado, e.estado, e.uf, e.id_pais, e.data_inclusao, p.pais AS nome_pais 
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
                                    NomePais = reader["nome_pais"].ToString() // Puxa o nome vindo do JOIN
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
    }
}