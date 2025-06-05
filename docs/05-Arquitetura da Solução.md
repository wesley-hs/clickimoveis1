# Arquitetura da Solução

<span style="color:red">Pré-requisitos: <a href="3-Projeto de Interface.md"> Projeto de Interface</a></span>

Definição de como o software é estruturado em termos dos componentes que fazem parte da solução e do ambiente de hospedagem da aplicação.

## Diagrama de Classes

O diagrama de classes ilustra graficamente como será a estrutura do software, e como cada uma das classes da sua estrutura estarão interligadas. Essas classes servem de modelo para materializar os objetos que executarão na memória.



![Diagrama de Classe](/docs/img/Diagrama-Classe.jpg)


## Modelo ER (Projeto Conceitual)

O Modelo ER representa através de um diagrama como as entidades (coisas, objetos) se relacionam entre si na aplicação interativa.






![Entidade-Relacionamento](https://github.com/user-attachments/assets/5acc07cb-c0fb-4ee6-8a40-c351475d17e8)




## Projeto da Base de Dados

O projeto da base de dados corresponde à representação das entidades e relacionamentos identificadas no Modelo ER, no formato de tabelas, com colunas e chaves primárias/estrangeiras necessárias para representar corretamente as restrições de integridade.
 
Para mais informações, consulte o microfundamento "Modelagem de Dados".

![Base de Dados](/docs/img/Diagrama-Base-Dados.jpeg)




## Tecnologias Utilizadas

### Linguagem de Programação

- C#: linguagem principal;
- Javascript: pequenos scripts no lado do cliente.

### Framework

- ASP.NET Core;

### Ferramentas e IDEs de Desenvolvimento

- Visual Studio Community 2022: IDE para desenvolvimento;
- Git e GitHub: controle de versão;
- Figma: wireframes e layouts;
- Draw.io: diagramas, fluxogramas e desenhos.


## Hospedagem

A hospedagem utilizada é o serviço de VPS da Hostinger. O servidor possui sistema operacional linux Ubuntu 22.04. Instalado o .NET Runtime e o MS SQL Server Express. Disponível em: http://212.85.10.208/

