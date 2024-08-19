using FluentValidation;
using MediatR;
using PottencialTechTest.App.Api.Vendas.ObterVenda.Dto.Request;
using PottencialTechTest.Domain.Interfaces.Servicos;
using PottencialTechTest.Domain.Shared.Response;

namespace PottencialTechTest.App.Api.Vendas.ObterVenda.Handler
{
    public class ObterVendaHandler : IRequestHandler<ObterVendaRequest, ResponseBase>
    {
        private readonly IVendaService _vendaService;
        private readonly IValidator<ObterVendaRequest> _validator;

        public ObterVendaHandler(IVendaService vendaService, IValidator<ObterVendaRequest> validator)
        {
            _vendaService = vendaService;
            _validator = validator;
        }

        public async Task<ResponseBase> Handle(ObterVendaRequest request, CancellationToken cancellationToken)
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

                var venda = await _vendaService.ObterVendaComProdutos(request.VendaId, cancellationToken);

                if (venda == null)
                {
                    response.Mensagem = "Venda não encontrada.";
                    return response;
                }

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
