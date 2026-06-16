using System;

namespace trabalho_kaneko.Models
{
    public class CidadeModel
    {
        public int IdCidade { get; set; }
        public string Cidade { get; set; }

        // NOVO: Chave estrangeira exigida pelo banco para vincular ao Estado
        public int IdEstado { get; set; }

        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }

        // NOVO: Campo extra para exibirmos o nome do estado na tabela HTML
        public string? NomeEstado { get; set; }
    }
}