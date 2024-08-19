using FluentValidation;
using MediatR;
using PottencialTechTest.App.Api.Vendas.AtualizarStatusVenda.Dto.Request;
using PottencialTechTest.Domain.Interfaces.Servicos;
using PottencialTechTest.Domain.Shared.Response;

namespace PottencialTechTest.App.Api.Vendas.AtualizarStatusVenda.Handler
{
    public class AtualizarStatusVendaHandler : IRequestHandler<AtualizarStatusVendaRequest, ResponseBase>
    {
        public readonly IVendaService _vendaService;
        private readonly IValidator<AtualizarStatusVendaRequest> _validator;

        public AtualizarStatusVendaHandler(IVendaService vendaService, IValidator<AtualizarStatusVendaRequest> validator)
        {
            _vendaService = vendaService;
            _validator = validator;
        }

        public async Task<ResponseBase> Handle(AtualizarStatusVendaRequest request, CancellationToken ct)
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

                var venda = await _vendaService.ObterPorIdAsync(request.VendaId, ct);

                venda.StatusVenda = request.StatusVenda;
                await _vendaService.AlterarAsync(venda, ct);

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
