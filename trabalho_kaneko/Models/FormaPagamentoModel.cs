using System;

namespace trabalho_kaneko.Models
{
    public class FormaPagamentoModel
    {
        public int IdFormaPagamento { get; set; }
        public string FormaPagamento { get; set; } // Obrigatório (NOT NULL)
        public bool Ativo { get; set; } // Mapeia o BOOLEAN do banco
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}