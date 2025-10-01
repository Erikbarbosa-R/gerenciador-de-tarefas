# Deploy no Railway - Task Manager API

Este guia explica como fazer o deploy da Task Manager API no Railway usando MySQL.

## 🚀 Pré-requisitos

1. **Conta no Railway**: [https://railway.app](https://railway.app)
2. **Railway CLI**: [https://docs.railway.app/develop/cli](https://docs.railway.app/develop/cli)
3. **.NET 8 SDK**: [https://dotnet.microsoft.com/download/dotnet/8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
4. **Git**: Para versionamento do código

## 📋 Passo a Passo

### 1. Preparação do Projeto

O projeto já está configurado para usar PostgreSQL e variáveis de ambiente do Railway.

### 2. Instalação do Railway CLI

**Windows:**
```bash
# Via Chocolatey
choco install railway

# Via Scoop
scoop install railway

# Via npm
npm install -g @railway/cli
```

**Linux/macOS:**
```bash
# Via curl
curl -fsSL https://railway.app/install.sh | sh

# Via npm
npm install -g @railway/cli
```

### 3. Login no Railway

```bash
railway login
```

### 4. Criar Projeto no Railway

```bash
# Inicializar projeto
railway init

# Ou conectar a um projeto existente
railway link [project-id]
```

### 5. Adicionar Banco MySQL

```bash
# Adicionar serviço MySQL
railway add mysql

# Ou via dashboard do Railway
```

### 6. Configurar Variáveis de Ambiente

No dashboard do Railway ou via CLI:

```bash
# Configurar variáveis de ambiente
railway variables set JWT_KEY="sua-chave-jwt-super-secreta-aqui"
railway variables set JWT_ISSUER="TaskManagerAPI"
railway variables set JWT_AUDIENCE="TaskManagerUsers"
```

**Variável automática do MySQL:**
- `DATABASE_URL` - Connection string completa do MySQL

### 7. Deploy Automático

**Opção 1: Script Automático**
```bash
# Windows
scripts/deploy-railway.bat

# Linux/macOS
chmod +x scripts/deploy-railway.sh
./scripts/deploy-railway.sh
```

**Opção 2: Manual**
```bash
# Criar migração
dotnet ef migrations add RailwayDeploy --project src/TaskManager.Infrastructure --startup-project src/TaskManager.API

# Build
dotnet build -c Release

# Deploy
railway up

# Aplicar migrações
railway run dotnet ef database update --project src/TaskManager.Infrastructure --startup-project src/TaskManager.API
```

### 8. Verificar Deploy

```bash
# Ver logs
railway logs

# Ver domínio
railway domain

# Ver status
railway status
```

## 🔧 Configurações Importantes

### String de Conexão

O Railway automaticamente fornece a variável de ambiente `DATABASE_URL` com a connection string completa:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "${DATABASE_URL}"
  }
}
```

**Exemplo de DATABASE_URL:**
```
mysql://root:senha@centerbeam.proxy.rlwy.net:37783/railway
```

### Variáveis de Ambiente Obrigatórias

```bash
JWT_KEY=sua-chave-jwt-super-secreta-aqui
JWT_ISSUER=TaskManagerAPI
JWT_AUDIENCE=TaskManagerUsers
```

### Porta da Aplicação

O Railway automaticamente define a variável `PORT`. A aplicação está configurada para usar:

```csharp
app.Run($"http://0.0.0.0:{Environment.GetEnvironmentVariable("PORT") ?? "8080"}");
```

## 📊 Monitoramento

### Logs

```bash
# Ver logs em tempo real
railway logs --follow

# Ver logs específicos
railway logs --service api
```

### Métricas

Acesse o dashboard do Railway para ver:
- CPU e memória
- Requisições por minuto
- Tempo de resposta
- Erros

### Health Check

A aplicação expõe um endpoint de health check:
- **URL**: `/swagger`
- **Timeout**: 100ms
- **Retry**: 10 tentativas

## 🐛 Troubleshooting

### Problemas Comuns

**1. Erro de Conexão com Banco**
```bash
# Verificar variáveis de ambiente
railway variables

# Testar conexão
railway run dotnet ef database update --project src/TaskManager.Infrastructure --startup-project src/TaskManager.API
```

**2. Erro de Migração**
```bash
# Verificar se as migrações existem
ls src/TaskManager.Infrastructure/Migrations/

# Criar nova migração se necessário
dotnet ef migrations add FixMigration --project src/TaskManager.Infrastructure --startup-project src/TaskManager.API
```

**3. Erro de Build**
```bash
# Limpar e restaurar
dotnet clean
dotnet restore
dotnet build -c Release
```

**4. Erro de JWT**
```bash
# Verificar se a chave JWT está configurada
railway variables get JWT_KEY

# Gerar nova chave se necessário
openssl rand -base64 32
```

### Logs de Debug

```bash
# Ver logs detalhados
railway logs --level debug

# Ver logs de um serviço específico
railway logs --service api --level info
```

## 🔄 CI/CD

### GitHub Actions

Crie `.github/workflows/railway-deploy.yml`:

```yaml
name: Deploy to Railway

on:
  push:
    branches: [main]

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '8.0.x'
          
      - name: Restore dependencies
        run: dotnet restore
        
      - name: Build
        run: dotnet build --no-restore
        
      - name: Test
        run: dotnet test --no-build --verbosity normal
        
      - name: Deploy to Railway
        uses: railwayapp/railway-deploy@v1
        with:
          railway-token: ${{ secrets.RAILWAY_TOKEN }}
```

### Variáveis de Ambiente no GitHub

Adicione no GitHub Secrets:
- `RAILWAY_TOKEN`: Token do Railway CLI

## 📈 Otimizações

### Performance

1. **Connection Pooling**: Configurado automaticamente
2. **Caching**: Implementar Redis se necessário
3. **CDN**: Para arquivos estáticos
4. **Load Balancing**: Automático no Railway

### Segurança

1. **HTTPS**: Automático no Railway
2. **JWT**: Configurado com chave segura
3. **CORS**: Configurado para produção
4. **SSL**: Configurado para MySQL
5. **Rate Limiting**: Implementar se necessário

## 🔗 Links Úteis

- [Railway Documentation](https://docs.railway.app)
- [Railway CLI Reference](https://docs.railway.app/develop/cli)
- [MySQL on Railway](https://docs.railway.app/databases/mysql)
- [.NET on Railway](https://docs.railway.app/languages/dotnet)

## 📞 Suporte

- **Railway Support**: [https://railway.app/support](https://railway.app/support)
- **Discord**: [https://discord.gg/railway](https://discord.gg/railway)
- **GitHub Issues**: Para problemas específicos do projeto
