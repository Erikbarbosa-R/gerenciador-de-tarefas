@echo off
echo ========================================
echo    Deploy para Railway - Task Manager API
echo ========================================
echo.

echo Verificando se o Railway CLI está instalado...
railway --version >nul 2>&1
if %errorlevel% neq 0 (
    echo Railway CLI não encontrado!
    echo Instale em: https://docs.railway.app/develop/cli
    pause
    exit /b 1
)

echo Verificando se está logado...
railway whoami >nul 2>&1
if %errorlevel% neq 0 (
    echo Faça login no Railway:
    railway login
)

echo Criando migração do banco de dados...
dotnet ef migrations add RailwayDeploy --project src/TaskManager.Infrastructure --startup-project src/TaskManager.API
if %errorlevel% neq 0 (
    echo AVISO: Falha ao criar migração. Continuando mesmo assim...
)

echo.
echo Compilando projeto...
dotnet build -c Release
if %errorlevel% neq 0 (
    echo ERRO: Falha na compilação!
    pause
    exit /b 1
)

echo.
echo Executando testes...
dotnet test
if %errorlevel% neq 0 (
    echo AVISO: Alguns testes falharam. Continuando mesmo assim...
)

echo.
echo Fazendo deploy para Railway...
railway up
if %errorlevel% neq 0 (
    echo ERRO: Falha no deploy!
    pause
    exit /b 1
)

echo.
echo Aplicando migrações no Railway...
railway run dotnet ef database update --project src/TaskManager.Infrastructure --startup-project src/TaskManager.API
if %errorlevel% neq 0 (
    echo AVISO: Falha ao aplicar migrações. Verifique manualmente.
)

echo.
echo ========================================
echo    Deploy concluído!
echo ========================================
echo.
echo Sua API está disponível em:
railway domain
echo.
echo Swagger UI: https://[seu-dominio]/swagger
echo API: https://[seu-dominio]/api
echo.

pause
