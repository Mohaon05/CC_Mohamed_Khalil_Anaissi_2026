using System;

namespace trabalho_kaneko.Models
{
    public class GrupoModel
    {
        public int IdGrupo { get; set; }
        public string Grupo { get; set; } // Obrigatório
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}