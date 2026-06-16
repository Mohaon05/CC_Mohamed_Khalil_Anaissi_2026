using System;

namespace trabalho_kaneko.Models
{
    public class EstadoModel
    {
        public int IdEstado { get; set; }
        public string Estado { get; set; }
        public string Uf { get; set; }
        public int IdPais { get; set; }

        // ALTERADO: O ponto de interrogação (?) avisa o C# que essas datas 
        // podem vir vazias do formulário (já que o banco preenche sozinho)
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }

        // ALTERADO: Colocamos o (?) para o C# não bloquear o cadastro
        // exigindo um Nome de País na hora de salvar
        public string? NomePais { get; set; }
    }
}