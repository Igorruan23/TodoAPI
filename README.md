# TodoAPI

API RESTful para gerenciar tarefas (todo items) com autenticação JWT e controle de acesso baseado em roles usando ASP.NET Core 8.0, Entity Framework Core e Identity.

---

## Tecnologias usadas

- .NET 8  
- ASP.NET Core Web API  
- Entity Framework Core (SQL Server)  
- ASP.NET Core Identity (User & Role management)  
- AutoMapper (DTOs)  
- JWT Authentication  
- Swagger (OpenAPI)  

---

## Setup rápido

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
- SQL Server (local ou remoto)  
- Visual Studio 2022 / VSCode  

### Passos

1. Clone o repo:
   ```bash
   git clone https://github.com/Igorruan23/TodoAPI
   cd TodoAPI
	 ```
2.Ajuste o arquivo appsettings.json conforme seu ambiente:
 ```json
   {
  "ConnectionStrings": {
     "SQLConnection": "Server=SEU_SERVIDOR\\SQLEXPRESS;Database=ToDoDB;Trusted_Connection=True;TrustServerCertificate=true"
  },
  "JWT": {
    "ValidAudience": "https://localhost:5001",
    "ValidIssuer": "https://localhost:24288",
    "secretKey": "SuaChaveSecretaBemGrandeAqui1234567890"
  }
}
   ```
3.  Instale as dependências
   ```bash
    dotnet restore
   ```
4. Execute as migrations para criar o banco de dados:
   ```bash
    dotnet ef database update
    ```
5. Rode o projeto:
   ```bash
   dotnet run
   ```
