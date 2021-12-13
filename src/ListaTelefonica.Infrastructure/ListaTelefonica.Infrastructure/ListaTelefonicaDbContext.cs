using ListaTelefonica.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ListaTelefonica.Infrastructure
{
    public class ListaTelefonicaDbContext : DbContext
    {
        public ListaTelefonicaDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Contato> Contato { get; set; }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            AddCreatedAt();
            AddUpdatedAt();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void AddCreatedAt() =>
            ChangeTracker.Entries().Where(p => p.Entity is BaseEntity && p.State.Equals(EntityState.Added)).ToList()
                .ForEach(p => ((BaseEntity)p.Entity).CriadoEm = DateTime.UtcNow);

        private void AddUpdatedAt() =>
            ChangeTracker.Entries().Where(p => p.Entity is BaseEntity && p.State.Equals(EntityState.Modified)).ToList()
                .ForEach(p => ((BaseEntity)p.Entity).AtualizadoEm = DateTime.UtcNow);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
