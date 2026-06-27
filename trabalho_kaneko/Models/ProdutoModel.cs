using System;

namespace trabalho_kaneko.Models
{
    public class ProdutoModel
    {
        public int IdProduto { get; set; }
        public string Produto { get; set; } // Obrigatório
        public string Und { get; set; } // Obrigatório (ex: UN, KG, CX)

        public decimal PrecoVenda { get; set; }
        public decimal PrecoCompra { get; set; }
        public decimal Custo { get; set; }
        public int Saldo { get; set; }

        public int IdMarca { get; set; } // Obrigatório
        public int IdGrupo { get; set; } // Obrigatório

        public DateTime? DataUltCompra { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }

        // Propriedades extras para os INNER JOINs na listagem
        public string? NomeMarca { get; set; }
        public string? NomeGrupo { get; set; }
    }
}