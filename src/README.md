# Instruções de utilização

## Instalação do Site

O site é um projeto ASP.NET Core que utiliza o banco de dados Microsoft SQL Server. Dessa forma deve estar instalado na máquina de desenvolvimento (Windows) os seguintes softwares:
1. SDK do .NET Core (Software Development Kit);
2. Visual Studio IDE (recomendado);
3. Microsoft SQL Server (recomendado edição Express);

### Configuração string de conexão

Para rodar o servidor é necessário configurar a string de conexão ao banco de dados MS SQL Server.

  1. Abrir o Gerenciador de Pacotes e digitar: `dotnet user-secrets init` 
  2. Abrir o Gerenciador de Senhas do Visual Studio
  3. Inserir  e salvar no arquivo secrets.json a string de conexão da máquina de desenvolvimento conforme abaixo. Adaptar a string de acordo com sua máquina. 

```
   {
     "ConnectionStrings:DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=click-imoveis;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
```

## Histórico de versões

### [0.1.0] - DD/MM/AAAA
#### Adicionado
- Adicionado ...
