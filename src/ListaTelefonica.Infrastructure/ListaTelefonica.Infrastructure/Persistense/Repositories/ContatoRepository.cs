using ListaTelefonica.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ListaTelefonica.Infrastructure.Persistense.Repositories
{
    public class ContatoRepository : Repository<Contato>, IContatoRepository
    {
        public ContatoRepository(ListaTelefonicaDbContext context) : base(context) 
        {

        }
    }
}
