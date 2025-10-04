# Task Manager API

Um gerenciador de tarefas completo implementado com .NET 8, seguindo os princípios de Clean Architecture.

## Acesso à Aplicação

**Link do Deploy:** https://gerenciador-de-tarefas-production-7bba.up.railway.app

**Documentação Swagger:** https://gerenciador-de-tarefas-production-7bba.up.railway.app/swagger

## Tecnologias Utilizadas

- **.NET 8** - Framework principal
- **PostgreSQL** - Banco de dados
- **JWT** - Autenticação
- **Entity Framework Core** - ORM
- **Railway** - Deploy e hospedagem

## Como Rodar o Projeto Localmente

### Opção 1: Script Automático (Recomendado)

```bash
git clone https://github.com/seu-usuario/gerenciador-de-tarefas.git
cd gerenciador-de-tarefas
./run.bat  # Windows
```

### Opção 2: Comandos Manuais

```bash
git clone https://github.com/seu-usuario/gerenciador-de-tarefas.git
cd gerenciador-de-tarefas
dotnet restore
dotnet build
dotnet ef database update --project src/TaskManager.Infrastructure --startup-project src/TaskManager.API
dotnet run --project src/TaskManager.API
```

**Acesse:** `https://localhost:7000/swagger`

## Endpoints da API

### Autenticação
- `POST /api/users/register` - Registrar usuário
- `POST /api/users/login` - Fazer login

### Tarefas
- `GET /api/tasks` - Listar tarefas
- `POST /api/tasks` - Criar tarefa
- `PUT /api/tasks/{id}` - Atualizar tarefa
- `DELETE /api/tasks/{id}` - Deletar tarefa

## Exemplo de Uso

**1. Registrar usuário:**
```bash
curl -X POST "https://gerenciador-de-tarefas-production-7bba.up.railway.app/api/users/register" \
  -H "Content-Type: application/json" \
  -d '{"name": "João Silva", "email": "joao@exemplo.com", "password": "senha123"}'
```

**2. Fazer login:**
```bash
curl -X POST "https://gerenciador-de-tarefas-production-7bba.up.railway.app/api/users/login" \
  -H "Content-Type: application/json" \
  -d '{"email": "joao@exemplo.com", "password": "senha123"}'
```

**3. Criar tarefa:**
```bash
curl -X POST "https://gerenciador-de-tarefas-production-7bba.up.railway.app/api/tasks" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{"title": "Minha Tarefa", "description": "Descrição", "userId": "id-usuario", "priority": 2}'
```

## Deploy no Railway

1. Instale o Railway CLI: `npm install -g @railway/cli`
2. Faça login: `railway login`
3. Execute: `./scripts/deploy-railway.bat`
4. Configure as variáveis: `JWT_KEY`, `JWT_ISSUER`, `JWT_AUDIENCE`

## Testes

```bash
dotnet test
```

## Contribuição

1. Fork o projeto
2. Crie uma branch (`git checkout -b feature/nova-feature`)
3. Commit (`git commit -m 'Adiciona nova feature'`)
4. Push (`git push origin feature/nova-feature`)
5. Abra um Pull Request

## Licença

MIT License