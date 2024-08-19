using FluentValidation;
using Moq;
using PottencialTechTest.App.Api.Vendas.IncluirVenda.Dto.Request;
using PottencialTechTest.App.Api.Vendas.IncluirVenda.Handler;
using PottencialTechTest.Domain.Arguments.Produto;
using PottencialTechTest.Domain.Entidades;
using PottencialTechTest.Domain.Interfaces.Servicos;
using PottencialTechTest.Domain.Shared.Enum;
using Assert = Xunit.Assert;

namespace PottencialTechTest.UnitTests.Vendas.IncluirVenda
{
    [TestClass]
    public class IncluirVendaHandlerTests
    {
        private readonly Mock<IVendaService> _mockVendaService;
        private readonly Mock<IVendedorService> _mockVendedorService;
        private readonly Mock<IValidator<IncluirVendaRequest>> _mockValidator;

        public IncluirVendaHandlerTests()
        {
            _mockVendaService = new Mock<IVendaService>();
            _mockVendedorService = new Mock<IVendedorService>();
            _mockValidator = new Mock<IValidator<IncluirVendaRequest>>();
        }

        [TestMethod]
        public async Task Handle_FalhaFluentValidation_ValidacaoRequestRetornaFalha()
        {
            // Arrange
            var request = new IncluirVendaRequest
            {
                DataVenda = DateTime.Now,
                VendedorId = Guid.NewGuid(),
                Produtos = new List<ProdutoDto>
                {
                    new ProdutoDto { NomeProduto = "Produto1", ValorProduto = 10.0m }
                }
            };

            var validationResult =
                new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure>
                {
                    new FluentValidation.Results.ValidationFailure("VendedorId", "O vendedor com o ID informado não existe.")
                });

            _mockValidator
                .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            var handler = new IncluirVendaHandler(_mockVendaService.Object, _mockVendedorService.Object, _mockValidator.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Sucesso);
            Assert.NotNull(result.Body);
            Assert.Single(result.Body as List<FluentValidation.Results.ValidationFailure>);
            _mockVendaService.Verify(vs => vs.InserirAsync(It.IsAny<Venda>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [TestMethod]
        public async Task Handle_RequestValido_InsereVendaRetornaSucesso()
        {
            // Arrange
            var request = new IncluirVendaRequest
            {
                DataVenda = DateTime.Now,
                VendedorId = Guid.NewGuid(),
                Produtos = new List<ProdutoDto>
                {
                    new ProdutoDto { NomeProduto = "Produto1", ValorProduto = 10.0m }
                }
            };

            _mockValidator
                .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            var handler = new IncluirVendaHandler(_mockVendaService.Object, _mockVendedorService.Object, _mockValidator.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            _mockVendaService
                .Verify(vs => vs.InserirAsync(It.Is<Venda>(
                    v =>
                    v.Produtos.Count == request.Produtos.Count &&
                    v.StatusVenda == StatusVenda.AguardandoPagamento), It.IsAny<CancellationToken>()),
                Times.Once);

            Assert.True(result.Sucesso);
            Assert.NotNull(result.Body);
            Assert.IsType<Venda>(result.Body);
            Assert.Equal(StatusVenda.AguardandoPagamento, ((Venda)result.Body).StatusVenda);
        }

        [TestMethod]
        public async Task Handle_Exception_RetornaMensagemErro()
        {
            // Arrange
            var request = new IncluirVendaRequest
            {
                DataVenda = DateTime.Now,
                VendedorId = Guid.NewGuid(),
                Produtos = new List<ProdutoDto>
                {
                    new ProdutoDto { NomeProduto = "Produto1", ValorProduto = 10.0m }
                }
            };

            _mockValidator
                .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _mockVendaService
                .Setup(vs => vs.InserirAsync(It.IsAny<Venda>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Erro inesperado"));

            var handler = new IncluirVendaHandler(_mockVendaService.Object, _mockVendedorService.Object, _mockValidator.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Sucesso);
            Assert.Equal("A operação não pôde ser concluída. Por favor, tente novamente mais tarde.", result.Mensagem);
        }

        [TestMethod]
        public void GerarIdentificadorVenda()
        {
            // Arrange & Act
            var identificador1 = IncluirVendaHandler.GerarIdentificadorVenda();
            var identificador2 = IncluirVendaHandler.GerarIdentificadorVenda();

            // Assert
            Assert.NotEqual(identificador1, identificador2);
            Assert.StartsWith("VENDA-", identificador1);
            Assert.StartsWith("VENDA-", identificador2);
            Assert.Equal(27, identificador1.Length); //"VENDA-yyyyMMddHHmmss-XXXXXX"
            Assert.Equal(27, identificador2.Length);
        }
    }
}
