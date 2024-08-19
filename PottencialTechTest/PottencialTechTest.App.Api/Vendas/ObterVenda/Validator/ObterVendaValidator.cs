using FluentValidation;
using PottencialTechTest.App.Api.Vendas.ObterVenda.Dto.Request;

namespace PottencialTechTest.App.Api.Vendas.ObterVenda.Validator
{
    public class ObterVendaValidator : AbstractValidator<ObterVendaRequest>
    {
        public ObterVendaValidator()
        {
            RuleFor(x => x.VendaId)
                .NotEmpty().WithMessage("O ID da venda é obrigatório.");
        }
    }
}
