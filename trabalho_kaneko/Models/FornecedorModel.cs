using System;

namespace trabalho_kaneko.Models
{
    public class FornecedorModel
    {
        public int IdFornecedor { get; set; }

        public string Fornecedor { get; set; }
        public string? NomeFantasia { get; set; }
        public string TipoPessoa { get; set; }
        public string CpfCnpj { get; set; }
        public string? RgInscest { get; set; }
        public string? Email { get; set; }
        public string? Celular { get; set; }
        public string? Site { get; set; }
        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string? Cep { get; set; }

        public int IdCidade { get; set; }

        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }

        // Propriedade extra para exibição do INNER JOIN
        public string? NomeCidade { get; set; }
    }
}