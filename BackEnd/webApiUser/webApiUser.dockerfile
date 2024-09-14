# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar el archivo .csproj desde su ruta correcta
COPY ["webApiUser/webApiUser.csproj", "webApiUser/"]

# Restaurar dependencias
RUN dotnet restore "webApiUser/webApiUser.csproj"

# Copiar todo el contenido del proyecto
COPY . .

# Compilar el proyecto
ARG BUILD_CONFIGURATION=Release
RUN dotnet build "webApiUser/webApiUser.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa 2: Publish
FROM build AS publish
RUN dotnet publish "webApiUser/webApiUser.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Exponer puertos (si es necesario exponer más de uno)
EXPOSE 5134

# Copiar los archivos publicados desde la etapa de publish
COPY --from=publish /app/publish .

# Configurar el entrypoint para ejecutar la aplicación
ENTRYPOINT ["dotnet", "webApiUser.dll"]
