using System;

namespace trabalho_kaneko.Models
{
    public class MarcaModel
    {
        public int IdMarca { get; set; }
        public string Marca { get; set; } // Obrigatório
        public int? IdGrupo { get; set; } // Pode ser nulo no banco

        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }

        // Propriedade extra para exibir o nome do grupo na listagem
        public string? NomeGrupo { get; set; }
    }
}