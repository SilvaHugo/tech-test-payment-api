using MediatR;
using PottencialTechTest.Domain.Arguments.Produto;
using PottencialTechTest.Domain.Shared.Response;

namespace PottencialTechTest.App.Api.Produtos.IncluirProdutos.Dto.Request
{
    public class IncluirProdutoRequest : IRequest<ResponseBase>
    {
        public List<ProdutoDto> Produtos { get; set; }
        public Guid VendaId { get; set; }
    }
}
