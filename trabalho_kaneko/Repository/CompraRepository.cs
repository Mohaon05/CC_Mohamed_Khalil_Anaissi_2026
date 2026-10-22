using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using trabalho_kaneko.Data;
using trabalho_kaneko.Models;

namespace trabalho_kaneko.Repository
{
    public class CompraRepository
    {
        private readonly DbContext _context;

        public CompraRepository(DbContext context)
        {
            _context = context;
        }

        // ========================================================================
        // 1. MÉTODO PARA LISTAR TODAS AS NOTAS FISCAIS
        // ========================================================================
        public List<CompraModel> ListarTodos()
        {
            List<CompraModel> compras = new List<CompraModel>();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM compras ORDER BY data_inclusao DESC";
                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            compras.Add(new CompraModel
                            {
                                Modelo = reader["modelo"].ToString(),
                                Serie = reader["serie"].ToString(),
                                NumNfe = Convert.ToInt32(reader["num_nfe"]),
                                IdFornecedor = Convert.ToInt32(reader["id_fornecedor"]),
                                DataEmitNfe = reader["data_emit_nfe"] != DBNull.Value ? Convert.ToDateTime(reader["data_emit_nfe"]) : (DateTime?)null,
                                DataInclusao = Convert.ToDateTime(reader["data_inclusao"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao listar compras: " + ex.Message);
            }
            return compras;
        }

        // ========================================================================
        // 2. MÉTODO PARA INSERIR UMA NOVA NOTA FISCAL (TODOS OS 33 CAMPOS)
        // ========================================================================
        public bool Inserir(CompraModel compra)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = @"INSERT INTO compras 
                        (modelo, serie, num_nfe, id_fornecedor, pagina, nat_oper, prot_acesso, data_prot_acesso, hora_prot_acesso, 
                         chave_acesso_nfe, data_emit_nfe, data_ent_nfe, hora_ent_nfe, base_calc_icms, valor_icms, base_calc_icms_sub, 
                         valor_icms_sub, valor_frete_nfe, valor_seguro_nfe, desconto_nfe, outras_desp_nfe, valor_ipi, id_transportadora, 
                         frete_por_conta_nfe, id_veiculo, qtde_vol, especie_vol, marca_vol, num_vol, peso_bruto, peso_liq, inf_comp, id_condicao) 
                        VALUES 
                        (@modelo, @serie, @num_nfe, @id_fornecedor, @pagina, @nat_oper, @prot_acesso, @data_prot_acesso, @hora_prot_acesso, 
                         @chave_acesso_nfe, @data_emit_nfe, @data_ent_nfe, @hora_ent_nfe, @base_calc_icms, @valor_icms, @base_calc_icms_sub, 
                         @valor_icms_sub, @valor_frete_nfe, @valor_seguro_nfe, @desconto_nfe, @outras_desp_nfe, @valor_ipi, @id_transportadora, 
                         @frete_por_conta_nfe, @id_veiculo, @qtde_vol, @especie_vol, @marca_vol, @num_vol, @peso_bruto, @peso_liq, @inf_comp, @id_condicao)";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        // Chaves obrigatórias
                        command.Parameters.AddWithValue("@modelo", compra.Modelo);
                        command.Parameters.AddWithValue("@serie", compra.Serie);
                        command.Parameters.AddWithValue("@num_nfe", compra.NumNfe);
                        command.Parameters.AddWithValue("@id_fornecedor", compra.IdFornecedor);
                        command.Parameters.AddWithValue("@id_condicao", compra.IdCondicao);

                        // Campos opcionais (tratando os valores nulos)
                        command.Parameters.AddWithValue("@pagina", compra.Pagina ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@nat_oper", compra.NatOper ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@prot_acesso", compra.ProtAcesso ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@data_prot_acesso", compra.DataProtAcesso ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@hora_prot_acesso", compra.HoraProtAcesso ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@chave_acesso_nfe", compra.ChaveAcessoNfe ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@data_emit_nfe", compra.DataEmitNfe ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@data_ent_nfe", compra.DataEntNfe ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@hora_ent_nfe", compra.HoraEntNfe ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@base_calc_icms", compra.BaseCalcIcms ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@valor_icms", compra.ValorIcms ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@base_calc_icms_sub", compra.BaseCalcIcmsSub ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@valor_icms_sub", compra.ValorIcmsSub ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@valor_frete_nfe", compra.ValorFreteNfe ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@valor_seguro_nfe", compra.ValorSeguroNfe ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@desconto_nfe", compra.DescontoNfe ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@outras_desp_nfe", compra.OutrasDespNfe ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@valor_ipi", compra.ValorIpi ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@id_transportadora", compra.IdTransportadora ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@frete_por_conta_nfe", compra.FretePorContaNfe ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@id_veiculo", compra.IdVeiculo ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@qtde_vol", compra.QtdeVol ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@especie_vol", compra.EspecieVol ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@marca_vol", compra.MarcaVol ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@num_vol", compra.NumVol ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@peso_bruto", compra.PesoBruto ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@peso_liq", compra.PesoLiq ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@inf_comp", compra.InfComp ?? (object)DBNull.Value);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao inserir compra: " + ex.Message);
                return false;
            }
        }

        // ========================================================================
        // 3. MÉTODO PARA EXCLUIR USANDO A CHAVE COMPOSTA
        // ========================================================================
        public bool Excluir(string modelo, string serie, int numNfe, int idFornecedor)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();
                    string query = "DELETE FROM compras WHERE modelo = @modelo AND serie = @serie AND num_nfe = @numNfe AND id_fornecedor = @idFornecedor";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@modelo", modelo);
                        command.Parameters.AddWithValue("@serie", serie);
                        command.Parameters.AddWithValue("@numNfe", numNfe);
                        command.Parameters.AddWithValue("@idFornecedor", idFornecedor);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao excluir compra: " + ex.Message);
                return false;
            }
        }
    }
}