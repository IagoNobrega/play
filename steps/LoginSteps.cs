using Microsoft.Playwright;
using TechTalk.SpecFlow;
using play.Pages;

namespace play.Steps;

[Binding]
public class LoginSteps
{
    private IPlaywright _playwright;
    private IBrowser    _browser;
    private IPage       _page;
    private HomePage    _homePage;
    private LoginPage   _loginPage;

    [BeforeScenario]
    public async Task Setup()
    {
        _playwright = await Playwright.CreateAsync();
        _browser    = await _playwright.Chromium.LaunchAsync(new()
        {
            Headless = false
        });
        _page      = await _browser.NewPageAsync();
        _homePage  = new HomePage(_page);
        _loginPage = new LoginPage(_page);
    }

    [AfterScenario]
    public async Task TearDown()
    {
        await _browser.CloseAsync();
        _playwright.Dispose();
    }

    [Given("I am on Login screen")]
    public async Task DadoQueEstouNaTelaDeLogin()
    {
        await _homePage.AccessLogin();
    }

    [Given("I fill e-mail")]
    public async Task PreenchoEmail()
    {
        await _loginPage.FillEmail("iago@teste.com.br");
    }

    [Given(@"I fill e-mail ""(.*)""")]
    public async Task PreenchoEmailComValor(string email)
    {
        await _loginPage.FillEmail(email);
    }

    [Given("I fill my credentials")]
    public async Task PreenchoMinhasCredenciais()
    {
        await _loginPage.FillEmail("iago@teste.com.br");
        await _loginPage.FillPassword("123456");
    }

    [When("I click on Login")]
    public async Task QuandoClicoEmLogin()
    {
        await _loginPage.DoLogin();
    }

    [Then(@"I see the message ""(.*)""")]
    public async Task EntaoVejoMensagemDeErro(string mensagem)
    {
        await _loginPage.CheckErrorMessage(mensagem);
    }

    [Then("I am redirected to the home page")]
    public async Task EntaoLoginComSucesso()
    {
        await _loginPage.CheckSuccessMessage("iago@teste.com.br");
    }
}