# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar la solución y todo el código fuente
COPY TransactionService.sln .
COPY src/ ./src/
COPY tests/ ./tests/

# Restaurar dependencias desde la solución
RUN dotnet restore TransactionService.sln

# Publicar el proyecto principal
WORKDIR /app/src/TransactionService.Api
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "TransactionService.Api.dll"]

