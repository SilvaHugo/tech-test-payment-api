using FluentValidation;
using MediatR;
using PottencialTechTest.App.Api.Produtos.IncluirProdutos.Dto.Request;
using PottencialTechTest.Domain.Entidades;
using PottencialTechTest.Domain.Interfaces.Servicos;
using PottencialTechTest.Domain.Shared.Response;

namespace PottencialTechTest.App.Api.Produtos.IncluirProdutos.Handler
{
    public class IncluirProdutoHandler : IRequestHandler<IncluirProdutoRequest, ResponseBase>
    {
        private readonly IProdutoService _produtoService;
        private readonly IVendaService _vendaService;
        private readonly IValidator<IncluirProdutoRequest> _validator;

        public IncluirProdutoHandler(IProdutoService produtoService, IVendaService vendaService, IValidator<IncluirProdutoRequest> validator)
        {
            _produtoService = produtoService;
            _vendaService = vendaService;
            _validator = validator;
        }

        public async Task<ResponseBase> Handle(IncluirProdutoRequest request, CancellationToken ct)
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

                var produtos = new List<Produto>();
                foreach (var produtoDto in request.Produtos)
                {
                    var produto = new Produto()
                    {
                        NomeProduto = produtoDto.NomeProduto,
                        ValorProduto = produtoDto.ValorProduto,
                        VendaId = request.VendaId
                    };

                    await _produtoService.InserirAsync(produto, ct);
                    produtos.Add(produto);
                }

                response.SetSucesso(produtos.ToList());
            }
            catch
            {
                response.Mensagem = "A operação não pôde ser concluída. Por favor, tente novamente mais tarde.";
            }

            return response;
        }
    }
}
