using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PottencialTechTest.App.Api.Vendas.AtualizarStatusVenda.Dto.Request;
using PottencialTechTest.App.Api.Vendas.CancelarVenda.Dto.Request;
using PottencialTechTest.App.Api.Vendas.IncluirVenda.Dto.Request;
using PottencialTechTest.App.Api.Vendas.ObterVenda.Dto.Request;
using PottencialTechTest.Domain.Shared.Response;

namespace PottencialTechTest.API.Controllers
{
    [ApiController]
    [Route("venda")]
    [Authorize(AuthenticationSchemes = "ApiKeyAuthentication")]
    public class VendaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VendaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("incluir-venda")]
        public async Task<ResponseBase> IncluirVendaAsync(IncluirVendaRequest request, CancellationToken ct) => await _mediator.Send(request, ct);

        [HttpPost("atualizar-status-venda")]
        public async Task<ResponseBase> AtualizarStatusVendaAsync(AtualizarStatusVendaRequest request, CancellationToken ct) => await _mediator.Send(request, ct);

        [HttpPost("cancelar-venda")]
        public async Task<ResponseBase> CancelarVendaAsync(CancelarVendaRequest request, CancellationToken ct) => await _mediator.Send(request, ct);

        [HttpGet("obter-venda")]
        public async Task<ResponseBase> ObterVendaAsync(ObterVendaRequest request, CancellationToken ct) => await _mediator.Send(request, ct);
    }
}
