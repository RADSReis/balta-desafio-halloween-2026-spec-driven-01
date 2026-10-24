using GerenciarSenhas.Application.Senhas;
using GerenciarSenhas.Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GerenciarSenhas.Infrastructure;

public static class DependencyInjection
{
    public const string NomeBanco = "SenhasFortes";
    private const string ChaveDiretorioBanco = "Database:Directory";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<SenhasDbContext>((provider, options) =>
            options.UseSqlite($"Data Source={ObterCaminhoBanco(provider)}"));
        services.AddScoped<ISenhaRepository, SenhaRepository>();
        return services;
    }

    /// <summary>
    /// Cria o diretório e o banco SQLite (com a tabela Senhas) caso ainda não existam.
    /// </summary>
    public static async Task InicializarBancoAsync(this IServiceProvider services)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(ObterCaminhoBanco(services))!);

        await using var scope = services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<SenhasDbContext>();
        await context.Database.EnsureCreatedAsync();
    }

    // O diretório vem da configuração; caminhos relativos são resolvidos a partir do ContentRoot.
    public static string ObterCaminhoBanco(IServiceProvider provider)
    {
        var configuration = provider.GetRequiredService<IConfiguration>();
        var environment = provider.GetRequiredService<IHostEnvironment>();
        var diretorio = configuration[ChaveDiretorioBanco] ?? "App_Data";
        return Path.Combine(environment.ContentRootPath, diretorio, $"{NomeBanco}.db");
    }
}
