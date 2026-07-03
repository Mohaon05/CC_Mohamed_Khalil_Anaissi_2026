using System;

namespace trabalho_kaneko.Models
{
    public class CidadeModel
    {
        public int IdCidade { get; set; }
        public string Cidade { get; set; }

        public int IdEstado { get; set; }

        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }

        public string? NomeEstado { get; set; }
    }
}