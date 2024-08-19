﻿using FluentValidation;
using Moq;
using PottencialTechTest.App.Api.Vendas.AtualizarStatusVenda.Dto.Request;
using PottencialTechTest.App.Api.Vendas.AtualizarStatusVenda.Handler;
using PottencialTechTest.Domain.Entidades;
using PottencialTechTest.Domain.Interfaces.Servicos;
using PottencialTechTest.Domain.Shared.Enum;
using Assert = Xunit.Assert;

namespace PottencialTechTest.UnitTests.Vendas.AtualizarStatusVenda
{
    [TestClass]
    public class AtualizarStatusVendaHandlerTests
    {
        public readonly Mock<IVendaService> _mockVendaService;
        private readonly Mock<IValidator<AtualizarStatusVendaRequest>> _mockValidator;

        public AtualizarStatusVendaHandlerTests()
        {
            _mockVendaService = new Mock<IVendaService>();
            _mockValidator = new Mock<IValidator<AtualizarStatusVendaRequest>>();
        }

        [TestMethod]
        public async Task Handle_FalhaFluentValidation_ValidacaoRequestRetornaFalha()
        {
            // Arrange
            var request = new AtualizarStatusVendaRequest
            {
                VendaId = Guid.NewGuid(),
                StatusVenda = StatusVenda.Cancelado
            };

            var validationResult = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("StatusVenda", "Para cancelar a venda, utilize a rota específica de cancelamento.")
            });

            _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            var handler = new AtualizarStatusVendaHandler(_mockVendaService.Object, _mockValidator.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            _mockVendaService.Verify(vs => vs.AlterarAsync(It.IsAny<Venda>(), It.IsAny<CancellationToken>()), Times.Never);
            Assert.False(result.Sucesso);
            Assert.NotNull(result.Body);
            Assert.Single(result.Body as List<FluentValidation.Results.ValidationFailure>);
        }

        [TestMethod]
        public async Task Handle_RequestValido_AtualizaStatusRetornaSucesso()
        {
            // Arrange
            var request = new AtualizarStatusVendaRequest
            {
                VendaId = Guid.NewGuid(),
                StatusVenda = StatusVenda.PagamentoAprovado
            };

            var venda = new Venda { Id = Guid.NewGuid(), StatusVenda = StatusVenda.AguardandoPagamento };

            _mockValidator
                .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _mockVendaService
                .Setup(vs => vs.ObterPorIdAsync(request.VendaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(venda);

            var handler = new AtualizarStatusVendaHandler(_mockVendaService.Object, _mockValidator.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            _mockVendaService.Verify(vs => vs.AlterarAsync(It.IsAny<Venda>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.True(result.Sucesso);
            Assert.Equal(result.Body, result.Body as Venda);
        }

        [TestMethod]
        public async Task Handle_Exception_RetornaMensagemErro()
        {
            // Arrange
            var request = new AtualizarStatusVendaRequest
            {
                VendaId = Guid.NewGuid(),
                StatusVenda = StatusVenda.PagamentoAprovado
            };

            _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _mockVendaService.Setup(vs => vs.ObterPorIdAsync(request.VendaId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Erro inesperado"));

            var handler = new AtualizarStatusVendaHandler(_mockVendaService.Object, _mockValidator.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Sucesso);
            Assert.Equal("A operação não pôde ser concluída. Por favor, tente novamente mais tarde.", result.Mensagem);
        }
    }
}
