# MillyBeads - E-commerce de Miçangas

## 📋 Sobre o Projeto
MillyBeads é uma plataforma de e-commerce especializada em miçangas e acessórios para artesanato. O projeto foi desenvolvido em ASP.NET Core 9.0, utilizando uma arquitetura em camadas para melhor organização e manutenção do código.

## 🚀 Funcionalidades
- **Autenticação e Autorização**
  - Login e registro de usuários
  - Gerenciamento de perfis
  - Controle de acesso baseado em roles (Admin/Usuário)

- **Gerenciamento de Produtos**
  - CRUD completo de produtos
  - Upload de imagens
  - Categorização de produtos

- **Carrinho de Compras**
  - Adicionar/remover produtos
  - Atualizar quantidades
  - Cálculo automático de total

- **Checkout**
  - Processamento de pedidos
  - Histórico de compras
  - Status de entrega

## 🛠️ Tecnologias Utilizadas
- **Backend**
  - ASP.NET Core 9.0
  - Entity Framework Core
  - SQLite
  - JWT para autenticação

- **Frontend**
  - Razor Pages
  - Bootstrap 5
  - JavaScript
  - HTML5/CSS3

## 📁 Estrutura do Projeto
```
MillyBeads/
├── Controllers/         # Controladores da aplicação
├── Models/             # Modelos de dados
│   ├── Entities/       # Entidades do banco de dados
│   ├── ViewModels/     # Modelos de visualização
│   └── DTOs/           # Objetos de transferência de dados
├── Services/           # Serviços de negócio
│   ├── Interfaces/     # Interfaces dos serviços
│   └── Implementations/# Implementações dos serviços
├── Repositories/       # Repositórios para acesso a dados
├── Infrastructure/     # Configurações e utilitários
├── Views/              # Views da aplicação
└── wwwroot/            # Arquivos estáticos
```

## 🔧 Pré-requisitos
- .NET 9.0 SDK
- Visual Studio 2022 (recomendado) ou VS Code
- Git

## 🚀 Como Executar
1. Clone o repositório:
```bash
git clone https://github.com/seu-usuario/milly-beads.git
```

2. Navegue até a pasta do projeto:
```bash
cd milly-beads
```

3. Restaure as dependências:
```bash
dotnet restore
```

4. Execute o projeto:
```bash
dotnet run
```

5. Acesse a aplicação em:
```
https://localhost:5001
```

## 🔒 Configuração de Ambiente
1. Crie um arquivo `appsettings.Development.json` na raiz do projeto:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=app.db"
  },
  "Jwt": {
    "Secret": "sua-chave-secreta-aqui",
    "Issuer": "https://localhost:5001",
    "Audience": "https://localhost:5001"
  }
}
```
