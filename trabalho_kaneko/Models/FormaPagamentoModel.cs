using System;

namespace trabalho_kaneko.Models
{
    public class FormaPagamentoModel
    {
        public int IdFormaPagamento { get; set; }
        public string FormaPagamento { get; set; }
        public bool Ativo { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}