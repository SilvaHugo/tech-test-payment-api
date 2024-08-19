﻿using PottencialTechTest.Data.Infra.Contexto;
using PottencialTechTest.Data.Infra.Repositories.Base;
using PottencialTechTest.Domain.Entidades;
using PottencialTechTest.Domain.Interfaces.Repositorios;

namespace PottencialTechTest.Data.Infra.Repositories
{
    public class VendedorRepository : RepositoryBase<Vendedor>, IVendedorRepository
    {
        private readonly PottencialContexto _context;
        public VendedorRepository(PottencialContexto context) : base(context)
        {
            _context = context;
        }
    }
}