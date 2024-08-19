using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PottencialTechTest.App.Api.Produtos.IncluirProdutos.Dto.Request;
using PottencialTechTest.App.Api.Produtos.IncluirProdutos.Handler;
using PottencialTechTest.App.Api.Produtos.IncluirProdutos.Validator;
using PottencialTechTest.App.Api.Produtos.RemoverProdutos.Dto.Request;
using PottencialTechTest.App.Api.Produtos.RemoverProdutos.Handler;
using PottencialTechTest.App.Api.Produtos.RemoverProdutos.Validator;
using PottencialTechTest.App.Api.Vendas.AtualizarStatusVenda.Dto.Request;
using PottencialTechTest.App.Api.Vendas.AtualizarStatusVenda.Handler;
using PottencialTechTest.App.Api.Vendas.AtualizarStatusVenda.Validator;
using PottencialTechTest.App.Api.Vendas.CancelarVenda.Dto.Request;
using PottencialTechTest.App.Api.Vendas.CancelarVenda.Handler;
using PottencialTechTest.App.Api.Vendas.CancelarVenda.Validator;
using PottencialTechTest.App.Api.Vendas.IncluirVenda.Dto.Request;
using PottencialTechTest.App.Api.Vendas.IncluirVenda.Handler;
using PottencialTechTest.App.Api.Vendas.IncluirVenda.Validator;
using PottencialTechTest.App.Api.Vendas.ObterVenda.Dto.Request;
using PottencialTechTest.App.Api.Vendas.ObterVenda.Handler;
using PottencialTechTest.App.Api.Vendas.ObterVenda.Validator;
using PottencialTechTest.Data.Infra.Contexto;
using PottencialTechTest.Data.Infra.Repositories;
using PottencialTechTest.Domain.Interfaces.Repositorios;
using PottencialTechTest.Domain.Interfaces.Servicos;
using PottencialTechTest.Domain.Services;
using PottencialTechTest.Domain.Shared.Response;
using PottencialTechTest.Infra.ApiKeyAuthentication;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = "ApiKeyAuthentication";
            options.DefaultChallengeScheme = "ApiKeyAuthentication";
        })
        .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>("ApiKeyAuthentication", null);

        // Adicionar Swagger e configurar API Key
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

            // Configuração para o header da API Key
            c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "API Key necessária para acessar os endpoints",
                Name = "X-Api-Key",
                Type = SecuritySchemeType.ApiKey
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "ApiKey"
                                }
                        },
                    Array.Empty<string>()
                }
            });
        });

        //DI DbContext
        services.AddDbContext<PottencialContexto>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("CNN_PottencialSeguradora")));
        services.AddHealthChecks()
            .AddDbContextCheck<PottencialContexto>("sql");

        // DI fluxo Venda
        services.AddScoped<IVendaRepository, VendaRepository>();
        services.AddScoped<IVendaService, VendaService>();

        services.AddTransient<IValidator<AtualizarStatusVendaRequest>, AtualizarStatusVendaValidator>();
        services.AddTransient<IRequestHandler<AtualizarStatusVendaRequest, ResponseBase>, AtualizarStatusVendaHandler>();

        services.AddTransient<IValidator<CancelarVendaRequest>, CancelarVendaValidator>();
        services.AddTransient<IRequestHandler<CancelarVendaRequest, ResponseBase>, CancelarVendaHandler>();

        services.AddTransient<IValidator<IncluirVendaRequest>, IncluirVendaValidator>();
        services.AddTransient<IRequestHandler<IncluirVendaRequest, ResponseBase>, IncluirVendaHandler>();

        services.AddTransient<IValidator<ObterVendaRequest>, ObterVendaValidator>();
        services.AddTransient<IRequestHandler<ObterVendaRequest, ResponseBase>, ObterVendaHandler>();
        
        // DI fluxo Produto
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IProdutoService, ProdutoService>();

        services.AddTransient<IValidator<RemoverProdutosRequest>, RemoverProdutosValidator>();
        services.AddTransient<IRequestHandler<RemoverProdutosRequest, ResponseBase>, RemoverProdutosHandler>();

        services.AddTransient<IValidator<IncluirProdutoRequest>, IncluirProdutoValidator>();
        services.AddTransient<IRequestHandler<IncluirProdutoRequest, ResponseBase>, IncluirProdutoHandler>();

        // DI fluxo Vendedor
        services.AddScoped<IVendedorRepository, VendedorRepository>();
        services.AddScoped<IVendedorService, VendedorService>();

        // Configurar o MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Startup).Assembly));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AtualizarStatusVendaHandler).Assembly));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CancelarVendaHandler).Assembly));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IncluirVendaHandler).Assembly));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ObterVendaHandler).Assembly));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IncluirProdutoHandler).Assembly));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RemoverProdutosHandler).Assembly));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHealthChecks("/health");
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}