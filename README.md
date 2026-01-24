Markdown

# 🚀 MyFinance - Gestão Financeira Enterprise

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Blazor](https://img.shields.io/badge/Blazor-WASM-512BD4?style=for-the-badge&logo=blazor&logoColor=white)
![MudBlazor](https://img.shields.io/badge/MudBlazor-7.0-7467EF?style=for-the-badge&logo=mudblazor&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2019-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)

> **"Mais que uma planilha: Um sistema robusto para gestão de finanças pessoais e empresariais (PJ)."**

---

## 📸 Screenshots

<div align="center">
  <img src="./assets/dashboard.png" alt="Dashboard Principal" width="800"/>
</div>

---

## 📖 Sobre o Projeto

O **MyFinance** é uma solução completa desenvolvida para resolver a dor de profissionais PJ (Pessoa Jurídica) e desenvolvedores que precisam organizar suas finanças. Diferente de apps comuns, o foco aqui é performance, escalabilidade e, futuramente, inteligência tributária para cálculo de impostos (Simples Nacional/Fator R).

O projeto foi construído seguindo rigorosamente os princípios de **Clean Architecture** e **Domain-Driven Design (DDD)**, garantindo um código desacoplado, testável e fácil de manter.

---

## 🏗️ Arquitetura & Tecnologias

Este projeto é um **Monorepo** que aplica padrões de mercado Enterprise:

### Backend (.NET 8)
- **Arquitetura:** Clean Architecture (Domain, Application, Infrastructure, API).
- **Padrões:** CQRS (Command Query Responsibility Segregation) com **MediatR**.
- **ORM:** Entity Framework Core (Code First) com Migrations Manuais.
- **Banco de Dados:** SQL Server 2019.
- **Dependency Injection:** Container nativo do .NET.

### Frontend (Blazor WebAssembly)
- **Framework:** Blazor WASM (Single Page Application rodando no client).
- **UI Library:** **MudBlazor** (Material Design Components).
- **Comunicação:** HTTP Client consumindo API REST via DTOs.
- **UX:** Feedback visual com Snackbars, Modais e Gráficos Interativos.

### Infraestrutura
- **Docker:** Containerização completa da API e do Banco de Dados.
- **Docker Compose:** Orquestração do ambiente de desenvolvimento com um único comando.

---

## ⚡ Como Rodar o Projeto

### Pré-requisitos
- [Docker Desktop](https://www.docker.com/products/docker-desktop) instalado e rodando.
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (Opcional, para debug).

### Passo a Passo

1. **Clone o repositório:**
   ```bash
   git clone [https://github.com/SEU-USUARIO/MyFinance.git](https://github.com/SEU-USUARIO/MyFinance.git)
   cd MyFinance
Suba o ambiente com Docker:

Bash

docker-compose up -d --build
Acesse a Aplicação:

Frontend (Blazor): http://localhost:XXXX (Verifique a porta no terminal)

Scalar API Reference: http://localhost:8080/scalar/v1

API (Swagger): http://localhost:8080/swagger


📂 Estrutura do Projeto
Plaintext

MyFinance/
├── MyFinance.API/            # Camada de Apresentação (REST API)
├── MyFinance.Application/    # Casos de Uso, CQRS (Commands/Queries), DTOs
├── MyFinance.Domain/         # Entidades, Interfaces, Regras de Negócio (Core)
├── MyFinance.Infrastructure/ # EF Core, Repositórios, Mapeamento de Banco
├── MyFinance.Web/            # Frontend em Blazor WebAssembly + MudBlazor
└── docker-compose.yml        # Orquestração dos containers
🗺️ Roadmap (Próximos Passos)
[x] Core: CRUD de Lançamentos e Categorias.

[x] Visual: Dashboard com Gráficos (MudBlazor).

[x] Arquitetura: Implementação completa do padrão CQRS.

[ ] Multi-Contas: Suporte para múltiplas contas bancárias.

[ ] Módulo PJ: Calculadora de Impostos (Simples Nacional) e Fator R.

[ ] Segurança: Autenticação e Autorização com ASP.NET Identity.

🤝 Contribuição
Contribuições são bem-vindas! Se você tiver uma ideia para melhorar a gestão financeira:

Faça um Fork do projeto.

Crie uma Branch para sua Feature (git checkout -b feature/NovaFeature).

Faça o Commit (git commit -m 'Add some AmazingFeature').

Faça o Push (git push origin feature/NovaFeature).

Abra um Pull Request.

📄 Licença
Este projeto está sob a licença MIT. Veja o arquivo LICENSE para mais detalhes.

<div align="center"> <sub>Desenvolvido com 💜 por Thiago Rodrigues</sub> </div>


-----

### 🎨 Dica Extra para ficar "Profissional":

Para gerar a estrutura de pastas bonita (aquela árvore de diretórios ali no meio), você pode usar o comando `tree` no terminal ou sites geradores.

Esse README mostra que você não é apenas um "codificador", mas um **Engenheiro de Software** que se preocupa com documentação e produto.

Boa sorte com o projeto\! 🚀
