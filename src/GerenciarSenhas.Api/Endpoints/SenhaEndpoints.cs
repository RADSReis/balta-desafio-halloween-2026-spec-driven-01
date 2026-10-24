using GerenciarSenhas.Application.Senhas;

namespace GerenciarSenhas.Api.Endpoints;

public sealed record SenhaResponse(Guid Id, string Senha);

public static class SenhaEndpoints
{
    public static IEndpointRouteBuilder MapSenhaEndpoints(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/senhas").WithTags("Senhas");

        grupo.MapPost("/", async (SenhaService service, CancellationToken cancellationToken) =>
            {
                var senha = await service.GerarAsync(cancellationToken);
                return Results.Created($"/senhas/{senha.Id}", new SenhaResponse(senha.Id, senha.Valor));
            })
            .WithName("GerarSenha")
            .WithSummary("Gera e armazena uma nova senha forte.")
            .Produces<SenhaResponse>(StatusCodes.Status201Created);

        grupo.MapGet("/{id}", async (string id, SenhaService service, CancellationToken cancellationToken) =>
            {
                var resultado = await service.ObterAsync(id, cancellationToken);
                return resultado.Status switch
                {
                    ObterSenhaStatus.Encontrada =>
                        Results.Ok(new SenhaResponse(resultado.Senha!.Id, resultado.Senha.Valor)),
                    ObterSenhaStatus.IdInvalido => Results.ValidationProblem(
                        new Dictionary<string, string[]> { ["id"] = ["O identificador informado não é um GUID válido."] }),
                    _ => Results.Problem(
                        statusCode: StatusCodes.Status404NotFound,
                        title: "Senha não encontrada.",
                        detail: $"Nenhuma senha encontrada para o identificador '{id}'.")
                };
            })
            .WithName("ObterSenha")
            .WithSummary("Recupera a senha associada ao identificador informado.")
            .Produces<SenhaResponse>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound);

        return app;
    }
}
