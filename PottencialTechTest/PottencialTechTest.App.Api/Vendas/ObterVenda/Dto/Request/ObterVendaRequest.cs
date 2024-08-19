using MediatR;
using PottencialTechTest.Domain.Shared.Response;

namespace PottencialTechTest.App.Api.Vendas.ObterVenda.Dto.Request
{
    public class ObterVendaRequest : IRequest<ResponseBase>
    {
        public Guid VendaId { get; set; }
    }
}
