using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PottencialTechTest.Domain.Entidades;

namespace PottencialTechTest.Data.Infra.EntityConfig
{
    public class VendaConfiguration : IEntityTypeConfiguration<Venda>
    {
        public void Configure(EntityTypeBuilder<Venda> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Identificador)
                .IsRequired();

            builder.Property(x => x.DataVenda)
                .IsRequired();

            builder.Property(x => x.VendedorId)
                .IsRequired();

            builder.HasOne(x => x.Vendedor)
                .WithMany(u => u.Vendas)
                .HasForeignKey(x => x.VendedorId)
                .OnDelete(DeleteBehavior.Restrict); //remover a exclusão em cascata

            builder.Property(x => x.StatusVenda)
                .IsRequired()
                .HasConversion<int>();
        }
    }
}
