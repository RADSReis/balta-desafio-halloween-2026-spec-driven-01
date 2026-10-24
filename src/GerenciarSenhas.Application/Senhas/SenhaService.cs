namespace GerenciarSenhas.Application.Senhas;

public sealed class SenhaService(IGeradorSenha gerador, ISenhaRepository repository)
{
    public async Task<Senha> GerarAsync(CancellationToken cancellationToken = default)
    {
        var senha = new Senha(Guid.NewGuid(), gerador.Gerar());
        await repository.AdicionarAsync(senha, cancellationToken);
        return senha;
    }

    public async Task<ObterSenhaResultado> ObterAsync(string? id, CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(id, out var guid))
            return new ObterSenhaResultado(ObterSenhaStatus.IdInvalido);

        var senha = await repository.ObterPorIdAsync(guid, cancellationToken);
        return senha is null
            ? new ObterSenhaResultado(ObterSenhaStatus.NaoEncontrada)
            : new ObterSenhaResultado(ObterSenhaStatus.Encontrada, senha);
    }
}
