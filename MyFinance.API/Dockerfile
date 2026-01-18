# Estágio 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia os projetos
COPY ["MyFinance.API/MyFinance.API.csproj", "MyFinance.API/"]
COPY ["MyFinance.Application/MyFinance.Application.csproj", "MyFinance.Application/"]
COPY ["MyFinance.Domain/MyFinance.Domain.csproj", "MyFinance.Domain/"]
COPY ["MyFinance.Infrastructure/MyFinance.Infrastructure.csproj", "MyFinance.Infrastructure/"]

# Restaura
RUN dotnet restore "MyFinance.API/MyFinance.API.csproj"

# Copia todo o resto e compila
COPY . .
WORKDIR "/src/MyFinance.API"
RUN dotnet build "MyFinance.API.csproj" -c Release -o /app/build

# Publica
FROM build AS publish
RUN dotnet publish "MyFinance.API.csproj" -c Release -o /app/publish

# Estágio 2: Runtime (Final)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyFinance.API.dll"]