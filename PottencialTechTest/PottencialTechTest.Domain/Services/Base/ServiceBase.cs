using PottencialTechTest.Domain.Interfaces.Repositorios.Base;
using PottencialTechTest.Domain.Interfaces.Servicos.Base;
using System.Linq.Expressions;

namespace PottencialTechTest.Domain.Services.Base
{
    public class ServiceBase<TEntidade> : IDisposable, IServiceBase<TEntidade> where TEntidade : class
    {
        private readonly IRepositoryBase<TEntidade> _repositorioBase;

        public ServiceBase(IRepositoryBase<TEntidade> repositorioBase)
        {
            _repositorioBase = repositorioBase;
        }

        public async Task AlterarAsync(TEntidade obj, CancellationToken cancellationToken = default)
        {
            await _repositorioBase.AlterarAsync(obj, cancellationToken);
        }

        public async Task InserirAsync(TEntidade obj, CancellationToken cancellationToken = default)
        {
            await _repositorioBase.InserirAsync(obj, cancellationToken);
        }

        public async Task<TEntidade> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _repositorioBase.ObterPorIdAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<TEntidade>> ObterTodosAsync(CancellationToken cancellationToken = default)
        {
            return await _repositorioBase.ObterTodosAsync(cancellationToken);
        }

        public async Task RemoverAsync(TEntidade obj, CancellationToken cancellationToken = default)
        {
            await _repositorioBase.RemoverAsync(obj, cancellationToken);
        }

        public async Task RemoverListaAsync(List<TEntidade> obj, CancellationToken cancellationToken = default)
        {
            await _repositorioBase.RemoverListaAsync(obj, cancellationToken);
        }

        public async Task<IEnumerable<TEntidade>> ListarPorAsync(Expression<Func<TEntidade, bool>> where, CancellationToken cancellationToken = default)
        {
            return await _repositorioBase.ListarPorAsync(where, cancellationToken);
        }

        public void Dispose()
        {
            _repositorioBase.Dispose();
        }

    }
}
