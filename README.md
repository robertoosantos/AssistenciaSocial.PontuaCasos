# Indicadores de Risco

## Aplicação para desenvolvimento de indicadores de casos da assistência social.


Tabela de conteúdos
=================
<!--ts-->
   * [Sobre](#Sobre)
   * [Tabela de Conteudo](#tabela-de-conteudo)
   * [Como usar](#como-usar)
      * [Pré-Requisitos](#pré-requisitos)
      * [Rodando a aplicação](#rodando-a-aplicação)
      * [Deploy](#deploy)
   * [Funcionalidades](#funcionalidades)
   * [Tecnologias](#tecnologias)
<!--te-->

## Sobre

Aplicação desenvolvida para permitir o registro e gestão de sistema de pontos de casos da assistência social.

Quanto maior a pontuação, maior a gravidade do caso e a importância de intensificar a frequência do acompanhamento.

Esta aplicação foi desenvolvida por [Roberto de Oliveira Santos](https://github.com/robertoosantos), através das necessidades apontadas pela gerente do CREAS Jundiaí [Karine Andressa Canalle](https://m.facebook.com/karine.canalle).

## Como usar

### Pré-requisitos

💻 Localmente

Antes de começar, você vai precisar ter instalado em sua máquina as seguintes ferramentas:

* [Git](https://git-scm.com)
* [.Net 6](https://learn.microsoft.com/pt-br/dotnet/)
* [SQL Server 2019](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads) (*Versão Express sugerida)
* Um editor para trabalhar com o código como [VSCode](https://code.visualstudio.com/)

🐳 Docker

Esta aplicação está pronta para ser executada em container [Docker](https://www.docker.com/).

Ao clonar a aplicação, altere o arquivo [Dockerfile](.devcontainer/Dockerfile), com as variáveis de ambiente do seu servidor SQL SERVER

```Dockerfile
ENV DatabaseServer
ENV DatabaseName
ENV DatabaseUser
ENV DatabasePassword
```

### Rodando a aplicação

Configure as seguintes variáveis de ambiente:

- DatabaseServer
- DatabaseName
- DatabaseUser
- DatabasePassword

Caso deseje utilizar [Google Authentication](https://developers.google.com/identity/oauth2/web/guides/overview), configure os seguinte [user secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=linux#enable-secret-storage):

- Authentication:Google:ClientId
- Authentication:Google:ClientSecret

```bash
# Clone este repositório
$ git clone https://github.com/robertoosantos/AssistenciaSocial.PontuaCasos

# Acesse a pasta do projeto no terminal/cmd
$ cd src/AssistenciaSocial.PontuaCasos.WebApp

# Execute a aplicação em modo de desenvolvimento
$ dotnet run

# O servidor inciará na porta:7176 - acesse <http://localhost:7176>
```

### Deploy

Um exemplo de deploy em Azure App Services poder ser encontrado em:
- [azure-pipelines.yml](azure-pipelines.yml)

### Funcionalidades

- Cadastro de usuário
- Google Login
- Cadastro de categorias de um caso
- Cadastro de itens de uma categoria
- Cadastro de casos
- Cadastro de pessoas em violação
- Cadastro de violências sofridas
- Cadastro de situação das violências
- Cadastro de condições das pessoas em violação
- Cadastro de sistema de pontuação de casos

### Tecnologias

As seguintes ferramentas foram usadas na construção do projeto:

- [Docker](https://www.docker.com/)
- [.Net MVC](https://learn.microsoft.com/pt-br/aspnet/core/mvc/overview?view=aspnetcore-6.0)
- [SQL SERVER](https://learn.microsoft.com/pt-br/sql/sql-server/?view=sql-server-ver16)
- [C# 10](https://learn.microsoft.com/pt-br/dotnet/csharp/whats-new/csharp-10)
- [Bootstrap 5](https://getbootstrap.com/docs/5.0/getting-started/introduction/)
- [Microsoft Azure](https://azure.microsoft.com/pt-br/)
