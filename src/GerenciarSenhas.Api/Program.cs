using GerenciarSenhas.Api.Endpoints;
using GerenciarSenhas.Application.Senhas;
using GerenciarSenhas.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IGeradorSenha, GeradorSenha>();
builder.Services.AddScoped<SenhaService>();
builder.Services.AddInfrastructure();

var app = builder.Build();

await app.Services.InicializarBancoAsync();

app.UseExceptionHandler();
app.UseStatusCodePages();

app.UseSwagger();
app.UseSwaggerUI();

app.MapSenhaEndpoints();

app.Run();

public partial class Program;
