using PottencialTechTest.Domain.Entidades;
using PottencialTechTest.Domain.Interfaces.Repositorios;
using PottencialTechTest.Domain.Interfaces.Servicos;
using PottencialTechTest.Domain.Services.Base;

namespace PottencialTechTest.Domain.Services
{
    public class ProdutoService : ServiceBase<Produto>, IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        public ProdutoService(IProdutoRepository produtoRepository)
            : base(produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }
    }
}
