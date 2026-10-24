namespace GerenciarSenhas.Application.Senhas;

public interface ISenhaRepository
{
    Task AdicionarAsync(Senha senha, CancellationToken cancellationToken = default);
    Task<Senha?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
}
