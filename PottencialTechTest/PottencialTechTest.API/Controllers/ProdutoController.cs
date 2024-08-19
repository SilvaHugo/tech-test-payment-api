using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PottencialTechTest.App.Api.Produtos.IncluirProdutos.Dto.Request;
using PottencialTechTest.App.Api.Produtos.RemoverProdutos.Dto.Request;
using PottencialTechTest.Domain.Shared.Response;

namespace PottencialTechTest.API.Controllers
{
    [ApiController]
    [Route("produto")]
    [Authorize(AuthenticationSchemes = "ApiKeyAuthentication")]
    public class ProdutoController : Controller
    {
        private readonly IMediator _mediator;

        public ProdutoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("incluir-produto")]
        public async Task<ResponseBase> IncluirProdutoAsync(IncluirProdutoRequest request, CancellationToken ct) => await _mediator.Send(request, ct);

        [HttpPost("remover-produto")]
        public async Task<ResponseBase> RemoverProdutoAsync(RemoverProdutosRequest request, CancellationToken ct) => await _mediator.Send(request, ct);
    }
}
