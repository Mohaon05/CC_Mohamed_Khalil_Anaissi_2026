using System;

namespace trabalho_kaneko.Models
{
    public class FuncionarioModel
    {
        public int IdFuncionario { get; set; }
        public string Funcionario { get; set; } // Obrigatório
        public string? Apelido { get; set; }
        public string Cpf { get; set; } // Obrigatório
        public string? Rg { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }

        public int IdCargo { get; set; } // Obrigatório
        public decimal? Salario { get; set; }

        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string? Cep { get; set; }

        public int IdCidade { get; set; } // Obrigatório

        public DateTime? DataContratado { get; set; }
        public DateTime? DataDemitido { get; set; }

        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }

        // Propriedades extras para os INNER JOINs de exibição
        public string? NomeCargo { get; set; }
        public string? NomeCidade { get; set; }
    }
}