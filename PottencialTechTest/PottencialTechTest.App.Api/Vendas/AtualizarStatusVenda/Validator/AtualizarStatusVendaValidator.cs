using FluentValidation;
using PottencialTechTest.App.Api.Vendas.AtualizarStatusVenda.Dto.Request;
using PottencialTechTest.Domain.Interfaces.Servicos;
using PottencialTechTest.Domain.Shared.Enum;

namespace PottencialTechTest.App.Api.Vendas.AtualizarStatusVenda.Validator
{
    public class AtualizarStatusVendaValidator : AbstractValidator<AtualizarStatusVendaRequest>
    {
        public readonly IVendaService _vendaService;

        public AtualizarStatusVendaValidator(IVendaService vendaService)
        {
            _vendaService = vendaService;

            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.VendaId)
                .NotEmpty().WithMessage("O ID da venda é obrigatório.")
                .MustAsync(VendaValida).WithMessage("Venda não encontrada.");

            RuleFor(x => x.StatusVenda)
                .IsInEnum().WithMessage("Status de venda inválido.")
                .NotEqual(StatusVenda.Cancelado).WithMessage("Para cancelar a venda, utilize a rota específica de cancelamento.");

            RuleFor(x => x)
                .MustAsync(TransicaoValidaAsync)
                .WithMessage("Venda não encontrada ou transição de status inválida.");
        }

        private async Task<bool> VendaValida(Guid vendaId, CancellationToken cancellationToken)
        {
            var venda = await _vendaService.ObterPorIdAsync(vendaId, cancellationToken);
            return venda != null;
        }

        private async Task<bool> TransicaoValidaAsync(AtualizarStatusVendaRequest request, CancellationToken cancellationToken)
        {
            var venda = await _vendaService.ObterPorIdAsync(request.VendaId, cancellationToken);

            var transicoesPermitidas = new Dictionary<StatusVenda, List<StatusVenda>>
            {
                { StatusVenda.AguardandoPagamento, new List<StatusVenda> { StatusVenda.PagamentoAprovado } },
                { StatusVenda.PagamentoAprovado, new List<StatusVenda> { StatusVenda.EnviadoTransportadora } },
                { StatusVenda.EnviadoTransportadora, new List<StatusVenda> { StatusVenda.Entregue } }
            };

            return transicoesPermitidas.TryGetValue(venda.StatusVenda, out var destinosPermitidos) && destinosPermitidos.Contains(request.StatusVenda);
        }
    }
}
