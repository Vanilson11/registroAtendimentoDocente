# Sistema de Gerenciamento de Atendimentos Docentes

## 📌 Sobre o Projeto
Esta API, desenvolvida utilizando .NET 8, adota os princípios do Domain-Driven Design (DDD) para oferecer uma solução estruturada e eficaz no gerenciamento de registros de atendimentos docentes realizados por coordenadores pedagógicos. O principal objetivo é permitir que os usuários (coordenadores pedagógicos) registrem seus atendimentos realizados com os docentes, detalhando informações como nome do docente, data e hora, assunto tratado, encaminhamentos e observações, com os dados sendo armazenados de forma segura em um banco de dados MySQL. A arquitetura da API baseia-se em REST, utilizando métodos HTTP padrão para uma comunicação eficiente e simplificada. Além disso, é complementada por uma documentação Swagger, que proporciona uma interface gráfica interativa para que os desenvolvedores possam explorar e testar os endpoints de maneira fácil. Dentre os pacotes NuGet utilizados, o AutoMapper é o responsável pelo mapeamento entre objetos de domínio e requisição/resposta, reduzindo a necessidade de código repetitivo e manual. Para as validações, o FluentValidation é usado para implementar regras de validação de forma simples e intuitiva nas classes de requisições, mantendo o código limpo e fácil de manter. O **BCrypt** atua para criptografar senhas dos usuários, garantindo a segurança de informações sensíveis. Por fim, o EntityFramework atua como um ORM (Object-Relational Mapper) que simplifica as interações com o banco de dados, permitindo o uso de objetos .NET para manipular dados diretamente, sem a necessidade de lidar com consultas SQL.

## ✨ Features
As principais funcionalidades do projeto são:

- Cadastro e gerenciamento de atendimentos docentes.
- Login de usuário (armazenando as informações de login na base de dados).
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

2. Preencha as informações no arquivo `appsettings.Development.json`.
3. Execute a API e aproveite o seu teste!

## 📌 Status do Projeto
🚧 **Em desenvolvimento**: funcionalidades básicas já implementadas (CRUD de atendimentos, login e relatórios), novas melhorias em andamento.

<!--[hero-image]: src/images/heorimage.jpg -->

<!-- Badges -->
[dot-net-badge]: https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=fff&style=flat-square
[mysql-badge]: https://img.shields.io/badge/MySQL-4479A1?logo=mysql&logoColor=fff&style=flat-square
[swagger-badge]: https://img.shields.io/badge/Swagger-85EA2D?logo=swagger&logoColor=000&style=flat-square