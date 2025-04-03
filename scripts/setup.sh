#!/bin/bash
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

STARTUP_PROJECT="src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj"
PROJECT_PATH="src/Ambev.DeveloperEvaluation.ORM/Ambev.DeveloperEvaluation.ORM.csproj"

echo "Script directory: $DIR"

echo "Applying Migrations..."

dotnet ef database update \
  --project "$DIR/../$PROJECT_PATH" \
  --startup-project "$DIR/../$STARTUP_PROJECT"


if [ $? -eq 0 ]; then
    echo "Migrations aplicadas com sucesso."
else
    echo "Erro ao aplicar as migrations."
    exit 1
fi
 