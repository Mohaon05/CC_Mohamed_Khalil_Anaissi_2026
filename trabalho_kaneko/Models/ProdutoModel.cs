using System;

namespace trabalho_kaneko.Models
{
    public class ProdutoModel
    {
        public int IdProduto { get; set; }
        public string Produto { get; set; } // NOT NULL
        public string Und { get; set; } // NOT NULL (Digitação livre de até 3 caracteres)
        public decimal PrecoVenda { get; set; } // NOT NULL
        public decimal PrecoCompra { get; set; } // NOT NULL DEFAULT 0.00
        public decimal Custo { get; set; } // NOT NULL DEFAULT 0.00
        public int Saldo { get; set; } // NOT NULL DEFAULT 0
        public int IdMarca { get; set; } // NOT NULL
        public int IdGrupo { get; set; } // NOT NULL
        public DateTime? DataUltCompra { get; set; } // DATETIME (Permite nulo se nunca foi comprado)
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }

        // Propriedades extras para junção de tabelas (INNER JOIN)
        public string? NomeMarca { get; set; }
        public string? NomeGrupo { get; set; }
    }
}