using System.Security.Cryptography;

namespace GerenciarSenhas.Application.Senhas;

/// <summary>
/// Gera senhas com <see cref="RandomNumberGenerator"/> (fonte criptograficamente segura).
/// </summary>
public sealed class GeradorSenha : IGeradorSenha
{
    public const int TamanhoMinimo = 16;
    public const int TamanhoMaximo = 32;

    // Caracteres ASCII imprimíveis, sem espaço (0x21 a 0x7E).
    public static readonly string CaracteresPermitidos =
        new(Enumerable.Range('!', '~' - '!' + 1).Select(c => (char)c).ToArray());

    public string Gerar()
    {
        var tamanho = RandomNumberGenerator.GetInt32(TamanhoMinimo, TamanhoMaximo + 1);
        return RandomNumberGenerator.GetString(CaracteresPermitidos, tamanho);
    }
}
