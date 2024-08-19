using FluentValidation;
using PottencialTechTest.App.Api.Vendas.IncluirVenda.Dto.Request;
using PottencialTechTest.Domain.Arguments.Produto;
using PottencialTechTest.Domain.Interfaces.Servicos;

namespace PottencialTechTest.App.Api.Vendas.IncluirVenda.Validator
{
    public class IncluirVendaValidator : AbstractValidator<IncluirVendaRequest>
    {
        private readonly IVendedorService _vendedorService;

        public IncluirVendaValidator(IVendedorService vendedorService)
        {
            _vendedorService = vendedorService;

            RuleFor(x => x.VendedorId)
                .NotEmpty().WithMessage("O ID do vendedor é obrigatório.")
                .MustAsync(ExisteVendedor).WithMessage("O vendedor com o ID informado não existe.");

            RuleFor(x => x.Produtos)
                .NotEmpty().WithMessage("A lista de produtos não pode estar vazia.")
                .MustAsync(ProdutosValidos).WithMessage("Existem produtos inválidos na lista.");

            RuleFor(x => x.DataVenda)
                .NotEmpty().WithMessage("A data da venda é obrigatória.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("A data da venda não pode ser futura.");
        }

        private async Task<bool> ExisteVendedor(Guid vendedorId, CancellationToken cancellationToken) => await _vendedorService.ObterPorIdAsync(vendedorId, cancellationToken) != null;
        private async Task<bool> ProdutosValidos(List<ProdutoDto> produtos, CancellationToken token) => produtos.All(p => !string.IsNullOrWhiteSpace(p.NomeProduto) && p.ValorProduto > 0);
    }
}
