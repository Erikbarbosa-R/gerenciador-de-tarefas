@echo off
echo ========================================
echo    Task Manager API - Gerenciador de Tarefas
echo ========================================
echo.

echo Verificando se o .NET 8 SDK está instalado...
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ERRO: .NET 8 SDK não encontrado!
    echo.
    echo Por favor, instale o .NET 8 SDK em:
    echo https://dotnet.microsoft.com/download/dotnet/8.0
    echo.
    pause
    exit /b 1
)

echo .NET SDK encontrado!
echo.

echo Restaurando dependências...
dotnet restore
if %errorlevel% neq 0 (
    echo ERRO: Falha ao restaurar dependências!
    pause
    exit /b 1
)

echo.
echo Compilando projeto...
dotnet build
if %errorlevel% neq 0 (
    echo ERRO: Falha na compilação!
    pause
    exit /b 1
)

echo.
echo Executando migrações do banco de dados...
dotnet ef database update --project src/TaskManager.Infrastructure --startup-project src/TaskManager.API
if %errorlevel% neq 0 (
    echo AVISO: Falha nas migrações. Continuando mesmo assim...
)

echo.
echo Executando testes...
dotnet test
if %errorlevel% neq 0 (
    echo AVISO: Alguns testes falharam. Continuando mesmo assim...
)

echo.
echo ========================================
echo    Iniciando a aplicação...
echo ========================================
echo.
echo A API estará disponível em:
echo - Swagger UI: https://localhost:7000/swagger
echo - API: https://localhost:7000/api
echo.
echo Pressione Ctrl+C para parar a aplicação
echo.

dotnet run --project src/TaskManager.API

pause
