using System;

namespace trabalho_kaneko.Models
{
    public class CargoModel
    {
        public int IdCargo { get; set; }
        public string Cargo { get; set; }
        public string? Descricao { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}