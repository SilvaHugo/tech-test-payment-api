using FluentValidation;
using FluentValidation.Results;
using Moq;
using PottencialTechTest.App.Api.Produtos.RemoverProdutos.Dto.Request;
using PottencialTechTest.App.Api.Produtos.RemoverProdutos.Handler;
using PottencialTechTest.Domain.Entidades;
using PottencialTechTest.Domain.Interfaces.Servicos;
using System.Linq.Expressions;
using Assert = Xunit.Assert;

namespace PottencialTechTest.UnitTests.Produtos.RemoverProdutos
{
    [TestClass]
    public class RemoverProdutosHandlerTests
    {
        private readonly Mock<IProdutoService> _mockProdutoService;
        private readonly Mock<IVendaService> _mockVendaService;
        private readonly Mock<IValidator<RemoverProdutosRequest>> _mockValidator;

        public RemoverProdutosHandlerTests()
        {
            _mockProdutoService = new Mock<IProdutoService>();
            _mockVendaService = new Mock<IVendaService>();
            _mockValidator = new Mock<IValidator<RemoverProdutosRequest>>();
        }

        [TestMethod]
        public async Task Handle_FalhaFluentValidation_ValidacaoRequestRetornaFalha()
        {
            // Arrange
            var request = new RemoverProdutosRequest
            {
                IdsProdutos = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
            };

            var validationResult = new ValidationResult(new List<FluentValidation.Results.ValidationFailure>
            {
                new ValidationFailure("IdsProdutos", "A remoção dos produtos deixará a venda sem produtos.")
            });

            _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            var handler = new RemoverProdutosHandler(_mockProdutoService.Object, _mockVendaService.Object, _mockValidator.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            _mockProdutoService.Verify(ps => ps.RemoverListaAsync(It.IsAny<List<Produto>>(), It.IsAny<CancellationToken>()), Times.Never);
            Assert.False(result.Sucesso);
            Assert.NotNull(result.Body);
            Assert.Single(result.Body as List<ValidationFailure>);
        }

        [TestMethod]
        public async Task Handle_RequestValido_RemoveProdutosRetornaSucesso()
        {
            // Arrange
            var request = new RemoverProdutosRequest
            {
                IdsProdutos = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
            };

            var listaProdutos = new List<Produto>
            {
                new Produto { Id = Guid.NewGuid(), NomeProduto = "Produto 1", ValorProduto = 10 },
                new Produto { Id = Guid.NewGuid(), NomeProduto = "Produto 2", ValorProduto = 20 },
                new Produto { Id = Guid.NewGuid(), NomeProduto = "Produto 3", ValorProduto = 30 }
            };

            _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _mockProdutoService
                .Setup(p => p.ListarPorAsync(It.IsAny<Expression<Func<Produto, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaProdutos);

            var handler = new RemoverProdutosHandler(_mockProdutoService.Object, _mockVendaService.Object, _mockValidator.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            _mockProdutoService.Verify(ps => ps.RemoverListaAsync(It.IsAny<List<Produto>>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.True(result.Sucesso);
        }

        [TestMethod]
        public async Task Handle_Exception_RetornaMensagemErro()
        {
            // Arrange
            var request = new RemoverProdutosRequest
            {
                IdsProdutos = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
            };

            _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _mockProdutoService
                .Setup(p => p.ListarPorAsync(It.IsAny<Expression<Func<Produto, bool>>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro inesperado"));

            var handler = new RemoverProdutosHandler(_mockProdutoService.Object, _mockVendaService.Object, _mockValidator.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Sucesso);
            Assert.Equal("A operação não pôde ser concluída. Por favor, tente novamente mais tarde.", result.Mensagem);
        }
    }
}
