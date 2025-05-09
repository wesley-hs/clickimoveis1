# Instruções de utilização

## Instalação do Site

O site em HTML/CSS/JS é um projeto estático, logo pode ser utilizado tanto em servidores...

### Configuração string de conexão

Para rodar o servidor é necessário configurar a string de conexão ao banco de dados MS SQL Server.

  1. Abrir o Gerenciador de Pacotes e digitar: dotnet user-secrets init 
  2. Abrir o Gerenciador de Senhas do Visual Studio
  3. Inserir  e salvar no arquivo secrets.json a string de conexão da máquina de desenvolvimento conforme abaixo. Adaptar a string de acordo com sua máquina. 

   { "ConnectionStrings:DefaultConnection": "server=SEU-SERVIDOR;database=click-    imoveis;Trusted_Connection=true;encrypt=false;" }

## Histórico de versões

### [0.1.0] - DD/MM/AAAA
#### Adicionado
- Adicionado ...
