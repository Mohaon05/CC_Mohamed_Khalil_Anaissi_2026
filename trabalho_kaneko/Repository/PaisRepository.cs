using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using trabalho_kaneko.Data;
using trabalho_kaneko.Models;

namespace trabalho_kaneko.Repository
{
    public class PaisRepository
    {
        private readonly DbContext _context;

        public PaisRepository(DbContext context)
        {
            _context = context;
        }

        public bool Inserir(PaisModel pais)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();

                    // Não precisamos enviar data_inclusao e data_alteracao, o MySQL preenche sozinho
                    string query = @"INSERT INTO paises (pais, sigla) VALUES (@pais, @sigla)";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@pais", pais.Pais);
                        command.Parameters.AddWithValue("@sigla", pais.Sigla.ToUpper()); // Força a sigla a ficar maiúscula

                        int linhasAfetadas = command.ExecuteNonQuery();
                        return linhasAfetadas > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao inserir país: " + ex.Message);
                return false;
            }
        }

        public List<PaisModel> ListarTodos()
        {
            List<PaisModel> paises = new List<PaisModel>();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "SELECT id_pais, pais, sigla, data_inclusao FROM paises ORDER BY pais ASC";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                paises.Add(new PaisModel
                                {
                                    IdPais = Convert.ToInt32(reader["id_pais"]),
                                    Pais = reader["pais"].ToString(),
                                    Sigla = reader["sigla"].ToString(),
                                    DataInclusao = Convert.ToDateTime(reader["data_inclusao"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao listar países: " + ex.Message);
            }
            return paises;
        }
    }
}