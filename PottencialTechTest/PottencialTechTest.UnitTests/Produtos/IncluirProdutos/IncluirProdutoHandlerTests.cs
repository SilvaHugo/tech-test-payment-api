using FluentValidation;
using FluentValidation.Results;
using Moq;
using PottencialTechTest.App.Api.Produtos.IncluirProdutos.Dto.Request;
using PottencialTechTest.App.Api.Produtos.IncluirProdutos.Handler;
using PottencialTechTest.Domain.Arguments.Produto;
using PottencialTechTest.Domain.Entidades;
using PottencialTechTest.Domain.Interfaces.Servicos;
using Assert = Xunit.Assert;

namespace PottencialTechTest.UnitTests.Produtos.IncluirProdutos
{
    [TestClass]
    public class IncluirProdutoHandlerTests
    {
        private readonly Mock<IProdutoService> _mockProdutoService;
        private readonly Mock<IVendaService> _mockVendaService;
        private readonly Mock<IValidator<IncluirProdutoRequest>> _mockValidator;

        public IncluirProdutoHandlerTests()
        {
            _mockProdutoService = new Mock<IProdutoService>();
            _mockVendaService = new Mock<IVendaService>();
            _mockValidator = new Mock<IValidator<IncluirProdutoRequest>>();
        }

        [TestMethod]
        public async Task Handle_FalhaFluentValidation_ValidacaoRequestRetornaFalha()
        {
            // Arrange
            var request = new IncluirProdutoRequest
            {
                Produtos = new List<ProdutoDto> { new ProdutoDto { NomeProduto = string.Empty, ValorProduto = 10 } },
                VendaId = Guid.NewGuid()
            };

            var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("NomeProduto", "Existem produtos inválidos na lista.")
            });

            _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            var handler = new IncluirProdutoHandler(_mockProdutoService.Object, _mockVendaService.Object, _mockValidator.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            _mockProdutoService.Verify(ps => ps.InserirAsync(It.IsAny<Produto>(), It.IsAny<CancellationToken>()), Times.Never);
            Assert.False(result.Sucesso);
            Assert.NotNull(result.Body);
            Assert.Single(result.Body as List<ValidationFailure>);
        }

        [TestMethod]
        public async Task Handle_RequestValido_InsereProdutosRetornaSucesso()
        {
            // Arrange
            var request = new IncluirProdutoRequest
            {
                Produtos = new List<ProdutoDto>
            {
                new ProdutoDto { NomeProduto = "Produto 1", ValorProduto = 10 },
                new ProdutoDto { NomeProduto = "Produto 2", ValorProduto = 20 }
            },
                VendaId = Guid.NewGuid()
            };

            _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            var handler = new IncluirProdutoHandler(_mockProdutoService.Object, _mockVendaService.Object, _mockValidator.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            _mockProdutoService.Verify(ps => ps.InserirAsync(It.IsAny<Produto>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
            Assert.True(result.Sucesso);
            Assert.Equal(2, (result.Body as List<Produto>)?.Count);
        }

        [TestMethod]
        public async Task Handle_Exception_RetornaMensagemErro()
        {
            // Arrange
            var request = new IncluirProdutoRequest
            {
                Produtos = new List<ProdutoDto> { new ProdutoDto { NomeProduto = "Produto Teste", ValorProduto = 20 } },
                VendaId = Guid.NewGuid()
            };

            _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _mockProdutoService.Setup(ps => ps.InserirAsync(It.IsAny<Produto>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro inesperado"));

            var handler = new IncluirProdutoHandler(_mockProdutoService.Object, _mockVendaService.Object, _mockValidator.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Sucesso);
            Assert.Equal("A operação não pôde ser concluída. Por favor, tente novamente mais tarde.", result.Mensagem);
        }
    }
}
