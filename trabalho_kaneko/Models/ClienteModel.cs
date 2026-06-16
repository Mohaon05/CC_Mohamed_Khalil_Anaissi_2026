using System;

namespace trabalho_kaneko.Models
{
    public class ClienteModel
    {
        public int IdCliente { get; set; }
        public string Cliente { get; set; }
        public string Documento { get; set; } // Alterado (antigo cpf_ou_cnpj)
        public string RegistroGeral { get; set; } // Alterado (antigo rg_ou_inscricao)
        public DateTime? DataNascimento { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public int IdCidade { get; set; }
        public string Email { get; set; } // Sincronizado com o banco
        public string Tipo { get; set; } // 'Fisico' ou 'Juridico'
        public DateTime DataInclusao { get; set; }
        public DateTime DataAlteracao { get; set; }
    }
}