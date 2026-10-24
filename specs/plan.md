# Plano de implementação

## 1. Contexto

Implementar uma API para geração e recuperação de senhas fortes, utilizando a stack e as regras arquiteturais definidas na Constituição do projeto.

## 2. Stack tecnológica

- **Runtime e linguagem:** .NET 10 e C#.
- **API HTTP:** ASP.NET Core Minimal API.
- **Persistência:** SQLite com Entity Framework Core.
- **Testes:** xUnit.
- **Documentação da API:** Swagger

Nenhuma dependência adicional deve ser introduzida sem justificativa neste plano.

## 3. Arquitetura

### 3.1. Camadas

O projeto será organizado em responsabilidades distintas:

- **API:** define os endpoints HTTP, recebe requisições, chama os serviços de aplicação e transforma os resultados em respostas HTTP.
- **Application:** contém regras de negócios e coordena o código
- **Infrastructure:** contém o acesso ao SQLite, o `DbContext` do EF Core e as configurações de persistência.
- **Tests:** contém os projetos de testes unitários e de integração.


### 3.2. Regras arquiteturais

- Endpoints não podem acessar o banco diretamente.
- Regras de negócio não podem ser implementadas nos endpoints nem em handlers HTTP.
- Os serviços de aplicação devem coordenar a geração, a persistência e a recuperação das senhas.
- A lógica de negócio deve poder ser testada sem iniciar a API ou depender do banco de dados.

## 4. Modelo de dados

- Utilizar SQLite.
- Nome lógico do banco: `SenhasFortes`.
- O arquivo do banco deve ser armazenado em um local definido pela configuração da aplicação, evitando caminhos absolutos específicos de uma máquina.
