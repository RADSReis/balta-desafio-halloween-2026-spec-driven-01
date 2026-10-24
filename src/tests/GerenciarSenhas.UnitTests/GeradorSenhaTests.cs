using GerenciarSenhas.Application.Senhas;

namespace GerenciarSenhas.UnitTests;

public class GeradorSenhaTests
{
    private const int Amostras = 1000;
    private readonly GeradorSenha _gerador = new();

    [Fact]
    public void Gerar_RespeitaTamanhoMinimoEMaximo()
    {
        for (var i = 0; i < Amostras; i++)
        {
            var senha = _gerador.Gerar();
            Assert.InRange(senha.Length, 16, 32);
        }
    }

    [Fact]
    public void Gerar_NaoContemEspacosEmBranco()
    {
        for (var i = 0; i < Amostras; i++)
            Assert.DoesNotContain(_gerador.Gerar(), char.IsWhiteSpace);
    }

    [Fact]
    public void Gerar_UtilizaApenasCaracteresPermitidos()
    {
        for (var i = 0; i < Amostras; i++)
            Assert.All(_gerador.Gerar(), c => Assert.Contains(c, GeradorSenha.CaracteresPermitidos));
    }

    [Fact]
    public void CaracteresPermitidos_NaoIncluemEspacosEmBranco()
    {
        Assert.DoesNotContain(GeradorSenha.CaracteresPermitidos, char.IsWhiteSpace);
    }

    [Fact]
    public void Gerar_ProduzTamanhosVariadosDentroDoIntervalo()
    {
        var tamanhos = Enumerable.Range(0, Amostras).Select(_ => _gerador.Gerar().Length).ToHashSet();

        Assert.Contains(16, tamanhos);
        Assert.Contains(32, tamanhos);
    }

    [Fact]
    public void Gerar_ProduzSenhasDistintas()
    {
        var senhas = Enumerable.Range(0, Amostras).Select(_ => _gerador.Gerar()).ToHashSet();

        Assert.Equal(Amostras, senhas.Count);
    }
}
