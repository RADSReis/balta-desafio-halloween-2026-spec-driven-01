namespace GerenciarSenhas.Application.Senhas;

public enum ObterSenhaStatus
{
    Encontrada,
    IdInvalido,
    NaoEncontrada
}

public sealed record ObterSenhaResultado(ObterSenhaStatus Status, Senha? Senha = null);
