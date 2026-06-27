using System;

namespace trabalho_kaneko.Models
{
    public class CondicaoPagamentoModel
    {
        public int IdCondicao { get; set; }
        public string CondicaoPagamento { get; set; } // Obrigatório
        public int QuantidadeParcelas { get; set; } // Obrigatório (Default 1)
        public decimal? Juros { get; set; } // Opcional no banco (Default 0.00)
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}