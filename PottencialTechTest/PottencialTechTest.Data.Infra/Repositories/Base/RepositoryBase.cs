using Microsoft.EntityFrameworkCore;
using PottencialTechTest.Domain.Entidades.Base;
using PottencialTechTest.Domain.Interfaces.Repositorios.Base;
using System.Linq.Expressions;

namespace PottencialTechTest.Data.Infra.Repositories.Base
{
    public class RepositoryBase<TEntidade> : IDisposable, IRepositoryBase<TEntidade>
        where TEntidade : EntidadeBase
    {
        protected DbContext Context { get; set; }

        public RepositoryBase(DbContext context)
        {
            Context = context;
        }

        public async Task AlterarAsync(TEntidade obj, CancellationToken cancellationToken = default)
        {
            Context.Set<TEntidade>().Update(obj);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public async Task InserirAsync(TEntidade obj, CancellationToken cancellationToken = default)
        {
            await Context.Set<TEntidade>().AddAsync(obj);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task<TEntidade> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) => await Context.Set<TEntidade>().FindAsync(id, cancellationToken);


        public async Task<IEnumerable<TEntidade>> ObterTodosAsync(CancellationToken cancellationToken = default) => await Context.Set<TEntidade>().AsNoTracking().ToListAsync(cancellationToken);


        public async Task RemoverAsync(TEntidade obj, CancellationToken cancellationToken = default)
        {
            Context.Set<TEntidade>().Remove(obj);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoverListaAsync(List<TEntidade> obj, CancellationToken cancellationToken = default)
        {
            Context.Set<TEntidade>().RemoveRange(obj);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<TEntidade>> ListarPorAsync(Expression<Func<TEntidade, bool>> where, CancellationToken cancellationToken = default)
            => await Context.Set<TEntidade>()
                                .Where(where)
                                .AsNoTracking()
                                .ToListAsync(cancellationToken);

    }
}
