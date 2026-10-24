namespace GerenciarSenhas.Application.Senhas;

public sealed class Senha
{
    public Guid Id { get; private set; }
    public string Valor { get; private set; } = string.Empty;

    private Senha() { }

    public Senha(Guid id, string valor)
    {
        Id = id;
        Valor = valor;
    }
}
