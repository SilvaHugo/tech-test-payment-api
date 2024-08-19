using System.Linq.Expressions;

namespace PottencialTechTest.Domain.Interfaces.Servicos.Base
{
    public interface IServiceBase<TEntidade> where TEntidade : class
    {
        Task InserirAsync(TEntidade obj, CancellationToken cancellationToken = default);
        Task<TEntidade> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntidade>> ObterTodosAsync(CancellationToken cancellationToken = default);
        Task AlterarAsync(TEntidade obj, CancellationToken cancellationToken = default);
        Task RemoverAsync(TEntidade obj, CancellationToken cancellationToken = default);
        void Dispose();
        Task RemoverListaAsync(List<TEntidade> obj, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntidade>> ListarPorAsync(Expression<Func<TEntidade, bool>> where, CancellationToken cancellationToken = default);
    }
}
