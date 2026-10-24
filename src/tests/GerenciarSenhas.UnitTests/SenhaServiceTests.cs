using GerenciarSenhas.Application.Senhas;

namespace GerenciarSenhas.UnitTests;

public class SenhaServiceTests
{
    private sealed class GeradorFixo(string valor) : IGeradorSenha
    {
        public string Gerar() => valor;
    }

    private sealed class RepositorioEmMemoria : ISenhaRepository
    {
        public Dictionary<Guid, Senha> Senhas { get; } = [];

        public Task AdicionarAsync(Senha senha, CancellationToken cancellationToken = default)
        {
            Senhas.Add(senha.Id, senha);
            return Task.CompletedTask;
        }

        public Task<Senha?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            Task.FromResult(Senhas.GetValueOrDefault(id));
    }

    private const string SenhaGerada = "Abc123!@#Def456$%^";
    private readonly RepositorioEmMemoria _repositorio = new();
    private readonly SenhaService _service;

    public SenhaServiceTests()
    {
        _service = new SenhaService(new GeradorFixo(SenhaGerada), _repositorio);
    }

    [Fact]
    public async Task GerarAsync_PersisteSenhaGeradaComNovoGuid()
    {
        var senha = await _service.GerarAsync();

        Assert.NotEqual(Guid.Empty, senha.Id);
        Assert.Equal(SenhaGerada, senha.Valor);
        Assert.Same(senha, _repositorio.Senhas[senha.Id]);
    }

    [Fact]
    public async Task GerarAsync_GeraIdentificadoresDistintos()
    {
        var primeira = await _service.GerarAsync();
        var segunda = await _service.GerarAsync();

        Assert.NotEqual(primeira.Id, segunda.Id);
    }

    [Fact]
    public async Task ObterAsync_IdExistente_RetornaSenha()
    {
        var gerada = await _service.GerarAsync();

        var resultado = await _service.ObterAsync(gerada.Id.ToString());

        Assert.Equal(ObterSenhaStatus.Encontrada, resultado.Status);
        Assert.Equal(gerada.Id, resultado.Senha!.Id);
        Assert.Equal(SenhaGerada, resultado.Senha.Valor);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("nao-e-um-guid")]
    [InlineData("12345")]
    public async Task ObterAsync_IdInvalido_RetornaIdInvalido(string? id)
    {
        var resultado = await _service.ObterAsync(id);

        Assert.Equal(ObterSenhaStatus.IdInvalido, resultado.Status);
        Assert.Null(resultado.Senha);
    }

    [Fact]
    public async Task ObterAsync_IdInexistente_RetornaNaoEncontrada()
    {
        var resultado = await _service.ObterAsync(Guid.NewGuid().ToString());

        Assert.Equal(ObterSenhaStatus.NaoEncontrada, resultado.Status);
        Assert.Null(resultado.Senha);
    }
}
