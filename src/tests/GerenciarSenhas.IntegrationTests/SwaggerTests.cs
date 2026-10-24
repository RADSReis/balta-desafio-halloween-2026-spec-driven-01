using System.Net;

namespace GerenciarSenhas.IntegrationTests;

public class SwaggerTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task SwaggerJson_DocumentaEndpointsDeSenhas()
    {
        var resposta = await _client.GetAsync("/swagger/v1/swagger.json");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        var documento = await resposta.Content.ReadAsStringAsync();
        Assert.Contains("\"/senhas\"", documento);
        Assert.Contains("\"/senhas/{id}\"", documento);
    }

    [Fact]
    public async Task SwaggerUi_EstaAcessivel()
    {
        var resposta = await _client.GetAsync("/swagger/index.html");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
    }
}
