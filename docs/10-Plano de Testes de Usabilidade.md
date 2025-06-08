# Plano de Testes de Usabilidade

O teste de usabilidade é uma ferramenta para avaliar a funcionalidade de um site, e garantir que as pessoas possam navegar nele com eficiência. 

Para o projeto ClickImóveis realizaremos testes remotos e não moderados.

## 1. Objetivos

- Verificar se os usuários conseguem concluir tarefas essenciais sem dificuldades;
- Identificar barreiras na navegação e interação com o sistema;
- Avaliar a eficiência e a satisfação do usuário ao utilizar a interface.

## 2. Participantes

Serão selecionados 5 participantes onde cada um representa uma das personas identificadas neste trabalho.
São requisitos para que cada participante possa participar:

- Possuir conexão com internet estável;
- Possuir webcam móvel para gravação da própria tela;
- Possuir notebook ou computador desktop para uso da webcam;

## 3. Cenários de Teste

### Cenário de Teste 1: CT-01

**Objetivo:** Avaliar a facilidade e eficiência do usuário em pesquisar, visualizar dados do imóvel e obter dados de contato do anunciante.

**Contexto:** O usuário deseja buscar um imóvel na cidade onde mora. Para isso, ele deverá entrar no site www.clickimoveis.com.br para buscar opções disponíveis, visualizar os dados do imóvel e obter dados de contato do anunciante.

**Tarefa(s):** 
- Acessar o site e localizar a barra de pesquisa.
- Pesquisar por "Belo Horizonte, MG" e utilizar os filtros para refinar a busca (exemplo: apartamento, casa, aluguel, venda, etc).
- Escolher um dos imóveis listados e acessar a página do imóvel.
- Acessar dados de contato do anunciante.

**Critério(s) de Sucesso(s):**
- O usuário consegue encontrar e filtrar os imóveis sem dificuldades.
- Os dados detalhados do imóvel são mostrados corretamente.
- O usuário acessa os dados de contato do anunciante sem dificuldades.
- Todo o processo ocorre em menos de 5 minutos, sem necessidade de assistência.

### Cenário de Teste 2: CT-02

**Objetivo:** Avaliar telas de login e cadastro de usuários.

**Contexto:** O usuário deseja realizar cadastro e login na plataforma.

**Tarefa(s):**
- Acessar o site sem estar previamente logado.
- Selecionar cadastrar uma conta na tela de login.
- Cadastrar uma nova conta de usuário na tela de cadastro.
- Retornar para a tela de login e realizar o login.
- Visualizar a área logada do site.

**Critério(s) de Sucesso(s):**
- O usuário deve identificar a opção de cadastro de novo usuário.
- O cadastro de novo usuário deve ocorrer de modo a não gerar dúvidas.
- O login com o novo usuário criado deve ocorrer sem problemas.


### Cenário de Teste 3: CT-03

**Objetivo:** Avaliar cadastro de imóveis.

**Contexto:** O usuário deseja realizar cadastro de imóvel.

**Tarefa(s):**
- Estar na página inicial da aplicação.
- Selecionar menu cadastrar imóvel.
- Cadastrar dados do imóvel.
- Fazer ulpoad de fotos e/ou vídeos.
- Visualizar novo imóvel como já cadastrado.

**Critério(s) de Sucesso(s):**
- O usuário deve conseguir cadastrar o imóvel.
- Caso as fotos e/ou vídeos não sejam compatíveis deve haver aviso claro ao usuário.
- Após o cadastro o imóvel deve aparecer na lista de já cadastrados.


### Cenário de Teste 4: CT-04

**Objetivo:** Avaliar criação de anúncios.

**Contexto:** O usuário deseja realizar a criação de um anúncio.

**Tarefa(s):**
- Estar na página inicial da aplicação.
- Selecionar menu criar anúncio.
- Criar novo anúncio.
- Visualizar novo anúncio na lista de anúncios criados.

**Critério(s) de Sucesso(s):**
- O usuário deve conseguir criar o anúncio.
- Após a criação do anúncio deve aparecer na lista de já criados.

### Cenário de Teste 5: CT-05

**Objetivo:** Avaliar criação de relatórios.

**Contexto:** O usuário deseja gerar relatórios.

**Tarefa(s):**
- Estar na página inicial da aplicação.
- Selecionar menu relatórios.
- Selecionar um dos tipos de relatórios disponíveis e selecionar gerar.
- Visualizar o relatório em tela.
- Selecionar a opção de salvar em formato PDF.

**Critério(s) de Sucesso(s):**
- O usuário deve conseguir gerar o relatório.
- O usuário deve visualizar o relatório em tela.
- O download do relatório ocorre sem problemas.

## Métodos de coleta de dados

Cada usuário selecionado deverá preencher a planilha abaixo à medida que for sendo realizado os testes de usabilidade. Posteriormente as informações geradas por cada usuário serão consolidadas no relatório do resultado dos testes de usabilidade.

| **Caso de Teste** 	| **Início hh:mm:ss** 	| **Término hh:mm:ss** | **Tarefa realizada sem erros? (sim ou não)** | **Erros observados** | **Comentários e observações** |
| --- 	| --- 	| --- | ---  | --- | --- | 
| CT-01	| 22:18 	| 22:20 | Não   | A pesquisa inicial não retornou resultado |  A barra de pesquisa é fácil de achar, mas os filtros deram erro. Tive que trocar cidade e quartos para conseguir ver algo.| 
| CT-02 |  22:54	| 22:55 | Sim  | Não é um erro, mas sim uma crítica, pois no login, tem as opções de usuário, corretor, administrador | Interface bem fácil de navegar, foi bem simples fazer o login na página.| 
| CT-03	| 23:09 	| 23:11 |  Sim | Nenhum | Bem fácil de cadastrar um imóvel, as opções são bem claras e os botões são bem visíveis. | 
| CT-04	| 23:17 	| 23:19 |  Sim | Nenhum | Os botões são bem fáceis de ver, bem coloridos, mas falta algumas explicações no cadastro e também no botão de deletar, que se clicar um avez já exclui o anúncio|
| CT-05|  23:25	| 23:28 |  Não | Botões que não levam as páginas | os botões de exportar para Excel e PDF, não estão funcionando, e também a busca nos relatório não está funcionando |

<hr>

                                                                                          

| CT-01	 Nayara Pereira   | https://github.com/user-attachments/assets/3cff6c5e-4f6c-4185-9963-0a580298925d   |
| ------|-----------------------------------------------------------------------------|

| CT-02 Luiz Felipe	| https://github.com/user-attachments/assets/7fc95a6d-c79f-4755-9529-dd31d690a17c |
| ------|-------------------------|

| CT-03 Luiz Augusto	| https://github.com/user-attachments/assets/7fc95a6d-c79f-4755-9529-dd31d690a17c |
| ------|-------------------------|

| CT-04 Diego Almeida	| https://github.com/user-attachments/assets/f8257a12-9e26-4ecc-8f8b-749525523f38 |
| ------|-------------------------|

| CT-05 Isaias Goulart	|https://github.com/user-attachments/assets/e9e40d10-9105-47b4-a94e-749c09199bec |
| ------|-------------------------|















