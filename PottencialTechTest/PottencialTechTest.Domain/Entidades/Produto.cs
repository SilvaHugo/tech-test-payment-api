using PottencialTechTest.Domain.Entidades.Base;

namespace PottencialTechTest.Domain.Entidades
{
    public class Produto : EntidadeBase
    {
        public string? NomeProduto { get; set; }
        public decimal ValorProduto { get; set; }
        public Guid VendaId { get; set; }
        public Venda Venda { get; set; }
    }
}
