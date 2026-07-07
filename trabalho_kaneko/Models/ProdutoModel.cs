using System;

namespace trabalho_kaneko.Models
{
    public class ProdutoModel
    {
        public int IdProduto { get; set; }
        public string Produto { get; set; } 
        public string Und { get; set; } 
        public decimal PrecoVenda { get; set; }
        public decimal PrecoCompra { get; set; }
        public decimal Custo { get; set; }
        public int Saldo { get; set; }
        public int IdMarca { get; set; }
        public int IdGrupo { get; set; }
        public DateTime? DataUltCompra { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }

        // Propriedades extras para junção de tabelas (INNER JOIN)
        public string? NomeMarca { get; set; }
        public string? NomeGrupo { get; set; }
    }
}