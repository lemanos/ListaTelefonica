using ListaTelefonica.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ListaTelefonica.Infrastructure.Persistense.Maps
{
    public class ContatoMap : IEntityTypeConfiguration<Contato>
    {
        public void Configure(EntityTypeBuilder<Contato> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Nome).IsRequired().HasMaxLength(100);
            builder.Property(p => p.DataNascimento);
            builder.Property(p => p.Sexo);
            builder.Property(p => p.Idade);
            builder.Property(p => p.IsAtivo);
            builder.Property(e => e.CriadoEm);
            builder.Property(e => e.AtualizadoEm);
        }
    }
}


