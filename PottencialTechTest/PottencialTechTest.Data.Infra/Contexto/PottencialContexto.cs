using Microsoft.EntityFrameworkCore;
using PottencialTechTest.Data.Infra.EntityConfig;
using PottencialTechTest.Domain.Entidades;

namespace PottencialTechTest.Data.Infra.Contexto
{
    public class PottencialContexto : DbContext
    {
        public PottencialContexto(DbContextOptions<PottencialContexto> options)
            : base(options)
        {

        }

        public DbSet<Venda> Vendas { get; set; }
        public DbSet<Vendedor> Vendedores { get; set; }
        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Remove pluralização dos nomes das tabelas
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.DisplayName());
            }

            // Remove exclusão em cascata
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            // Definir campos com o nome Id como Primary Key
            modelBuilder.Entity<Venda>()
                .HasKey(u => u.Id);
            modelBuilder.Entity<Vendedor>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<Produto>()
                .HasKey(e => e.Id);

            // Definir campos do tipo string como varchar
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(string)))
            {
                property.SetColumnType("varchar(255)");
            }

            modelBuilder.ApplyConfiguration(new VendedorConfiguration());
            modelBuilder.ApplyConfiguration(new VendaConfiguration());
            modelBuilder.ApplyConfiguration(new ProdutoConfiguration());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("CriadoEm") != null))
            {
                // Define data de criação por padrão a data atual
                if (entry.State == EntityState.Added)
                {
                    entry.Property("CriadoEm").CurrentValue = DateTime.Now;
                }

                // Não deixa alterar a data de criação em updates
                if (entry.State == EntityState.Modified)
                {
                    entry.Property("CriadoEm").IsModified = false;
                }
            }

            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("Id") != null))
            {
                // Gera um novo GUID no insert
                if (entry.State == EntityState.Added)
                {
                    entry.Property("Id").CurrentValue = Guid.NewGuid();
                }

                // Não deixa alterar o Id em updates
                if (entry.State == EntityState.Modified)
                {
                    entry.Property("Id").IsModified = false;
                }
            }

            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("VendedorId") != null))
            {
                // Não deixa alterar o Id em updates
                if (entry.State == EntityState.Modified)
                {
                    entry.Property("VendedorId").IsModified = false;
                }
            }

            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("VendaId") != null))
            {
                // Não deixa alterar o Id em updates
                if (entry.State == EntityState.Modified)
                {
                    entry.Property("VendaId").IsModified = false;
                }
            }

            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("AlteradoEm") != null))
            {
                // Define data de alteração por padrão a data atual
                if (entry.State == EntityState.Modified)
                {
                    entry.Property("AlteradoEm").CurrentValue = DateTime.Now;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
