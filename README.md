# Rise GS (.NET 8)

Projeto de API em ASP.NET Core 8 que integra autenticação JWT, Oracle como banco de dados e recursos de IA para auxiliar no gerenciamento de currículos.

## Pré-requisitos
- .NET 8 SDK instalado.
- Acesso a uma instância Oracle (string de conexão no formato `User Id=<usuario>;Password=<senha>;Data Source=<host>:<porta>/<service_name>;`).
- Chaves para autenticação JWT e, opcionalmente, para a API Gemini.

## Configuração
1. Copie o arquivo `rise_gs/appsettings.json` ou use variáveis de ambiente para definir as seguintes chaves:
   - `ConnectionStrings:OracleConnection` para apontar para seu banco Oracle.
   - `Jwt:Key`, `Jwt:Issuer` e `Jwt:Audience` para assinar e validar tokens.
   - `Gemini:ApiKey` e `Gemini:Model` caso vá usar a integração com IA.
2. (Opcional) Crie um arquivo `appsettings.Development.json` para manter segredos fora do controle de versão.

## Como executar localmente
1. Restaure as dependências:
   ```bash
   dotnet restore
   ```
2. Rode a aplicação Web API:
   ```bash
   dotnet run --project rise_gs
   ```
3. Acesse os endpoints principais:
   - **Swagger UI:** `http://localhost:5085/swagger`
   - **Health check:** `http://localhost:5085/health`

> Ajuste a porta conforme o que o `dotnet run` indicar no console.

## Testes
Execute a suíte de testes com:
```bash
dotnet test
```

## Integrantes
| Nome | RM | Contato |
| --- | --- | --- |
| _Preencha aqui_ | _RM_ | _Email/Telefone_ |
| _Preencha aqui_ | _RM_ | _Email/Telefone_ |

## Link do vídeo no YouTube
Cole aqui o link da apresentação ou demonstração do projeto: `https://youtube.com/...`
