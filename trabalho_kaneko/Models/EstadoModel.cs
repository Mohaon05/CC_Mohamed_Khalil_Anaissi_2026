using System;

namespace trabalho_kaneko.Models
{
    public class EstadoModel
    {
        public int IdEstado { get; set; }
        public string Estado { get; set; }
        public string Uf { get; set; }
        public int IdPais { get; set; }

        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }

        public string? NomePais { get; set; }
    }
}