using FluentValidation;
using PottencialTechTest.App.Api.Produtos.IncluirProdutos.Dto.Request;
using PottencialTechTest.Domain.Arguments.Produto;
using PottencialTechTest.Domain.Interfaces.Servicos;
using PottencialTechTest.Domain.Shared.Enum;

namespace PottencialTechTest.App.Api.Produtos.IncluirProdutos.Validator
{
    public class IncluirProdutoValidator : AbstractValidator<IncluirProdutoRequest>
    {
        private readonly IVendaService _vendaService;
        public IncluirProdutoValidator(IVendaService vendaService)
        {
            _vendaService = vendaService;

            RuleFor(x => x.VendaId)
                .NotEmpty().WithMessage("O ID da venda é obrigatório.")
                .MustAsync(VerificarVendaValida).WithMessage("Venda não encontrada ou status não permite alteração de produtos.");

            RuleFor(x => x.Produtos)
                .NotEmpty().WithMessage("A lista de produtos não pode estar vazia.")
                .MustAsync(ProdutosValidos).WithMessage("Existem produtos inválidos na lista.");
        }

        private async Task<bool> VerificarVendaValida(Guid vendaId, CancellationToken cancellationToken)
        {
            var venda = await _vendaService.ObterPorIdAsync(vendaId, cancellationToken);
            return venda != null && venda.StatusVenda == StatusVenda.AguardandoPagamento;
        }

        private async Task<bool> ProdutosValidos(List<ProdutoDto> produtos, CancellationToken token) => produtos.All(p => !string.IsNullOrWhiteSpace(p.NomeProduto) && p.ValorProduto > 0);

    }
}
