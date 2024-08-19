using PottencialTechTest.Domain.Entidades;
using PottencialTechTest.Domain.Interfaces.Repositorios;
using PottencialTechTest.Domain.Interfaces.Servicos;
using PottencialTechTest.Domain.Services.Base;

namespace PottencialTechTest.Domain.Services
{
    public class VendaService : ServiceBase<Venda>, IVendaService
    {
        private readonly IVendaRepository _vendaRepository;
        public VendaService(IVendaRepository vendaRepository)
            : base(vendaRepository)
        {
            _vendaRepository = vendaRepository;
        }

        public async Task<Venda> ObterVendaComProdutos(Guid vendaId, CancellationToken cancellationToken = default) => await _vendaRepository.ObterVendaComProdutosEVendedor(vendaId, cancellationToken);

    }
}
