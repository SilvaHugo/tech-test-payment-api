using PottencialTechTest.Domain.Entidades;
using PottencialTechTest.Domain.Interfaces.Servicos.Base;

namespace PottencialTechTest.Domain.Interfaces.Servicos
{
    public interface IVendedorService : IServiceBase<Vendedor>
    {
        Task<bool> VerificarVendedorValido(Guid vendedorId, CancellationToken cancellationToken = default);
    }
}
