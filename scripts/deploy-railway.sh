#!/bin/bash

echo "========================================"
echo "   Deploy para Railway - Task Manager API"
echo "========================================"
echo

# Verificar se o Railway CLI está instalado
if ! command -v railway &> /dev/null; then
    echo "Railway CLI não encontrado!"
    echo "Instale em: https://docs.railway.app/develop/cli"
    exit 1
fi

# Verificar se está logado
if ! railway whoami &> /dev/null; then
    echo "Faça login no Railway:"
    railway login
fi

echo "Criando migração do banco de dados..."
dotnet ef migrations add RailwayDeploy --project src/TaskManager.Infrastructure --startup-project src/TaskManager.API

echo
echo "Compilando projeto..."
dotnet build -c Release

echo
echo "Executando testes..."
dotnet test

echo
echo "Fazendo deploy para Railway..."
railway up

echo
echo "Aplicando migrações no Railway..."
railway run dotnet ef database update --project src/TaskManager.Infrastructure --startup-project src/TaskManager.API

echo
echo "========================================"
echo "   Deploy concluído!"
echo "========================================"
echo
echo "Sua API está disponível em:"
railway domain
echo
echo "Swagger UI: https://$(railway domain)/swagger"
echo "API: https://$(railway domain)/api"
echo
