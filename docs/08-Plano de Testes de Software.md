# Plano de Testes de Software

Os requisitos para a realização dos testes de software são:

- Site publicado na Internet

- Navegador da Internet: Chrome, Firefox ou Edge

- Conectividade de Internet

## Tipos de Testes

| **Caso de Teste**  | **CT01 – Cadastrar perfil de cliente**  |
|:---: |:---: |
| Requisito Associado  | RF-01 - A aplicação deve permitir ao cliente cadastrar uma conta. |
| Objetivo do Teste  | Verificar se o cliente consegue se cadastrar na aplicação. |
| Passos  | - Acessar o navegador <br> - Informar o endereço do site <br> - Clicar em "Criar conta" <br> - Preencher os campos obrigatórios (nome, e-mail, senha, confirmação de senha) <br> - Clicar em "Registrar" |
| Critério de Êxito | - O cadastro foi realizado com sucesso. |
|   |   |
| **Caso de Teste**  | **CT02 – Cadastrar perfil de agente imobiliário** |
| Requisito Associado | RF-02 - A aplicação deve permitir aos agentes imobiliários cadastrar uma conta. |
| Objetivo do Teste  | Verificar se o agente imobiliário consegue se cadastrar na aplicação. |
| Passos  | - Acessar o navegador <br> - Informar o endereço do site <br> - Clicar em "Criar conta" <br> - Selecionar a opção "Agente Imobiliário" <br> - Preencher os campos obrigatórios (nome, e-mail, senha, confirmação de senha, CRECI) <br> - Clicar em "Registrar" |
| Critério de Êxito | - O cadastro foi realizado com sucesso. |
|   |   |
| **Caso de Teste**  | **CT03 – Efetuar login** |
| Requisito Associado | RF-03 - A aplicação deve permitir aos usuários fazer o login. |
| Objetivo do Teste  | Verificar se o usuário consegue realizar login. |
| Passos  | - Acessar o navegador <br> - Informar o endereço do site <br> - Clicar no botão "Entrar" <br> - Preencher o campo de e-mail <br> - Preencher o campo da senha <br> - Clicar em "Login" |
| Critério de Êxito | - O login foi realizado com sucesso. |
|   |   |
| **Caso de Teste**  | **CT04 – Cadastrar imóvel** |
| Requisito Associado | RF-04 - A aplicação deve permitir aos agentes imobiliários cadastrar imóveis. |
| Objetivo do Teste  | Verificar se o agente imobiliário consegue cadastrar um imóvel na plataforma. |
| Passos  | - Acessar o navegador <br> - Informar o endereço do site <br> - Fazer login como agente imobiliário <br> - Acessar a seção de "Cadastrar Imóvel" <br> - Preencher os campos obrigatórios (endereço, preço, descrição, tipo de imóvel) <br> - Clicar em "Salvar" |
| Critério de Êxito | - O imóvel foi cadastrado com sucesso. |
|   |   |
| **Caso de Teste**  | **CT05 – Pesquisar imóveis** |
| Requisito Associado | RF-05 - A aplicação deve oferecer aos clientes funcionalidades de filtro/pesquisa para seleção dos imóveis disponíveis. |
| Objetivo do Teste  | Verificar se o cliente consegue pesquisar imóveis conforme os filtros disponíveis. |
| Passos  | - Acessar o navegador <br> - Informar o endereço do site <br> - Fazer login como cliente <br> - Acessar a seção de "Pesquisar Imóveis" <br> - Utilizar os filtros disponíveis (preço, localização, tipo de imóvel, etc.) <br> - Clicar em "Buscar" |
| Critério de Êxito | - A lista de imóveis filtrados é exibida corretamente. |
|   |   |
| **Caso de Teste**  | **CT06 – Upload de fotos e vídeos** |
| Requisito Associado | RF-06 - A aplicação deve permitir aos agentes imobiliários fazer upload de fotos e vídeos. |
| Objetivo do Teste  | Verificar se o agente imobiliário consegue enviar imagens e vídeos para um anúncio. |
| Passos  | - Acessar o navegador <br> - Informar o endereço do site <br> - Fazer login como agente imobiliário <br> - Acessar a seção de "Meus Imóveis" <br> - Selecionar um imóvel cadastrado <br> - Clicar em "Adicionar Mídia" <br> - Fazer upload de imagens e vídeos <br> - Clicar em "Salvar" |
| Critério de Êxito | - As mídias foram enviadas e aparecem corretamente no anúncio. |
|   |   |
| **Caso de Teste**  | **CT07 – Exibir mapas** |
| Requisito Associado | RF-07 - A aplicação deve permitir integração com APIs externas para exibição de mapas. |
| Objetivo do Teste  | Verificar se os imóveis possuem mapas com a localização correta. |
| Passos  | - Acessar o navegador <br> - Informar o endereço do site <br> - Acessar a página de um imóvel <br> - Verificar se o mapa é carregado corretamente com a localização do imóvel |
| Critério de Êxito | - O mapa é exibido corretamente. |
|   |   |
| **Caso de Teste**  | **CT08 – Chat entre compradores e vendedores** |
| Requisito Associado | RF-08 - A aplicação deve disponibilizar um chat para comunicação entre compradores e vendedores. |
| Objetivo do Teste  | Verificar se o chat funciona corretamente entre usuários. |
| Passos  | - Acessar o navegador <br> - Informar o endereço do site <br> - Fazer login como cliente <br> - Acessar um anúncio de imóvel <br> - Clicar em "Enviar mensagem" <br> - Digitar uma mensagem e enviar |
| Critério de Êxito | - A mensagem é enviada e recebida corretamente pelo vendedor. |
|   |   |
| **Caso de Teste**  | **CT09 – Gerar relatórios** |
| Requisito Associado | RF-09 - A aplicação deve gerar relatórios para os agentes do mercado imobiliário. |
| Objetivo do Teste  | Verificar se a funcionalidade de geração de relatórios está funcionando corretamente. |
| Passos  | - Acessar o navegador <br> - Informar o endereço do site <br> - Fazer login como agente imobiliário <br> - Acessar a seção "Relatórios" <br> - Escolher um período de análise <br> - Clicar em "Gerar Relatório" |
| Critério de Êxito | - O relatório é gerado corretamente com as informações de desempenho dos imóveis. |
|   |   |
| **Caso de Teste**  | **CT10 – Avaliação e comentários nos anúncios** |
| Requisito Associado | RF-10 - A aplicação deve possibilitar avaliações e comentários nos anúncios. |
| Objetivo do Teste  | Verificar se os usuários conseguem avaliar e comentar em um anúncio de imóvel. |
| Passos  | - Acessar o navegador <br> - Informar o endereço do site <br> - Fazer login como cliente <br> - Acessar a página de um imóvel <br> - Preencher o campo de avaliação (ex: nota de 1 a 5) <br> - Digitar um comentário <br> - Clicar em "Publicar" |
| Critério de Êxito | - A avaliação e o comentário foram publicados com sucesso. |









