using FluentValidation;
using MediatR;
using PottencialTechTest.App.Api.Vendas.IncluirVenda.Dto.Request;
using PottencialTechTest.Domain.Entidades;
using PottencialTechTest.Domain.Interfaces.Servicos;
using PottencialTechTest.Domain.Shared.Enum;
using PottencialTechTest.Domain.Shared.Response;

namespace PottencialTechTest.App.Api.Vendas.IncluirVenda.Handler
{
    public class IncluirVendaHandler : IRequestHandler<IncluirVendaRequest, ResponseBase>
    {
        private readonly IVendaService _vendaService;
        private readonly IVendedorService _vendedorService;
        private readonly IValidator<IncluirVendaRequest> _validator;

        public IncluirVendaHandler(IVendaService vendaService, IVendedorService vendedorService, IValidator<IncluirVendaRequest> validator)
        {
            _vendaService = vendaService;
            _vendedorService = vendedorService;
            _validator = validator;
        }

        public async Task<ResponseBase> Handle(IncluirVendaRequest request, CancellationToken cancellationToken)
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

                var venda = new Venda()
                {
                    DataVenda = request.DataVenda,
                    VendedorId = request.VendedorId,
                    Identificador = GerarIdentificadorVenda(),
                    Produtos = new List<Produto>(),
                    StatusVenda = StatusVenda.AguardandoPagamento
                };

                foreach (var produtoDto in request.Produtos)
                {
                    var produto = new Produto()
                    {
                        NomeProduto = produtoDto.NomeProduto,
                        ValorProduto = produtoDto.ValorProduto
                    };
                    venda.Produtos.Add(produto);
                }

                await _vendaService.InserirAsync(venda, cancellationToken);
                response.SetSucesso(venda);
            }
            catch
            {
                response.Mensagem = "A operação não pôde ser concluída. Por favor, tente novamente mais tarde.";
            }

            return response;
        }

        public static string GerarIdentificadorVenda()
        {
            // Defina o prefixo para identificar o tipo de venda
            const string prefixo = "VENDA-";

            // Use a data e hora atual para garantir a unicidade
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

            // Gere um valor aleatório para adicionar mais variabilidade
            var valorAleatorio = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper(); // 6 caracteres aleatórios

            return $"{prefixo}{timestamp}-{valorAleatorio}";
        }
    }
}
