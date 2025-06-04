# Registro de Testes de Software

<span style="color:red">Pré-requisitos: <a href="3-Projeto de Interface.md"> Projeto de Interface</a></span>, <a href="8-Plano de Testes de Software.md"> Plano de Testes de Software</a>

Para cada caso de teste definido no Plano de Testes de Software, realize o registro das evidências dos testes feitos na aplicação pela equipe, que comprovem que o critério de êxito foi alcançado (ou não!!!). Para isso, utilize uma ferramenta de captura de tela que mostre cada um dos casos de teste definidos (obs.: cada caso de teste deverá possuir um vídeo do tipo _screencast_ para caracterizar uma evidência do referido caso).

| **Caso de Teste** 	| **CT01 – Cadastrar perfil usuário e login** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-01	e RF-03 - A aplicação deve permitir aos usuários pessoa física cadastrarem uma conta |
|Registro de evidência | https://github.com/user-attachments/assets/d0c410ae-5e4f-490f-9869-1fd53ffc4f85|



  





| **Caso de Teste** 	| **CT02 – Cadastrar perfil corretor e login** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-02 e RF-03- A aplicação deve permitir aos usuários pessoa jurídica cadastrarem uma conta |
|Registro de evidência | https://github.com/user-attachments/assets/96ab345a-13a7-43dc-828a-5439c90cfa14|














| **Caso de Teste** 	| **CT03 – Cadastro de imóveis**	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-4 - A aplicação deve permitir aos usuários cadastrarem imóveis |
|Registro de evidência | https://github.com/user-attachments/assets/5efdb3dc-f71b-4da8-8aff-0958fc6dd6d4|






## Relatório de testes de software

Os testes focaram na validação do fluxo de cadastro e login, essencial para a segurança e usabilidade do sistema. Foram avaliados:
 

 ### ✅  Pontos Fortes 

- Validação de Dados: O sistema impede o avanço quando dados obrigatórios estão ausentes ou incorretos, garantindo a integridade das informações armazenadas.

- Segurança: A presença de validações básicas contribui para a proteção contra entradas maliciosas e acessos não autorizados.

- Feedback claro de erros
  - Exemplo: Mensagem "E-mail inválido" sob o campo
 
### ⚠️ Fragilidades Encontradas 

- Ausência de Recuperação de Senha: Não oferecer uma opção para redefinir a senha em caso de esquecimento compromete a acessibilidade do sistema.

- Problema:
O formulário só solicita e-mail, senha e nome completo, ignorando dados essenciais como:

Telefone para contato

CPF/CNPJ (validação de identidade)

Endereço (para corretores/imobiliárias)


### 💡Estratégias de Melhoria Integral 

Aprimoramento do Cadastro de Perfil
Adoção de Cadastro Progressivo:

Fase 1: Dados básicos (nome, e-mail, senha)

Fase 2: Complemento obrigatório (telefone, CPF/CNPJ, data de cadastro)


<hr>

Apresente e discuta detalhadamente os resultados obtidos nos testes realizados, destacando tanto os pontos fortes quanto as fragilidades identificadas na solução. Explique como os aspectos positivos contribuem para o desempenho e a usabilidade do sistema, e como os pontos fracos impactam sua eficácia.

Descreva as principais falhas detectadas durante os testes, fornecendo exemplos concretos e evidências que sustentem essas observações. Explicite os impactos dessas falhas na experiência do usuário, na funcionalidade do sistema e nos objetivos do projeto.

Com base nessas análises, detalhe as estratégias que o grupo pretende adotar para corrigir as deficiências e aprimorar a solução nas próximas iterações. Inclua ações específicas, como ajustes no código, modificações na interface, otimizações de desempenho ou melhorias na acessibilidade e usabilidade.

Por fim, apresente e/ou proponha as melhorias a partir dos testes realizados, destacando os ganhos obtidos e como essas alterações contribuem para a evolução do projeto.

> **Links Úteis**:
> - [Ferramentas de Test para Java Script](https://geekflare.com/javascript-unit-testing/)
