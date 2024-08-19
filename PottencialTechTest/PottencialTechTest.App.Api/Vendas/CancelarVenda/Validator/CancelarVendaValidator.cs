using FluentValidation;
using PottencialTechTest.App.Api.Vendas.CancelarVenda.Dto.Request;
using PottencialTechTest.Domain.Interfaces.Servicos;
using PottencialTechTest.Domain.Shared.Enum;

namespace PottencialTechTest.App.Api.Vendas.CancelarVenda.Validator
{
    public class CancelarVendaValidator : AbstractValidator<CancelarVendaRequest>
    {
        private readonly IVendaService _vendaService;

        public CancelarVendaValidator(IVendaService vendaService)
        {
            _vendaService = vendaService;

            RuleFor(x => x.VendaId)
                .NotEmpty().WithMessage("O ID da venda é obrigatório.");

            RuleFor(x => x)
                .MustAsync(VendaValidaParaCancelamento)
                .WithMessage("Venda não encontrada ou o status atual da venda não permite cancelamento.");
        }

        private async Task<bool> VendaValidaParaCancelamento(CancelarVendaRequest request, CancellationToken cancellationToken)
        {
            var venda = await _vendaService.ObterPorIdAsync(request.VendaId, cancellationToken);

            if (venda == null)
                return false;

            return venda.StatusVenda == StatusVenda.AguardandoPagamento || venda.StatusVenda == StatusVenda.PagamentoAprovado;
        }
    }
}
