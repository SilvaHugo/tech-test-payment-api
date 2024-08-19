using FluentValidation;
using MediatR;
using PottencialTechTest.App.Api.Produtos.RemoverProdutos.Dto.Request;
using PottencialTechTest.Domain.Interfaces.Servicos;
using PottencialTechTest.Domain.Shared.Response;

namespace PottencialTechTest.App.Api.Produtos.RemoverProdutos.Handler
{
    public class RemoverProdutosHandler : IRequestHandler<RemoverProdutosRequest, ResponseBase>
    {
        private readonly IProdutoService _produtoService;
        private readonly IVendaService _vendaService;
        private readonly IValidator<RemoverProdutosRequest> _validator;

        public RemoverProdutosHandler(IProdutoService produtoService, IVendaService vendaService, IValidator<RemoverProdutosRequest> validator)
        {
            _produtoService = produtoService;
            _vendaService = vendaService;
            _validator = validator;
        }

        public async Task<ResponseBase> Handle(RemoverProdutosRequest request, CancellationToken ct)
        {
            var response = new ResponseBase();

            try
            {
                var validationResult = await _validator.ValidateAsync(request, ct);

                if (!validationResult.IsValid)
                {
                    response.SetBody(validationResult.Errors.ToList());
                    return response;
                }

                var listaProdutos = await _produtoService.ListarPorAsync(x => request.IdsProdutos.Contains(x.Id), ct);
                await _produtoService.RemoverListaAsync(listaProdutos.ToList());
                response.SetSucesso();
            }
            catch
            {
                response.Mensagem = "A operação não pôde ser concluída. Por favor, tente novamente mais tarde.";
            }

            return response;
        }
    }
}
