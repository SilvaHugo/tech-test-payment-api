using Microsoft.EntityFrameworkCore;
using PottencialTechTest.Data.Infra.Contexto;
using PottencialTechTest.Data.Infra.Repositories.Base;
using PottencialTechTest.Domain.Entidades;
using PottencialTechTest.Domain.Interfaces.Repositorios;

namespace PottencialTechTest.Data.Infra.Repositories
{
    public class VendaRepository : RepositoryBase<Venda>, IVendaRepository
    {
        private readonly PottencialContexto _context;

        public VendaRepository(PottencialContexto context) : base(context)
        {
            _context = context;
        }

        public async Task<Venda> ObterVendaComProdutosEVendedor(Guid vendaId, CancellationToken cancellationToken = default)
            => await _context.Vendas
                .Include(p => p.Produtos)
                .Include(v => v.Vendedor)
                .FirstOrDefaultAsync(v => v.Id == vendaId, cancellationToken);

    }
}
