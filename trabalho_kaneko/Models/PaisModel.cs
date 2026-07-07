using System;

namespace trabalho_kaneko.Models
{
    public class PaisModel
    {
        public int IdPais { get; set; }
        public string Pais { get; set; }
        public string Sigla { get; set; }
        public string Ddi { get; set; }
        public string Moeda { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}