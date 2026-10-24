using GerenciarSenhas.Application.Senhas;
using Microsoft.EntityFrameworkCore;

namespace GerenciarSenhas.Infrastructure.Persistencia;

public sealed class SenhaRepository(SenhasDbContext context) : ISenhaRepository
{
    public async Task AdicionarAsync(Senha senha, CancellationToken cancellationToken = default)
    {
        context.Senhas.Add(senha);
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task<Senha?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        context.Senhas.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
}
