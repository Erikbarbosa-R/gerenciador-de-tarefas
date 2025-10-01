# Task Manager API

Um gerenciador de tarefas completo implementado com .NET 8, seguindo os princípios de Clean Architecture e Clean Code.

## 🏗️ Arquitetura

O projeto segue os princípios da Clean Architecture, organizado em camadas:

- **Domain**: Entidades, Value Objects, Interfaces e Enums
- **Application**: Casos de uso, DTOs, Validações e Comandos/Queries (CQRS)
- **Infrastructure**: Implementações de repositórios, Entity Framework e serviços externos
- **API**: Controllers, Middlewares e configurações da API

## 🚀 Funcionalidades

### Tarefas
- ✅ Criar tarefa
- ✅ Editar tarefa
- ✅ Deletar tarefa
- ✅ Listar tarefas
- ✅ Visualizar detalhes de uma tarefa
- ✅ Filtros por usuário, status, prioridade
- ✅ Busca por texto
- ✅ Tarefas vencidas
- ✅ Tarefas do dia

### Usuários
- ✅ Cadastro de usuário
- ✅ Autenticação JWT
- ✅ Controle de acesso por roles
- ✅ Validação de email único

### Recursos Técnicos
- ✅ Autenticação JWT
- ✅ Swagger/OpenAPI
- ✅ Logging com Serilog
- ✅ Tratamento de erros global
- ✅ Validações com FluentValidation
- ✅ Testes unitários
- ✅ Entity Framework Core
- ✅ CQRS com MediatR
- ✅ Repository Pattern
- ✅ Unit of Work Pattern

## 🛠️ Tecnologias

- **.NET 8**
- **Entity Framework Core 8**
- **MySQL**
- **JWT Authentication**
- **FluentValidation**
- **MediatR**
- **Serilog**
- **Swagger/OpenAPI**
- **xUnit**
- **FluentAssertions**
- **Moq**

## 📋 Pré-requisitos

### Desenvolvimento Local
- .NET 8 SDK
- MySQL (local ou Docker)
- Visual Studio 2022 ou VS Code

### Deploy no Railway
- Conta no Railway
- Railway CLI
- Git

## 🚀 Como executar

### Desenvolvimento Local

1. **Clone o repositório**
   ```bash
   git clone <repository-url>
   cd gerenciador-de-tarefas
   ```

2. **Restaure as dependências**
   ```bash
   dotnet restore
   ```

3. **Execute as migrações**
   ```bash
   dotnet ef database update --project src/TaskManager.Infrastructure --startup-project src/TaskManager.API
   ```

4. **Execute a aplicação**
   ```bash
   dotnet run --project src/TaskManager.API
   ```

5. **Acesse a documentação**
   - Swagger UI: `https://localhost:7000/swagger`
   - API: `https://localhost:7000/api`

### Deploy no Railway

1. **Instale o Railway CLI**
   ```bash
   npm install -g @railway/cli
   ```

2. **Faça login**
   ```bash
   railway login
   ```

3. **Execute o script de deploy**
   ```bash
   # Windows
   scripts/deploy-railway.bat
   
   # Linux/macOS
   ./scripts/deploy-railway.sh
   ```

4. **Configure as variáveis de ambiente no Railway**
   - `JWT_KEY`: Sua chave JWT secreta
   - `JWT_ISSUER`: TaskManagerAPI
   - `JWT_AUDIENCE`: TaskManagerUsers

Para mais detalhes, consulte [docs/RAILWAY_DEPLOY.md](docs/RAILWAY_DEPLOY.md)

## 🧪 Executar testes

```bash
dotnet test
```

## 📚 Endpoints da API

### Autenticação
- `POST /api/users/register` - Registrar usuário
- `POST /api/users/login` - Fazer login

### Tarefas
- `GET /api/tasks` - Listar tarefas
- `GET /api/tasks/{id}` - Obter tarefa por ID
- `POST /api/tasks` - Criar tarefa
- `PUT /api/tasks/{id}` - Atualizar tarefa
- `DELETE /api/tasks/{id}` - Deletar tarefa

## 🔐 Autenticação

A API utiliza JWT para autenticação. Para acessar os endpoints protegidos:

1. Registre um usuário ou faça login
2. Use o token retornado no header `Authorization: Bearer {token}`

## 📊 Estrutura do Banco de Dados

### Tabela Users
- Id (Guid, PK)
- Name (string)
- Email (string, unique)
- PasswordHash (string)
- Role (enum)
- IsActive (bool)
- CreatedAt (DateTime)
- UpdatedAt (DateTime?)
- IsDeleted (bool)

### Tabela Tasks
- Id (Guid, PK)
- Title (string)
- Description (string)
- Status (enum)
- Priority (enum)
- DueDate (DateTime?)
- UserId (Guid, FK)
- CreatedAt (DateTime)
- UpdatedAt (DateTime?)
- IsDeleted (bool)

## 🎯 Padrões de Design Implementados

- **Repository Pattern**: Abstração do acesso a dados
- **Unit of Work**: Controle de transações
- **CQRS**: Separação de comandos e consultas
- **Mediator**: Desacoplamento entre camadas
- **Value Objects**: Objetos imutáveis para representar conceitos
- **Domain Events**: Eventos de domínio (preparado para implementação)
- **Specification Pattern**: Consultas complexas (preparado para implementação)

## 🔧 Configuração

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TaskManagerDb;Uid=root;Pwd=root;"
  },
  "Jwt": {
    "Key": "TaskManagerSuperSecretKeyThatShouldBeAtLeast32CharactersLong",
    "Issuer": "TaskManagerAPI",
    "Audience": "TaskManagerUsers"
  }
}
```

### Railway (Produção)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "${DATABASE_URL}"
  },
  "Jwt": {
    "Key": "${JWT_KEY}",
    "Issuer": "${JWT_ISSUER}",
    "Audience": "${JWT_AUDIENCE}"
  }
}
```

## 📈 Melhorias Futuras

- [ ] Implementar Domain Events
- [ ] Adicionar cache com Redis
- [ ] Implementar Specification Pattern
- [ ] Adicionar testes de integração
- [ ] Implementar rate limiting
- [ ] Adicionar métricas e monitoramento
- [ ] Implementar background jobs
- [ ] Adicionar notificações
- [ ] Implementar auditoria
- [ ] Adicionar versionamento da API

## 🤝 Contribuição

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.
