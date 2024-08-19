using PottencialTechTest.Domain.Entidades;
using PottencialTechTest.Domain.Interfaces.Repositorios;
using PottencialTechTest.Domain.Interfaces.Servicos;
using PottencialTechTest.Domain.Services.Base;

namespace PottencialTechTest.Domain.Services
{
    public class VendedorService : ServiceBase<Vendedor>, IVendedorService
    {
        private readonly IVendedorRepository _vendedorRepository;
        public VendedorService(IVendedorRepository vendedorRepository)
            : base(vendedorRepository)
        {
            _vendedorRepository = vendedorRepository;
        }

        public async Task<bool> VerificarVendedorValido(Guid vendedorId, CancellationToken cancellationToken = default) => await _vendedorRepository.ObterPorIdAsync(vendedorId, cancellationToken) is not null;
    }
}
