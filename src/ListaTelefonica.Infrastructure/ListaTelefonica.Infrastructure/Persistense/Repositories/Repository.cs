using ListaTelefonica.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListaTelefonica.Infrastructure.Persistense.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly ListaTelefonicaDbContext Context;
        protected readonly DbSet<TEntity> DbSet;

        protected Repository(ListaTelefonicaDbContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        public void Delete(TEntity entity) =>
            DbSet.Remove(entity);

        public async Task<bool> Exists(int id) =>
            await DbSet.AsNoTracking().AnyAsync(x => Equals(x.Id, id));

        public  async Task<TEntity> Insert(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentException("Não pode adicionar entidade nula");

            var entityEntry = await DbSet.AddAsync(entity);
            _ = Context.SaveChangesAsync();

            return entityEntry.Entity;
        }

        public IQueryable<TEntity> AsQueryable() => DbSet;

        public async Task<TEntity> SelectById(int id) =>
            await DbSet.FindAsync(new object[] { id });

        public async Task Update(TEntity entity)
        {
            if (await Exists(entity.Id) == false)
                return;

            DbSet.Update(entity);
        }

        public async Task Commit() => await Context.SaveChangesAsync(true);

    }
}
