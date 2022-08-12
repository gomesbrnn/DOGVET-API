# DogVet API 🐶

Projeto focado em desenvolvimento de uma [API](https://aws.amazon.com/pt/what-is/api), com proposito final de criar uma clínica veterinaria para cachorros com possibilidade de gerenciamento utilizando [CRUDS.](https://devporai.com.br/o-que-e-crud-e-porque-voce-deveria-aprender-a-criar-um/)

<br>

## 🏁 Desafios propostos

### [Regras do projeto](/App/Docs/Clinica%20DOG%20API.pdf)
<br>

## 📂Estrutura do Projeto
<br>

**📂 Controllers**
<p>Contém os controladores com os endpoints para cada entidade.</p>

**📂 Data**
<p>Contém o mapeamento das entidades para o banco de dados.</p>

**📂 Models**
<p>Contém todas as entidades da aplicação.</p>

**📂 DTO**
<p>Contém todas as entidades da aplicação com validações aplicadas.</p>

**📂 Helpers**
<p>Classes Auxiliares que fornecem serviços para demais classes</p>

**📂 Mappings**
<p>Contém o mapeamento das entidades DTO para as entidades padrão</p>

<br>


## ⚙️ Dependências para execução do projeto
<br>
 
- Ter instalado e configurado o MySQL

- Ter instalado o .NET SDK 5

- Configurar conexão local com banco de dados no appsetings.json


<br>


## ▶️ Executando o projeto
<br>

### Configurando conexão local com o banco de dados:
<br>

```bash
Crie uma base dados no workbench e digite o nome dela em #DataBase

{
    "DefaultConnection": "Server=localhost;DataBase=NomeDoBanco;
    port=3306 Uid=root;Pwd=SenhaDoSeuBanco"
}

```

### Acesse o diretorio da aplicação e execute a migração de dados pro banco:
<br>

```bash
# cd App/

# dotnet ef database update
```
### Rode os seguintes comandos para iniciar a aplicação:
<br>

```bash
# dotnet restore
# dotnet build
# dotnet watch run

caso você não seja redirecionado automaticamente, acesse:

# https://localhost:5001/swagger/index.html
```

### Realize a autenticação para conseguir acessar os endpoints da API:
<br>

```bash
Acesse o endpoint: POST/v1/Autenticacao/Login com as seguintes credênciais:

Acesso de Funcionário:

# - E-mail: funcionario@gft.com (tem acesso a todos os endpoints)

# - Senha: funcionario

Acesso de Cliente:

# - E-mail: cliente@gft.com (tem acesso apenas aos próprios endpoints e os dos seus animais)

# - Senha: cliente
```

Ao realizar o login será gerado um token JWT, você deverá inseri-lo no campo **Authorize** do swagger, digitando: **Bearer + (token)** e confirmando.


<br>

## 📌 Endpoints
<br>

![Animais](/App/img/Animais.png)

![Atendimentos](/App/img/Atendimentos.png)

![Autenticação](/App/img/Autenticacao.png)

![Clinicas](/App/img/Clinicas.png)

![Tutores](/App/img/Tutores.png)

![Veterinários](/App/img/Veterinarios.png)

<br>

## 👨🏻‍💻 Tecnologias utilizadas

- [.NET](https://dotnet.microsoft.com/en-us/download/dotnet/5.0)

- [Entitiy Framework Core](https://docs.microsoft.com/pt-br/ef/)

- [AspNetCore MVC](https://docs.microsoft.com/pt-br/aspnet/core/mvc/overview?view=aspnetcore-6.0)

- [AspNetCore Authentication](https://docs.microsoft.com/pt-br/aspnet/core/security/authentication/?view=aspnetcore-5.0)

- [MySQL](https://www.mysql.com/)
 

