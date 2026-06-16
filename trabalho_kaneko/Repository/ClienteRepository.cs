using System;
using MySql.Data.MySqlClient;
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

        public bool Inserir(ClienteModel cliente)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();

                    // Query atualizada com os novos nomes das colunas do banco de dados
                    string query = @"INSERT INTO clientes 
                                    (cliente, documento, registro_geral, data_nascimento, logradouro, numero, bairro, cep, id_cidade, email, tipo) 
                                    VALUES 
                                    (@cliente, @documento, @registro_geral, @data_nascimento, @logradouro, @numero, @bairro, @cep, @id_cidade, @email, @tipo)";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@cliente", cliente.Cliente);
                        command.Parameters.AddWithValue("@documento", cliente.Documento);
                        command.Parameters.AddWithValue("@registro_geral", (object)cliente.RegistroGeral ?? DBNull.Value);
                        command.Parameters.AddWithValue("@data_nascimento", (object)cliente.DataNascimento ?? DBNull.Value);
                        command.Parameters.AddWithValue("@logradouro", (object)cliente.Logradouro ?? DBNull.Value);
                        command.Parameters.AddWithValue("@numero", (object)cliente.Numero ?? DBNull.Value);
                        command.Parameters.AddWithValue("@bairro", (object)cliente.Bairro ?? DBNull.Value);
                        command.Parameters.AddWithValue("@cep", (object)cliente.Cep ?? DBNull.Value);
                        command.Parameters.AddWithValue("@id_cidade", cliente.IdCidade);
                        command.Parameters.AddWithValue("@email", (object)cliente.Email ?? DBNull.Value);
                        command.Parameters.AddWithValue("@tipo", cliente.Tipo);

                        int linhasAfetadas = command.ExecuteNonQuery();
                        return linhasAfetadas > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Se der erro ao salvar, você conseguirá ver a mensagem exata aqui no console de saída
                Console.WriteLine("Erro ao inserir cliente: " + ex.Message);
                return false;
            }
        }
    }
}