# Projeto de Automacao com Playwright + SpecFlow

Este projeto contem testes automatizados de interface usando `.NET 8`, `Playwright`, `NUnit` e `SpecFlow`.

O objetivo e validar fluxos principais da aplicacao, como:

- login
- cadastro de usuario

## Tecnologias

- `.NET 8`
- `Microsoft.Playwright`
- `NUnit`
- `SpecFlow`
- `LivingDoc`

## Estrutura do Projeto

- `features/`: cenarios escritos em Gherkin
- `steps/`: implementacao das steps do SpecFlow
- `Pages/`: Page Objects com a logica de interacao da UI
- `Hooks/`: setup e teardown compartilhados entre os cenarios
- `Config/`: leitura das configuracoes externas do teste
- `Support/`: massa dinamica e contexto compartilhado da execucao
- `scripts/`: scripts auxiliares, incluindo resumo do pipeline
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

Instale os navegadores usados pelo Playwright:

```powershell
pwsh bin/Debug/net8.0/playwright.ps1 install
```

Se o script ainda nao existir, faca antes:

```powershell
dotnet build
pwsh bin/Debug/net8.0/playwright.ps1 install
```

## Configuracao

Os dados fixos do projeto ficam em `testsettings.json`.

Nele voce pode alterar:

- `BaseUrl`
- `Browser.Headless`
- `Login.Email`
- `Login.Password`
- `Register.DefaultName`
- `Register.DefaultPassword`
- `Register.EmailDomain`

Exemplo:

```json
{
  "BaseUrl": "https://automationpratice.com.br/",
  "Browser": {
    "Headless": true
  }
}
```

## Como Executar os Testes

Para rodar todos os testes:

```powershell
dotnet test
```

Para rodar com o navegador visivel:

```powershell
$env:PLAYWRIGHT_HEADLESS="false"
dotnet test
```

Para voltar ao modo headless:

```powershell
Remove-Item Env:PLAYWRIGHT_HEADLESS
dotnet test
```

## Cenários Cobertos

### Login

- login com e-mail vazio
- login com senha vazia
- login com sucesso

### Cadastro

- nome obrigatorio
- e-mail obrigatorio
- e-mail invalido
- senha obrigatoria
- cadastro com sucesso usando massa dinamica

## Boas Praticas Usadas

- `Page Object Model` para separar regras de tela
- `SpecFlow` para descrever comportamento em Gherkin
- espera explicita de elementos antes de interagir
- screenshot automatica quando um cenario falha
- hooks centralizados em uma classe dedicada
- configuracao isolada em arquivo externo
- massa de cadastro gerada dinamicamente para evitar conflito entre execucoes

## Comandos Uteis

Gerar build do projeto:

```powershell
dotnet build
```

Executar um filtro de testes:

```powershell
dotnet test --filter TestCategory=register
```

## Pipeline

O projeto agora possui workflow em `.github/workflows/tests.yml`.

Esse pipeline:

- restaura dependencias
- compila o projeto
- instala os navegadores do Playwright
- executa os testes automatizados
- gera arquivo `.trx`
- publica resumo da execucao no GitHub Actions
- envia artefatos com resultados e screenshots

## Observacoes

- Os arquivos `.feature` estao usando textos em portugues com steps em ingles.
- O projeto esta configurado como projeto de teste em `play.csproj`.
- O fluxo de login salva screenshot automaticamente em caso de erro para facilitar a analise.

## Proximos Passos Sugeridos

- adicionar tags por tipo de suite, como smoke e regressao
- rodar pipeline tambem por agendamento
- gerar relatorio HTML complementar para consulta local
- integrar execucao com ambiente de QA dedicado
