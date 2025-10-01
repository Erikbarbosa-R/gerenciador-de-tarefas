#!/bin/bash

echo "========================================"
echo "   Task Manager API - Gerenciador de Tarefas"
echo "========================================"
echo

echo "Verificando se o .NET 8 SDK está instalado..."
if ! command -v dotnet &> /dev/null; then
    echo "ERRO: .NET 8 SDK não encontrado!"
    echo
    echo "Por favor, instale o .NET 8 SDK em:"
    echo "https://dotnet.microsoft.com/download/dotnet/8.0"
    echo
    exit 1
fi

echo ".NET SDK encontrado!"
echo

echo "Restaurando dependências..."
dotnet restore
if [ $? -ne 0 ]; then
    echo "ERRO: Falha ao restaurar dependências!"
    exit 1
fi

echo
echo "Compilando projeto..."
dotnet build
if [ $? -ne 0 ]; then
    echo "ERRO: Falha na compilação!"
    exit 1
fi

echo
echo "Executando migrações do banco de dados..."
dotnet ef database update --project src/TaskManager.Infrastructure --startup-project src/TaskManager.API
if [ $? -ne 0 ]; then
    echo "AVISO: Falha nas migrações. Continuando mesmo assim..."
fi

echo
echo "Executando testes..."
dotnet test
if [ $? -ne 0 ]; then
    echo "AVISO: Alguns testes falharam. Continuando mesmo assim..."
fi

echo
echo "========================================"
echo "   Iniciando a aplicação..."
echo "========================================"
echo
echo "A API estará disponível em:"
echo "- Swagger UI: https://localhost:7000/swagger"
echo "- API: https://localhost:7000/api"
echo
echo "Pressione Ctrl+C para parar a aplicação"
echo

dotnet run --project src/TaskManager.API
