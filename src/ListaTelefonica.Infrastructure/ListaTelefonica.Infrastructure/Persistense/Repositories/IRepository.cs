using ListaTelefonica.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListaTelefonica.Infrastructure.Persistense.Repositories
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        void Delete(TEntity entity);
        Task<bool> Exists(int id);
        Task<TEntity> Insert(TEntity entity);
        IQueryable<TEntity> AsQueryable();
        Task<TEntity> SelectById(int id);
        Task Update(TEntity entity);
        Task Commit();
    }
}
