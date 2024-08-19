using FluentValidation;
using MediatR;
using PottencialTechTest.App.Api.Vendas.CancelarVenda.Dto.Request;
using PottencialTechTest.Domain.Interfaces.Servicos;
using PottencialTechTest.Domain.Shared.Enum;
using PottencialTechTest.Domain.Shared.Response;

namespace PottencialTechTest.App.Api.Vendas.CancelarVenda.Handler
{
    public class CancelarVendaHandler : IRequestHandler<CancelarVendaRequest, ResponseBase>
    {
        private readonly IVendaService _vendaService;
        private readonly IValidator<CancelarVendaRequest> _validator;

        public CancelarVendaHandler(IVendaService vendaService, IValidator<CancelarVendaRequest> validator)
        {
            _vendaService = vendaService;
            _validator = validator;
        }

        public async Task<ResponseBase> Handle(CancelarVendaRequest request, CancellationToken cancellationToken)
        {
            var response = new ResponseBase();

            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    response.SetBody(validationResult.Errors.ToList());
                    return response;
                }

                var venda = await _vendaService.ObterPorIdAsync(request.VendaId, cancellationToken);

                venda.StatusVenda = StatusVenda.Cancelado;
                await _vendaService.AlterarAsync(venda, cancellationToken);

                response.SetSucesso(venda);
            }
            catch
            {
                response.Mensagem = "A operação não pôde ser concluída. Por favor, tente novamente mais tarde.";
            }

            return response;
        }
    }
}
