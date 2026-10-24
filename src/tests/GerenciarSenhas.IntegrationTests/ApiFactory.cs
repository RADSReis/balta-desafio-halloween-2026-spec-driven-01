using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;

namespace GerenciarSenhas.IntegrationTests;

/// <summary>
/// Sobe a API com um banco SQLite isolado em um diretório temporário.
/// </summary>
public sealed class ApiFactory : WebApplicationFactory<Program>
{
    public string DiretorioBanco { get; } =
        Path.Combine(Path.GetTempPath(), "GerenciarSenhas.Tests", Guid.NewGuid().ToString("N"));

    public string CaminhoBanco => Path.Combine(DiretorioBanco, "SenhasFortes.db");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("Database:Directory", DiretorioBanco);
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        SqliteConnection.ClearAllPools();
        if (Directory.Exists(DiretorioBanco))
            Directory.Delete(DiretorioBanco, recursive: true);
    }
}
