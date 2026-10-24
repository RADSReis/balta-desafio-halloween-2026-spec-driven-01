using Microsoft.Data.Sqlite;

namespace GerenciarSenhas.IntegrationTests;

public class BancoDeDadosTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    [Fact]
    public async Task Inicializacao_CriaBancoSenhasFortesComTabelaSenhas()
    {
        _ = factory.CreateClient();

        Assert.True(File.Exists(factory.CaminhoBanco));

        await using var conexao = new SqliteConnection($"Data Source={factory.CaminhoBanco}");
        await conexao.OpenAsync();
        var comando = conexao.CreateCommand();
        comando.CommandText = "SELECT name, type, \"notnull\", pk FROM pragma_table_info('Senhas') ORDER BY cid";

        var colunas = new List<(string Nome, string Tipo, bool NotNull, bool Pk)>();
        await using var leitor = await comando.ExecuteReaderAsync();
        while (await leitor.ReadAsync())
            colunas.Add((leitor.GetString(0), leitor.GetString(1), leitor.GetBoolean(2), leitor.GetBoolean(3)));

        Assert.Equal(2, colunas.Count);
        Assert.Contains(("Id", "TEXT", true, true), colunas);
        Assert.Contains(("Senha", "TEXT", true, false), colunas);
    }
}
