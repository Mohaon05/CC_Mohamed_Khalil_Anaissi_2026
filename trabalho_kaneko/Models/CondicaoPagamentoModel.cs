using System;

namespace trabalho_kaneko.Models
{
    public class CondicaoPagamentoModel
    {
        public int IdCondicao { get; set; }
        public string CondicaoPagamento { get; set; } 
        public int QuantidadeParcelas { get; set; } 
        public decimal? Juros { get; set; } 
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}