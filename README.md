# Sistema de Gerenciamento de Atendimentos Docentes

## 📌 Sobre o Projeto
Esta API, desenvolvida utilizando **.NET 8**, adota os princípios do **Domain-Driven Design (DDD)** para oferecer uma solução estruturada e eficaz no gerenciamento de registros de atendimentos docentes realizados por coordenadores pedagógicos.

O principal objetivo é permitir que os usuários registrem seus atendimentos realizados com os docentes, detalhando informações como **nome do docente, data e hora, assunto tratado, encaminhamentos e observações**, com os dados sendo armazenados de forma segura em um banco de dados **MySQL**.

A arquitetura da API baseia-se em **REST**, utilizando métodos **HTTP** padrão para uma comunicação eficiente e simplificada. Além disso, a API é complementada por uma documentação **Swagger**, que proporciona uma interface gráfica interativa para que desenvolvedores possam explorar e testar os endpoints de maneira fácil.

Para garantir a segurança da aplicação, o sistema utiliza **autenticação baseada em tokens JWT (JSON Web Token)**. Dessa forma, apenas usuários autenticados podem acessar os recursos protegidos da API.

Também foi implementado um sistema de **autorização por papéis (roles)**, permitindo diferentes níveis de acesso no sistema. Atualmente existem **três tipos de usuários**:

- **Admin**
- **Coordenador**
- **Outros**

Essas permissões controlam quais endpoints cada tipo de usuário pode acessar ou executar.

Dentre os pacotes **NuGet** utilizados:

- **AutoMapper** é responsável pelo mapeamento entre objetos de domínio e requisição/resposta, reduzindo código repetitivo.
- **FluentValidation** é utilizado para implementar regras de validação de forma clara e organizada nas classes de requisição.
- **BCrypt** é utilizado para **criptografar senhas dos usuários**, garantindo a segurança de informações sensíveis.
- **EntityFramework** atua como um **ORM (Object-Relational Mapper)** que simplifica as interações com o banco de dados, permitindo manipular dados utilizando objetos .NET em vez de consultas SQL diretas.

Além disso, o projeto conta com uma estratégia de **testes automatizados**, incluindo:

- **Testes de UseCase** utilizando **Moq** para simulação de dependências e validação das regras de negócio.
- **Testes de Integração**, garantindo que os diferentes componentes da aplicação funcionem corretamente quando integrados.

## ✨ Features
As principais funcionalidades do projeto são:

- Cadastro e gerenciamento de atendimentos docentes.
- **Autenticação com JWT** para acesso seguro à API.
- **Controle de autorização por roles** (Admin, Coordenador e Outros).
- Login de usuário com **senhas criptografadas utilizando BCrypt**.
- Geração de relatórios de atendimentos em **Excel**.
- Geração de relatórios de atendimentos em **PDF**.
- Controle de permissões: somente coordenadores podem registrar atendimentos e gerar relatórios.

## 🛠 Construído com
![dot-net-badge]
![mysql-badge]
![swagger-badge]

## 🚀 Getting Started

Para obter uma cópia local funcionando, siga os passos:

### 📋 Requisitos
Para executar este projeto, você precisará ter instalado:
- [SDK do .NET 8](https://dotnet.microsoft.com/download)
- [Visual Studio](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)
- [MySQL Server](https://dev.mysql.com/downloads/mysql/)

### 🔧 Instalação
1. Clone o repositório:
   ```bash
   git clone https://github.com/Vanilson11/registroAtendimentoDocente.git
   ```

2. Preencha as informações de connectionString no arquivo `appsettings.Development.json`.
3. Execute a API e aproveite o seu teste!

## 📌 Status do Projeto
🚧 **Finalizado**✅

<!--[hero-image]: src/images/heorimage.jpg -->

<!-- Badges -->
[dot-net-badge]: https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=fff&style=flat-square
[mysql-badge]: https://img.shields.io/badge/MySQL-4479A1?logo=mysql&logoColor=fff&style=flat-square

[swagger-badge]: https://img.shields.io/badge/Swagger-85EA2D?logo=swagger&logoColor=000&style=flat-square

