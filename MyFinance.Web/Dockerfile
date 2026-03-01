# Estágio 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["MyFinance.Web/MyFinance.Web.csproj", "MyFinance.Web/"]
RUN dotnet restore "MyFinance.Web/MyFinance.Web.csproj"
COPY . .
WORKDIR "/src/MyFinance.Web"
RUN dotnet publish "MyFinance.Web.csproj" -c Release -o /app/publish

# Estágio 2: Servidor Web Nginx
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

# Limpa a página padrão do Nginx
RUN rm -rf ./*

# Copia os arquivos do Blazor
COPY --from=build /app/publish/wwwroot .

# Configuração mágica para o roteamento das páginas do Blazor funcionar no Nginx
RUN echo 'server { listen 80; location / { root /usr/share/nginx/html; try_files $uri $uri/ /index.html; } }' > /etc/nginx/conf.d/default.conf

EXPOSE 80