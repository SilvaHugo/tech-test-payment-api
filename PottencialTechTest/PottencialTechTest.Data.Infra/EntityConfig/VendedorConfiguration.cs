using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PottencialTechTest.Domain.Entidades;

namespace PottencialTechTest.Data.Infra.EntityConfig
{
    public class VendedorConfiguration : IEntityTypeConfiguration<Vendedor>
    {
        public void Configure(EntityTypeBuilder<Vendedor> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Nome)
                .IsRequired();

            builder.Property(x => x.Cpf)
                .IsRequired();

            builder.Property(x => x.Telefone)
                .IsRequired();

            builder.Property(x => x.Email)
                .IsRequired();
        }
    }
}
