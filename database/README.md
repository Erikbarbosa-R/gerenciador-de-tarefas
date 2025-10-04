# Scripts SQL - Task Manager

##  Arquivos

- `railway_tables.sql` - Script para MySQL (Railway)
- `railway_tables_postgresql.sql` - Script para PostgreSQL (Railway)
- `create_tables.sql` - Script completo para desenvolvimento local

##  Como usar

### 1. PostgreSQL (Railway)

```sql
-- Execute o arquivo PostgreSQL
SOURCE database/railway_tables_postgresql.sql;
```

### 2. MySQL (Railway)

```sql
-- Execute o arquivo MySQL
SOURCE database/railway_tables.sql;
```

### 3. DBeaver

1. Abra o DBeaver
2. Conecte ao banco
3. Abra o arquivo SQL correto
4. Execute o script

##  Estrutura das Tabelas

### Users
- `Id` - UUID (Primary Key)
- `Name` - Nome do usuário
- `Email` - Email único
- `PasswordHash` - Hash da senha
- `Role` - Papel (0=User, 1=Admin)
- `IsActive` - Usuário ativo
- `CreatedAt` - Data de criação
- `UpdatedAt` - Data de atualização
- `IsDeleted` - Soft delete

### Tasks
- `Id` - UUID (Primary Key)
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
-- PostgreSQL
\dt

-- MySQL
SHOW TABLES;
```

### Ver estrutura
```sql
-- PostgreSQL
\d Users
\d Tasks

-- MySQL
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

##  Dados de Exemplo

O script inclui:
- 3 usuários (1 admin, 2 usuários)
- 3 tarefas de exemplo
- Dados para teste da API

##  Troubleshooting

### Erro: Table already exists
- Use `CREATE TABLE IF NOT EXISTS` (Railway)
- Ou `DROP TABLE` antes de criar (Local)

### Erro: Foreign key constraint
- Crie primeiro a tabela `Users`
- Depois a tabela `Tasks`

### Erro: Duplicate entry
- Use `ON CONFLICT DO NOTHING` (PostgreSQL)
- Use `INSERT IGNORE` (MySQL)

### Erro: Syntax error at INDEX
- Use o script PostgreSQL: `railway_tables_postgresql.sql`
- Ou use o script MySQL: `railway_tables.sql`

##  Diferenças PostgreSQL vs MySQL

| Recurso | PostgreSQL | MySQL |
|---------|------------|-------|
| UUID | `UUID` | `CHAR(36)` |
| Timestamp | `TIMESTAMP(6)` | `DATETIME(6)` |
| Integer | `INTEGER` | `INT` |
| Auto UUID | `gen_random_uuid()` | `UUID()` |
| Interval | `INTERVAL '7 days'` | `INTERVAL 7 DAY` |
| Conflict | `ON CONFLICT DO NOTHING` | `INSERT IGNORE` |
| Index | `CREATE INDEX IF NOT EXISTS` | `CREATE INDEX` |