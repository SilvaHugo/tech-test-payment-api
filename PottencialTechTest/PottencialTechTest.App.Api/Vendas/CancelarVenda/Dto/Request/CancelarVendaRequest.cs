using MediatR;
using PottencialTechTest.Domain.Shared.Response;

namespace PottencialTechTest.App.Api.Vendas.CancelarVenda.Dto.Request
{
    public class CancelarVendaRequest : IRequest<ResponseBase>
    {
        public Guid VendaId { get; set; }
    }
}
