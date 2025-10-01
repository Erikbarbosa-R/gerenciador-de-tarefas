# Scripts SQL - Task Manager

## 📁 Arquivos

- `create_tables.sql` - Script completo para desenvolvimento local
- `railway_tables.sql` - Script para Railway (sem CREATE DATABASE)

## 🚀 Como usar

### 1. Desenvolvimento Local

```sql
-- Execute o arquivo completo
SOURCE database/create_tables.sql;
```

### 2. Railway

```sql
-- Execute apenas as tabelas (database já existe)
SOURCE database/railway_tables.sql;
```

### 3. DBeaver

1. Abra o DBeaver
2. Conecte ao banco
3. Abra o arquivo SQL
4. Execute o script

## 📊 Estrutura das Tabelas

### Users
- `Id` - GUID (Primary Key)
- `Name` - Nome do usuário
- `Email` - Email único
- `PasswordHash` - Hash da senha
- `Role` - Papel (0=User, 1=Admin)
- `IsActive` - Usuário ativo
- `CreatedAt` - Data de criação
- `UpdatedAt` - Data de atualização
- `IsDeleted` - Soft delete

### Tasks
- `Id` - GUID (Primary Key)
- `Title` - Título da tarefa
- `Description` - Descrição
- `Status` - Status (0=Pending, 1=InProgress, 2=Completed, 3=Cancelled)
- `Priority` - Prioridade (0=Low, 1=Medium, 2=High, 3=Critical)
- `DueDate` - Data de vencimento
- `UserId` - ID do usuário (Foreign Key)
- `CreatedAt` - Data de criação
- `UpdatedAt` - Data de atualização
- `IsDeleted` - Soft delete

## 🔍 Queries Úteis

### Verificar tabelas
```sql
SHOW TABLES;
```

### Ver estrutura
```sql
DESCRIBE Users;
DESCRIBE Tasks;
```

### Ver dados
```sql
SELECT * FROM Users WHERE IsDeleted = FALSE;
SELECT * FROM Tasks WHERE IsDeleted = FALSE;
```

### Contar registros
```sql
SELECT COUNT(*) FROM Users WHERE IsDeleted = FALSE;
SELECT COUNT(*) FROM Tasks WHERE IsDeleted = FALSE;
```

## 🎯 Dados de Exemplo

O script inclui:
- 3 usuários (1 admin, 2 usuários)
- 3 tarefas de exemplo
- Dados para teste da API

## 🔧 Troubleshooting

### Erro: Table already exists
- Use `CREATE TABLE IF NOT EXISTS` (Railway)
- Ou `DROP TABLE` antes de criar (Local)

### Erro: Foreign key constraint
- Crie primeiro a tabela `Users`
- Depois a tabela `Tasks`

### Erro: Duplicate entry
- Use `INSERT IGNORE` (Railway)
- Ou verifique se os dados já existem
