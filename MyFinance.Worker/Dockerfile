# Estágio 1: Build (Compilação)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia os arquivos de projeto e restaura as dependências
COPY ["MyFinance.Worker/MyFinance.Worker.csproj", "MyFinance.Worker/"]
COPY ["MyFinance.Domain/MyFinance.Domain.csproj", "MyFinance.Domain/"]
COPY ["MyFinance.Infrastructure/MyFinance.Infrastructure.csproj", "MyFinance.Infrastructure/"]
RUN dotnet restore "MyFinance.Worker/MyFinance.Worker.csproj"

# Copia todo o resto do código
COPY . .
WORKDIR "/src/MyFinance.Worker"
RUN dotnet build "MyFinance.Worker.csproj" -c Release -o /app/build

# Publica o executável final
FROM build AS publish
RUN dotnet publish "MyFinance.Worker.csproj" -c Release -o /app/publish

# Estágio 2: Runtime (Execução - Imagem leve)
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyFinance.Worker.dll"]