using System;
using System.Collections.Generic;
using System.Text;

namespace ListaTelefonica.Domain.Entities
{
    public abstract class BaseEntity
    {
        protected BaseEntity() 
        {

        }
        public int Id { get;  set; }

        public DateTime? CriadoEm { get; set; }
        public DateTime? AtualizadoEm { get; set; }
    }
}
