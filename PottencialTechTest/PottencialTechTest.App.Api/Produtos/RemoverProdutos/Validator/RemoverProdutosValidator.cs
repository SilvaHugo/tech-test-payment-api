using FluentValidation;
using PottencialTechTest.App.Api.Produtos.RemoverProdutos.Dto.Request;
using PottencialTechTest.Domain.Interfaces.Servicos;
using PottencialTechTest.Domain.Shared.Enum;

namespace PottencialTechTest.App.Api.Produtos.RemoverProdutos.Validator
{
    public class RemoverProdutosValidator : AbstractValidator<RemoverProdutosRequest>
    {
        private readonly IProdutoService _produtoService;
        private readonly IVendaService _vendaService;

        public RemoverProdutosValidator(IProdutoService produtoService, IVendaService vendaService)
        {
            _produtoService = produtoService;
            _vendaService = vendaService;

            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.VendaId)
                .NotEmpty().WithMessage("O ID da venda é obrigatório.")
                .MustAsync(VerificarVendaValida).WithMessage("Venda não encontrada ou status não permite alteração de produtos.");

            RuleFor(x => x.IdsProdutos)
                .NotEmpty().WithMessage("A lista de produtos a serem removidos não pode estar vazia.")
                .MustAsync(ValidoParaRemover).WithMessage("A remoção dos produtos deixará a venda sem produtos.");

            RuleFor(x => x)
                .MustAsync(ExistemTodosOsProdutos).WithMessage("Algum dos produtos informados não existe.")
                .MustAsync(TodosProdutosDaMesmaVenda).WithMessage("Os produtos não pertencem à mesma venda.");
        }

        private async Task<bool> VerificarVendaValida(Guid vendaId, CancellationToken cancellationToken)
        {
            var venda = await _vendaService.ObterPorIdAsync(vendaId, cancellationToken);
            return venda != null && venda.StatusVenda == StatusVenda.AguardandoPagamento;
        }

        private async Task<bool> ValidoParaRemover(RemoverProdutosRequest request, List<Guid> idsProdutosParaRemover, CancellationToken cancellationToken)
        {
            var produtos = await _produtoService.ListarPorAsync(x => x.VendaId == request.VendaId, cancellationToken);

            var produtosRestantes = produtos.Count(p => !idsProdutosParaRemover.Contains(p.Id));
            return produtosRestantes > 0;
        }

        private async Task<bool> ExistemTodosOsProdutos(RemoverProdutosRequest request, CancellationToken cancellationToken)
        {
            if (request.IdsProdutos == null || !request.IdsProdutos.Any())
                return false;

            var produtosExistentes = await _produtoService.ListarPorAsync(x => request.IdsProdutos.Contains(x.Id), cancellationToken);

            return produtosExistentes.Count() == request.IdsProdutos.Count();
        }

        private async Task<bool> TodosProdutosDaMesmaVenda(RemoverProdutosRequest request, CancellationToken cancellationToken)
        {
            var produtos = await _produtoService.ListarPorAsync(x => request.IdsProdutos.Contains(x.Id) && x.VendaId == request.VendaId, cancellationToken);

            if (produtos == null || !produtos.Any())
                return false;

            return produtos.Count() == request.IdsProdutos.Count();
        }
    }
}
