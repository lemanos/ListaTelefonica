using System;
using System.Collections.Generic;
using System.Text;

namespace ListaTelefonica.Domain.Entities
{
    public class Contato : BaseEntity
    {
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public bool IsAtivo { get; set; }
        public string Sexo { get; set; }
        public int? Idade { get; set; }
    }
}
