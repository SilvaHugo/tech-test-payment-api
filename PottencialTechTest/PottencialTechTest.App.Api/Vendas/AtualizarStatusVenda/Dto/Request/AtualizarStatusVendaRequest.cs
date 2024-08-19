using MediatR;
using PottencialTechTest.Domain.Shared.Enum;
using PottencialTechTest.Domain.Shared.Response;

namespace PottencialTechTest.App.Api.Vendas.AtualizarStatusVenda.Dto.Request
{
    public class AtualizarStatusVendaRequest : IRequest<ResponseBase>
    {
        public Guid VendaId { get; set; }
        public StatusVenda StatusVenda { get; set; }
    }
}
