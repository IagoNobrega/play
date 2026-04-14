# Projeto de Automacao com Playwright, SpecFlow e NUnit

Este projeto contem testes automatizados de interface usando `.NET 8`, `Playwright`, `SpecFlow` e `NUnit`.

O foco atual da automacao e validar fluxos principais da aplicacao, como:

- login
- cadastro de usuario

## Tecnologias

- `.NET 8`
- `Microsoft.Playwright`
- `SpecFlow`
- `NUnit`
- `GitHub Actions`

## Estrutura do Projeto

- `features/`: cenarios escritos em Gherkin
- `steps/`: implementacao das steps do SpecFlow
- `Pages/`: Page Objects com a logica de interacao da interface
- `Hooks/`: setup e teardown compartilhados entre os cenarios
- `Config/`: leitura das configuracoes externas dos testes
- `Support/`: massa dinamica e contexto compartilhado da execucao
- `scripts/`: scripts auxiliares para resumo e relatorio
- `bin/Debug/net8.0/artifacts/`: screenshots gerados em falhas

## Pre-requisitos

Antes de executar os testes, tenha instalado:

- `.NET SDK 8`
- navegadores do Playwright

## Instalacao

Restaure os pacotes do projeto:

```powershell
dotnet restore
```

Gere o build inicial:

```powershell
dotnet build
```

Instale os navegadores usados pelo Playwright:

```powershell
pwsh bin/Debug/net8.0/playwright.ps1 install
```

## Configuracao

O projeto usa arquivos de configuracao para evitar dados fixos no codigo.

Arquivos disponiveis:

- `testsettings.json`: configuracao padrao
- `testsettings.qa.json`: configuracao para ambiente de QA

Exemplos de configuracoes disponiveis:

- `BaseUrl`
- `Browser.Headless`
- `Login.Email`
- `Login.Password`
- `Register.DefaultName`
- `Register.DefaultPassword`
- `Register.EmailDomain`

Para executar apontando para o ambiente de QA:

```powershell
$env:TEST_ENVIRONMENT="qa"
dotnet test
```

Para voltar ao ambiente padrao da maquina atual:

```powershell
Remove-Item Env:TEST_ENVIRONMENT
```

## Como Executar os Testes

Executar todos os testes:

```powershell
dotnet test
```

Executar com navegador visivel:

```powershell
$env:PLAYWRIGHT_HEADLESS="false"
dotnet test
```

Voltar ao modo headless:

```powershell
Remove-Item Env:PLAYWRIGHT_HEADLESS
```

## Suites de Teste

As features usam tags para organizacao por suite:

- `@smoke`
- `@regression`
- `@register`

Executar somente a suite smoke:

```powershell
dotnet test --filter TestCategory=smoke
```

Executar somente a suite de regressao:

```powershell
dotnet test --filter TestCategory=regression
```

Executar somente cenarios de cadastro:

```powershell
dotnet test --filter TestCategory=register
```

## Cenarios Cobertos

### Login

- login com e-mail vazio
- login com senha vazia
- login com sucesso

### Cadastro

- campo nome vazio
- campo e-mail vazio
- campo e-mail invalido
- campo senha vazio
- cadastro com sucesso com massa dinamica

## Relatorio HTML Local

Para gerar um relatorio HTML local apos a execucao:

```powershell
dotnet test --logger "trx;LogFileName=test-results.trx" --results-directory TestResults
pwsh ./scripts/Convert-TrxToHtml.ps1 -ResultsPath TestResults -OutputPath TestResults/report.html
```

## Pipeline

O projeto possui workflow em `.github/workflows/tests.yml`.

Esse pipeline:

- restaura dependencias
- compila o projeto
- instala os navegadores do Playwright
- executa os testes por suite
- gera arquivo `.trx`
- gera relatorio HTML complementar
- publica resumo da execucao no GitHub Actions
- envia artefatos com resultados e screenshots
- roda por push, pull request, execucao manual e agendamento
- executa apontando para o ambiente `qa`

## Boas Praticas Aplicadas

- `Page Object Model` para separar regras de tela
- `SpecFlow` para descrever comportamento em Gherkin
- hooks centralizados em uma classe dedicada
- configuracao externa para evitar dados fixos no codigo
- massa de cadastro gerada dinamicamente para evitar conflito entre execucoes
- screenshot automatica quando um cenario falha

## Observacoes

- Os arquivos `.feature` usam descricoes em portugues com steps em ingles.
- O projeto esta configurado como projeto de teste em `play.csproj`.
- O ambiente de QA pode ser ajustado no arquivo `testsettings.qa.json`.
