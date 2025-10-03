# Task Manager API - Sobre Projeto 2

> **Criado por:** Erick  
> **Data:** Janeiro 2025  
> **Versão:** 1.0.0

---

## **O QUE É ESTE PROJETO?**

Este é um **Sistema de Gerenciamento de Tarefas** completo construído em **.NET 8 Web API**. O projeto implementa uma arquitetura limpa (Clean Architecture) com autenticação JWT, banco de dados PostgreSQL e deploy automático no Railway.

### **Funcionalidades Principais:**

- **Autenticação JWT** - Login/registro de usuários
- **CRUD de Tarefas** - Criar, ler, atualizar e deletar tarefas  
- **Gestão de Usuários** - Sistema de perfis (Admin/User)
- **Sistema de Prioridades** - Low, Medium, High, Critical
- **Filtros e Busca** - Por status, usuário, prioridade
- **Deploy Automático** - Railway com PostgreSQL

---

## **ESTRUTURA DO PROJETO**

### **Arquivos Principais:**

```
gerenciador-de-tarefas/
├── TaskManager.sln           # Arquivo principal da solução (.NET)
├── Dockerfile               # Configuração para containers Docker
├── nixpacks.toml           # Configuração automática Railway
├── railway.json            # Configurações específicas Railway  
├── run.bat                 # Script para rodar localmente
└── INTEGRATION_GUIDE.md    # Guia completo da API (frontend)
```

### **Pastas do Código:**

```
src/
├── TaskManager.API/          # Controllers e endpoints da API
├── TaskManager.Application/ # Regras de negócio e comandos/queries
├── TaskManager.Infrastructure/ # Banco de dados e repositorios
└── TaskManager.Domain/      # Entidades e interfaces principais

tests/
└── TaskManager.Tests/       # Testes unitários
```

### **Recursos:**

```
database/
├── railway_tables_postgresql.sql  # Schema do banco PostgreSQL
└── README.md                       # Documentação do banco

scripts/
└── deploy-railway.bat              # Script de deploy Railway

docs/
└── RAILWAY_DEPLOY.md              # Guia de deploy detalhado

examples/
└── api-examples.http              # Exemplos de requisições HTTP
```

---

## **COMANDOS ESSENCIAIS**

### **Executar Localmente:**

```bash
# Opção 1: Usar script automático
./run.bat

# Opção 2: Comandos manuais
dotnet restore
dotnet build
dotnet run --project src/TaskManager.API
```

### **Testes:**

```bash
# Executar todos os testes
dotnet test

# Executar com detalhes
dotnet test --verbosity normal

# Executar apenas testes específicos
dotnet test --filter "TestClassName"
```

### **Banco de Dados:**

```bash
# Aplicar migrações
dotnet ef database update --project src/TaskManager.Infrastructure --startup-project src/TaskManager.API

# Criar nova migração
dotnet ef migrations add NomeDaMigração --project src/TaskManager.Infrastructure --startup-project src/TaskManager.API

# Remover última migração
dotnet ef migrations remove --project src/TaskManager.Infrastructure --startup-project src/TaskManager.API
```

### **Deploy Railway:**

```bash
# Salvar tudo no git primeiro
git add .
git commit -m "minha-atualizacao"
git push

# Depois usar o script automático
./scripts/deploy-railway.bat

# Ou fazer manualmente:
railway up
```

---

## **TECNOLOGIAS UTILIZADAS**

### **Backend (.NET 8):**
- **ASP.NET Core Web API** - Framework principal
- **Entity Framework Core** - ORM para banco de dados
- **JWT Bearer** - Autenticação por tokens
- **Swagger/OpenAPI** - Documentação automática
- **Serilog** - Sistema de logs profissional

### **Banco de Dados:**
- **PostgreSQL** - Banco principal (Railway)
- **Migration System** - Controle de versão do schema

### **Deploy e DevOps:**
- **Railway** - Hospedagem em nuvem
- **Docker** - Containerização  
- **Nixpacks** - Build automático
- **Railway CLI** - Deploy via linha de comando

### **Qualidade:**
- **xUnit** - Framework de testes
- **MediatR** - Padrão CQRS
- **FluentValidation** - Validação de dados
- **Clean Architecture** - Arquitetura limpa

---

## **ARQUITETURA DO SISTEMA**

### **Clean Architecture:**

```
┌─────────────────────────────────────────┐
│             TaskManager.API             │ ← Controllers HTTP
├─────────────────────────────────────────┤
│          TaskManager.Application        │ ← Commands/Queries/Handlers  
├─────────────────────────────────────────┤
│         TaskManager.Infrastructure      │ ← Database/Repositories
├─────────────────────────────────────────┤
│           TaskManager.Domain             │ ← Entities/Interfaces
└─────────────────────────────────────────┘
```

### **Principais Entidades:**

- **User** - Usuários do sistema
- **Task** - Tarefas com prioridade/status
- **BaseEntity** - Classe base com ID e timestamps

---

## **SISTEMA DE AUTENTICAÇÃO**

### **JWT Token:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "userId": "guia-unique-id",
  "name": "Nome do Usuário", 
  "email": "email@exemplo.com",
  "role": 1
}
```

### **Roles Disponíveis:**
- **0** = USER (Usuário comum)
- **1** = ADMIN (Administrador)

### **Endpoints Protegidos:**
- `/api/tasks/*` - Todas operações de tarefas
- Header obrigatório: `Authorization: Bearer {token}`

---

## **SISTEMA DE TAREFAS**

### **Prioridades:**
- **0** = Low (Baixa)
- **1** = Medium (Média) 
- **2** = High (Alta)
- **3** = Critical (Crítica)

### **Status:**
- **0** = Pending (Pendente)
- **1** = InProgress (Em andamento)
- **2** = Completed (Concluída)
- **3** = Cancelled (Cancelada)

---

## **ENDPOINTS PRINCIPAIS**

### **Autentificação:**
```
POST /api/users/register  - Registrar usuário
POST /api/users/login     - Fazer login
```

### **Tarefas:**
```
GET    /api/tasks         - Listar tarefas (com filtros)
POST   /api/tasks         - Criar nova tarefa
GET    /api/tasks/{id}    - Buscar tarefa por ID
PATCH  /api/tasks/{id}    - Atualizar tarefa
DELETE /api/tasks/{id}    - Deletar tarefa
```

### **Filtros Disponíveis:**
```
?userId=sua-id          # Por usuário criador
&status=0              # Por status
&priority=2            # Por prioridade  
&searchTerm=palavra    # Busca textual
```

---

## **BANCO DE DADOS**

### **PostgreSQL (Railway):**
- **Tables:** Users, Tasks
- **Types:** UUID, VARCHAR, TIMESTAMP, BOOLEAN, INTEGER
- **Indexes:** Otimizados para consultas rápidas
- **Foreign Keys:** Integridade referencial

### **Principais Campos:**

**Users:**
- `Id` (UUID, PK) - Identificador único
- `Name` (VARCHAR) - Nome do usuário
- `Email` (VARCHAR, UNIQUE) - Email único
- `PasswordHash` (VARCHAR) - Senha criptografada
- `Role` (INTEGER) - Papel do usuário
- `CreatedAt`, `UpdatedAt` (TIMESTAMP) - Auditoria

```sql
Tasks:
- `Id` (UUID, PK) - Identificador único  
- `Title` (VARCHAR) - Título da tarefa
- `Description` (VARCHAR) - Descrição
- `Status`, `Priority` (INTEGER) - Status e prioridade
- `UserId` (UUID, FK) - Criador da tarefa
- `AssignedToUserId` (UUID, FK) - Usuário atribuído
```

---

## **DEPLOY E CONFIGURAÇÃO**

### **Railway (Hospedagem):**
- **URL:** https://gerenciador-de-tarefas-production-7bba.up.railway.app
- **Swagger:** https://gerenciador-de-tarefas-production-7bba.up.railway.app/swagger  
- **Health:** https://gerenciador-de-tarefas-production-7bba.up.railway.app/health

### **Variáveis de Ambiente (.env):**
```
DATABASE_URL=postgresql://...              # String de conexão PostgreSQL
JWT_KEY=minha-chave-jwt-super-secreta       # Chave para assinar JWT
JWT_ISSUER=TaskManager.API                  # Emissor do token
JWT_AUDIENCE=TaskManager.Users              # Audiência do token
PORT=8080                                   # Porta da aplicação
```

---

## **TESTANDO A API**

### **Usando Postman ou Thunder Client:**

**1. Registrar Usuário:**
```http
POST {{baseUrl}}/api/users/register
Content-Type: application/json

{
  "name": "Erick Silva",
  "email": "erick@exemplo.com", 
  "password": "senha123"
}
```

**2. Fazer Login:**
```http
POST {{baseUrl}}/api/users/login
Content-Type: application/json

{
  "email": "erick@exemplo.com",
  "password": "senha123"
}
```

**3. Criar Tarefa (usar token do login):**
```http
POST {{baseUrl}}/api/tasks
Authorization: Bearer {token}
Content-Type: application/json

{
  "title": "Minha Primeira Tarefa",
  "description": "Descrição detalhada",
  "userId": "id-do-usuario",
  "priority": 2,
  "dueDate": "2025-12-12T23:59:59Z"
}
```

### **Documentação Interativa:**
Acesse: `https://gerenciador-de-tarefas-production-7bba.up.railway.app/swagger`

---

## **MANUTENÇÃO E DESENVOLVIMENTO**

### **Fazer Alterações:**

1. **Modificar código** em `src/`
2. **Testar localmente:** `./run.bat`
3. **Executar testes:** `dotnet test`
4. **Commit no Git:** `git add . && git commit -m "descrição"`
5. **Deploy:** `./scripts/deploy-railway.bat`

### **Debugging:**

**Logs Locais:** Aparecem no console quando roda com `./run.bat`

**Logs Railway:** 
```bash
railway logs --follow    # Logs em tempo real
railway status           # Status da aplicação  
railway variables        # Ver variáveis de ambiente
```

### **Monitoramento:**

**Railway Dashboard:** Métricas de CPU, memória, requisições

**Health Check:** `GET /health` retorna status da aplicação

---

## **RESUMO DA TECNOLOGIA**

### **O que eu sei fazer:**
- Criar APIs RESTful com .NET 8
- Implementar autenticação JWT  
- Usar PostgreSQL com Entity Framework
- Deploy automático no Railway
- Testes unitários com xUnit
- Clean Architecture

### **Para que serve:**
- sistema completo de tarefas
- Base para outros projetos .NET
- Aprendizagem de arquitetura limpa
- Portfolio profissional

### **Próximos passos sugeridos:**
- Implementar cache Redis
- Adicionar notificações
- Sistema de roles mais complexo
- Upload de arquivos
- Dashboard administrativo

---

**Projeto criado e documentado por mim (Erick) em Janeiro 2025!**

> Se você está lendo isto, significa que eu consegui criar um sistema completo de gerenciamento de tarefas usando tecnologias modernas. É um projeto que demonstra conhecimento em backend, banco de dados, deploy e boas práticas de desenvolvimento!
