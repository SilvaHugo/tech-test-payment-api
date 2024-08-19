using MediatR;
using PottencialTechTest.Domain.Shared.Response;

namespace PottencialTechTest.App.Api.Produtos.RemoverProdutos.Dto.Request
{
    public class RemoverProdutosRequest : IRequest<ResponseBase>
    {
        public List<Guid> IdsProdutos { get; set; }
        public Guid VendaId { get; set; }
    }
}
