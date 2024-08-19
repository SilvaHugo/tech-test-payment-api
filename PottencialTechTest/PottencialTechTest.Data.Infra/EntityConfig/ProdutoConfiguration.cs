using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PottencialTechTest.Domain.Entidades;

namespace PottencialTechTest.Data.Infra.EntityConfig
{
    public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.NomeProduto)
                .IsRequired();

            builder.Property(x => x.ValorProduto)
                .IsRequired();

            builder.HasOne(x => x.Venda)
                .WithMany(u => u.Produtos)
                .HasForeignKey(x => x.VendaId)
                .OnDelete(DeleteBehavior.Restrict); //remover a exclusão em cascata
        }
    }
}

