using PottencialTechTest.Domain.Entidades.Base;
using PottencialTechTest.Domain.Shared.Enum;

namespace PottencialTechTest.Domain.Entidades
{
    public class Venda : EntidadeBase
    {
        public string? Identificador { get; set; }
        public DateTime? DataVenda { get; set; }
        public ICollection<Produto>? Produtos { get; set; }
        public Guid VendedorId { get; set; }
        public Vendedor Vendedor { get; set; }
        public StatusVenda StatusVenda { get; set; }
    }
}
