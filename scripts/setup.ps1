#!/usr/bin/env pwsh

Write-Host "Script directory: $PSScriptRoot"

$projectPath = Join-Path $PSScriptRoot "../src/Ambev.DeveloperEvaluation.ORM/Ambev.DeveloperEvaluation.ORM.csproj"
$startupPath =  Join-Path $PSScriptRoot "../src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj"

Write-Host "Applying Migrations..."
Write-Host "Project: $projectPath"
Write-Host "Startup Project: $startupPath"

# Executa o comando dotnet ef para atualizar o banco de dados
dotnet ef database update --project $projectPath --startup-project $startupPath

# Verifica o código de saída do comando anterior
if ($LASTEXITCODE -eq 0) {
    Write-Host "Migrations aplicadas com sucesso."
} else {
    Write-Host "Erro ao aplicar as migrations."
    exit 1
}
