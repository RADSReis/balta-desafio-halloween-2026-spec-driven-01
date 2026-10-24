using System.Net;
using System.Net.Http.Json;
using GerenciarSenhas.Api.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace GerenciarSenhas.IntegrationTests;

public class SenhaEndpointsTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Post_GeraSenhaValida()
    {
        var resposta = await _client.PostAsync("/senhas", null);

        Assert.Equal(HttpStatusCode.Created, resposta.StatusCode);
        var corpo = await resposta.Content.ReadFromJsonAsync<SenhaResponse>();
        Assert.NotNull(corpo);
        Assert.NotEqual(Guid.Empty, corpo.Id);
        Assert.InRange(corpo.Senha.Length, 16, 32);
        Assert.DoesNotContain(corpo.Senha, char.IsWhiteSpace);
        Assert.Equal($"/senhas/{corpo.Id}", resposta.Headers.Location?.OriginalString);
    }

    [Fact]
    public async Task Post_PersisteSenhaNaTabelaSenhas()
    {
        var resposta = await _client.PostAsync("/senhas", null);
        var corpo = await resposta.Content.ReadFromJsonAsync<SenhaResponse>();

        await using var conexao = new SqliteConnection($"Data Source={factory.CaminhoBanco}");
        await conexao.OpenAsync();
        var comando = conexao.CreateCommand();
        comando.CommandText = "SELECT Senha FROM Senhas WHERE upper(Id) = upper($id)";
        comando.Parameters.AddWithValue("$id", corpo!.Id.ToString());

        Assert.Equal(corpo.Senha, await comando.ExecuteScalarAsync());
    }

    [Fact]
    public async Task Post_DepoisGet_RetornaMesmaSenha()
    {
        var gerada = await (await _client.PostAsync("/senhas", null)).Content.ReadFromJsonAsync<SenhaResponse>();

        var resposta = await _client.GetAsync($"/senhas/{gerada!.Id}");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        var recuperada = await resposta.Content.ReadFromJsonAsync<SenhaResponse>();
        Assert.Equal(gerada, recuperada);
    }

    [Fact]
    public async Task Get_GuidInvalido_Retorna400ComProblemDetails()
    {
        var resposta = await _client.GetAsync("/senhas/nao-e-um-guid");

        Assert.Equal(HttpStatusCode.BadRequest, resposta.StatusCode);
        Assert.Equal("application/problem+json", resposta.Content.Headers.ContentType?.MediaType);
        var problema = await resposta.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.Equal(400, problema!.Status);
        Assert.True(problema.Errors.ContainsKey("id"));
    }

    [Fact]
    public async Task Get_RegistroInexistente_Retorna404ComProblemDetails()
    {
        var resposta = await _client.GetAsync($"/senhas/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, resposta.StatusCode);
        Assert.Equal("application/problem+json", resposta.Content.Headers.ContentType?.MediaType);
        var problema = await resposta.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.Equal(404, problema!.Status);
    }
}
