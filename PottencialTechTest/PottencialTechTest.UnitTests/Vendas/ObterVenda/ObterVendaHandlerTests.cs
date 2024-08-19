using FluentValidation;
using Moq;
using PottencialTechTest.App.Api.Vendas.ObterVenda.Dto.Request;
using PottencialTechTest.App.Api.Vendas.ObterVenda.Handler;
using PottencialTechTest.Domain.Entidades;
using PottencialTechTest.Domain.Interfaces.Servicos;
using PottencialTechTest.Domain.Shared.Enum;
using Assert = Xunit.Assert;

namespace PottencialTechTest.UnitTests.Vendas.ObterVenda
{
    [TestClass]
    public class ObterVendaHandlerTests
    {
        private readonly Mock<IVendaService> _mockVendaService;
        private readonly Mock<IValidator<ObterVendaRequest>> _mockValidator;

        public ObterVendaHandlerTests()
        {
            _mockVendaService = new Mock<IVendaService>();
            _mockValidator = new Mock<IValidator<ObterVendaRequest>>();
        }

        [TestMethod]
        public async Task Handle_FalhaFluentValidation_ValidacaoRequestRetornaFalha()
        {
            // Arrange
            var request = new ObterVendaRequest
            {
                VendaId = Guid.Empty
            };

            var validationResult = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("VendaId", "O ID da venda é obrigatório.")
            });

            _mockValidator
                .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            var handler = new ObterVendaHandler(_mockVendaService.Object, _mockValidator.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Sucesso);
            Assert.NotNull(result.Body);
            Assert.Single(result.Body as List<FluentValidation.Results.ValidationFailure>);
            _mockVendaService.Verify(vs => vs.ObterVendaComProdutos(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [TestMethod]
        public async Task Handle_VendaNaoEncontrada_RetornaMensagemVendaNaoEncontrada()
        {
            // Arrange
            var request = new ObterVendaRequest
            {
                VendaId = Guid.NewGuid()
            };

            _mockValidator
                .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _mockVendaService
                .Setup(vs => vs.ObterVendaComProdutos(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Venda)null);

            var handler = new ObterVendaHandler(_mockVendaService.Object, _mockValidator.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Sucesso);
            Assert.Equal("Venda não encontrada.", result.Mensagem);
        }

        [TestMethod]
        public async Task Handle_RequestValido_RetornaVenda()
        {
            // Arrange
            var request = new ObterVendaRequest
            {
                VendaId = Guid.NewGuid()
            };

            var venda = new Venda { Id = Guid.NewGuid(), StatusVenda = StatusVenda.AguardandoPagamento };

            _mockValidator
                .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _mockVendaService.Setup(vs => vs.ObterVendaComProdutos(request.VendaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(venda);

            var handler = new ObterVendaHandler(_mockVendaService.Object, _mockValidator.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            _mockVendaService.Verify(vs => vs.ObterVendaComProdutos(request.VendaId, It.IsAny<CancellationToken>()), Times.Once);
            Assert.True(result.Sucesso);
            Assert.NotNull(result.Body);
            Assert.IsType<Venda>(result.Body);
            Assert.Equal(venda, result.Body);
        }

        [TestMethod]
        public async Task Handle_Exception_RetornaMensagemErro()
        {
            // Arrange
            var request = new ObterVendaRequest
            {
                VendaId = Guid.NewGuid()
            };

            _mockValidator
                .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _mockVendaService.Setup(vs => vs.ObterVendaComProdutos(request.VendaId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Erro inesperado"));

            var handler = new ObterVendaHandler(_mockVendaService.Object, _mockValidator.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Sucesso);
            Assert.Equal("A operação não pôde ser concluída. Por favor, tente novamente mais tarde.", result.Mensagem);
        }
    }
}
