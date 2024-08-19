using PottencialTechTest.Domain.Entidades;
using PottencialTechTest.Domain.Interfaces.Repositorios.Base;

namespace PottencialTechTest.Domain.Interfaces.Repositorios
{
    public interface IVendaRepository : IRepositoryBase<Venda>
    {
        Task<Venda> ObterVendaComProdutosEVendedor(Guid vendaId, CancellationToken cancellationToken = default);
    }
}
