#!/bin/bash
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
PROJECT_PATH="src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj"

echo "Script directory: $DIR"

echo "Applying Migrations..."

dotnet ef database update \
  --project "$DIR/../$PROJECT_PATH" \
  --startup-project "$DIR/../$PROJECT_PATH"


if [ $? -eq 0 ]; then
    echo "Migrations aplicadas com sucesso."
else
    echo "Erro ao aplicar as migrations."
    exit 1
fi
