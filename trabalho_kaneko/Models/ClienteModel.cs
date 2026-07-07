using System;

namespace trabalho_kaneko.Models
{
    public class ClienteModel
    {
        public int IdCliente { get; set; }

        public string Cliente { get; set; }
        public string TipoPessoa { get; set; }
        public string CpfCnpj { get; set; }

        public string? RgInscest { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string? Cep { get; set; }
        public string? Email { get; set; }

        public int IdCidade { get; set; }

        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }

        public string? NomeCidade { get; set; }
    }
}