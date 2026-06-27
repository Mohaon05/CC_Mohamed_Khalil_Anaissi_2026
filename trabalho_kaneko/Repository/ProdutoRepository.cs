using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using trabalho_kaneko.Data;
using trabalho_kaneko.Models;

namespace trabalho_kaneko.Repository
{
    public class ProdutoRepository
    {
        private readonly DbContext _context;

        public ProdutoRepository(DbContext context)
        {
            _context = context;
        }

        public List<ProdutoModel> ListarTodos()
        {
            List<ProdutoModel> produtos = new List<ProdutoModel>();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = @"
                        SELECT p.*, m.marca AS nome_marca, g.grupo AS nome_grupo 
                        FROM produtos p 
                        INNER JOIN marcas m ON p.id_marca = m.id_marca 
                        INNER JOIN grupos g ON p.id_grupo = g.id_grupo 
                        ORDER BY p.produto ASC";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            produtos.Add(new ProdutoModel
                            {
                                IdProduto = Convert.ToInt32(reader["id_produto"]),
                                Produto = reader["produto"].ToString(),
                                Und = reader["und"].ToString(),
                                PrecoVenda = Convert.ToDecimal(reader["preco_venda"]),
                                PrecoCompra = Convert.ToDecimal(reader["preco_compra"]),
                                Custo = Convert.ToDecimal(reader["custo"]),
                                Saldo = Convert.ToInt32(reader["Saldo"]),
                                IdMarca = Convert.ToInt32(reader["id_marca"]),
                                IdGrupo = Convert.ToInt32(reader["id_grupo"]),
                                NomeMarca = reader["nome_marca"].ToString(),
                                NomeGrupo = reader["nome_grupo"].ToString(),
                                DataUltCompra = reader["data_ult_compra"] != DBNull.Value ? Convert.ToDateTime(reader["data_ult_compra"]) : (DateTime?)null,
                                DataInclusao = Convert.ToDateTime(reader["data_inclusao"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao listar produtos: " + ex.Message);
            }
            return produtos;
        }

        public bool Inserir(ProdutoModel produto)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = @"INSERT INTO produtos 
                        (produto, und, preco_venda, preco_compra, custo, Saldo, id_marca, id_grupo, data_ult_compra) 
                        VALUES 
                        (@produto, @und, @preco_venda, @preco_compra, @custo, @saldo, @id_marca, @id_grupo, @data_ult_compra)";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@produto", produto.Produto);
                        command.Parameters.AddWithValue("@und", produto.Und);
                        command.Parameters.AddWithValue("@preco_venda", produto.PrecoVenda);
                        command.Parameters.AddWithValue("@preco_compra", produto.PrecoCompra);
                        command.Parameters.AddWithValue("@custo", produto.Custo);
                        command.Parameters.AddWithValue("@saldo", produto.Saldo);
                        command.Parameters.AddWithValue("@id_marca", produto.IdMarca);
                        command.Parameters.AddWithValue("@id_grupo", produto.IdGrupo);
                        command.Parameters.AddWithValue("@data_ult_compra", produto.DataUltCompra.HasValue ? (object)produto.DataUltCompra.Value : DBNull.Value);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao inserir produto: " + ex.Message);
                return false;
            }
        }

        public ProdutoModel BuscarPorId(int id)
        {
            ProdutoModel produto = null;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM produtos WHERE id_produto = @id";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                produto = new ProdutoModel
                                {
                                    IdProduto = Convert.ToInt32(reader["id_produto"]),
                                    Produto = reader["produto"].ToString(),
                                    Und = reader["und"].ToString(),
                                    PrecoVenda = Convert.ToDecimal(reader["preco_venda"]),
                                    PrecoCompra = Convert.ToDecimal(reader["preco_compra"]),
                                    Custo = Convert.ToDecimal(reader["custo"]),
                                    Saldo = Convert.ToInt32(reader["Saldo"]),
                                    IdMarca = Convert.ToInt32(reader["id_marca"]),
                                    IdGrupo = Convert.ToInt32(reader["id_grupo"]),
                                    DataUltCompra = reader["data_ult_compra"] != DBNull.Value ? Convert.ToDateTime(reader["data_ult_compra"]) : (DateTime?)null
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao buscar produto por ID: " + ex.Message);
            }
            return produto;
        }

        public bool Atualizar(ProdutoModel produto)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = @"UPDATE produtos SET 
                        produto = @produto, und = @und, preco_venda = @preco_venda, 
                        preco_compra = @preco_compra, custo = @custo, Saldo = @saldo, 
                        id_marca = @id_marca, id_grupo = @id_grupo, data_ult_compra = @data_ult_compra, 
                        data_alteracao = NOW() 
                        WHERE id_produto = @id";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", produto.IdProduto);
                        command.Parameters.AddWithValue("@produto", produto.Produto);
                        command.Parameters.AddWithValue("@und", produto.Und);
                        command.Parameters.AddWithValue("@preco_venda", produto.PrecoVenda);
                        command.Parameters.AddWithValue("@preco_compra", produto.PrecoCompra);
                        command.Parameters.AddWithValue("@custo", produto.Custo);
                        command.Parameters.AddWithValue("@saldo", produto.Saldo);
                        command.Parameters.AddWithValue("@id_marca", produto.IdMarca);
                        command.Parameters.AddWithValue("@id_grupo", produto.IdGrupo);
                        command.Parameters.AddWithValue("@data_ult_compra", produto.DataUltCompra.HasValue ? (object)produto.DataUltCompra.Value : DBNull.Value);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar produto: " + ex.Message);
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
                    string query = "DELETE FROM produtos WHERE id_produto = @id";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao excluir produto: " + ex.Message);
                return false;
            }
        }
    }
}