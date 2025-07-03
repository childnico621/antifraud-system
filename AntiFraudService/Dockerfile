# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar archivos de solución y restaurar dependencias
COPY *.sln ./
COPY src/ ./src/

WORKDIR /app/src/AntiFraudService.Worker
RUN dotnet restore

# Publicar el Worker
RUN dotnet publish -c Release -o /app/publish

# Imagen final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "AntiFraudService.Worker.dll"]
