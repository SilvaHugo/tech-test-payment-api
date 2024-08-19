using MediatR;
using PottencialTechTest.Domain.Arguments.Produto;
using PottencialTechTest.Domain.Shared.Response;

namespace PottencialTechTest.App.Api.Vendas.IncluirVenda.Dto.Request
{
    public class IncluirVendaRequest : IRequest<ResponseBase>
    {
        public List<ProdutoDto> Produtos { get; set; }
        public Guid VendedorId { get; set; }
        public DateTime DataVenda { get; set; }
    }
}
