using PottencialTechTest.Domain.Entidades.Base;

namespace PottencialTechTest.Domain.Entidades
{
    public class Vendedor : EntidadeBase
    {
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public string? Email { get; set; }
        public long? Telefone { get; set; }
        public ICollection<Venda>? Vendas { get; set; }
    }
}
