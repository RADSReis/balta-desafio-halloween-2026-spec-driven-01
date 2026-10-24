# Constituição do projeto

## Stack
- .NET 10, C#, Minimal API
- Persistência em SQLite com EF Core
- Testes com xUnit


## Arquitetura

- Endpoint não acessa banco diretamente, sempre via serviço de aplicação
- Regra de negócio não vive no endpoint nem no handler HTTP
- Nenhuma dependência nova entra sem justificativa escrita no plano

## Qualidade
- Toda regra de negócio precisa ter teste unitário
- Todo endpoint precisa ter ao menos um teste de integração de caminho feliz
- Erro de validação sempre retorna 400 com ProblemDetails
