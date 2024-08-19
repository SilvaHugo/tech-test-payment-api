using PottencialTechTest.Domain.Entidades;
using PottencialTechTest.Domain.Interfaces.Servicos.Base;

namespace PottencialTechTest.Domain.Interfaces.Servicos
{
    public interface IVendaService : IServiceBase<Venda>
    {
        Task<Venda> ObterVendaComProdutos(Guid vendaId, CancellationToken cancellationToken = default);
    }
}
