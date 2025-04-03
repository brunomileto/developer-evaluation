#!/usr/bin/env pwsh
# Aplica as migrations do EF Core utilizando caminhos relativos ao diretório deste script

# $PSScriptRoot contém o diretório onde o script está localizado
Write-Host "Script directory: $PSScriptRoot"

# Monta os caminhos absolutos dos arquivos de projeto (ajuste o caminho relativo conforme sua estrutura)
$projectPath = Join-Path $PSScriptRoot "../src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj"
$startupPath = $projectPath  # Se o startup project for o mesmo

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
