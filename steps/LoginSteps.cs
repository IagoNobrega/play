using play.Support;
using TechTalk.SpecFlow;

namespace play.Steps;

[Binding]
[Scope(Feature = "Login")]
public class LoginSteps
{
    private readonly ScenarioContext _scenarioContext;

    public LoginSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given("I am on Login screen")]
    public async Task DadoQueEstouNaTelaDeLogin()
    {
        await Session.HomePage.AccessLogin();
        await Session.LoginPage.WaitUntilLoaded();
    }

    [Given("I fill e-mail")]
    public async Task PreenchoEmail()
    {
        await Session.LoginPage.FillEmail(Session.Settings.Login.Email);
    }

    [Given(@"I fill e-mail ""(.*)""")]
    public async Task PreenchoEmailComValor(string email)
    {
        await Session.LoginPage.FillEmail(email);
    }

    [Given("I fill my credentials")]
    public async Task PreenchoMinhasCredenciais()
    {
        await Session.LoginPage.FillEmail(Session.Settings.Login.Email);
        await Session.LoginPage.FillPassword(Session.Settings.Login.Password);
    }

    [When("I click on Login")]
    public async Task QuandoClicoEmLogin()
    {
        await Session.LoginPage.DoLogin();
    }

    [Then(@"I see the message ""(.*)""")]
    public async Task EntaoVejoMensagemDeErro(string mensagem)
    {
        await Session.LoginPage.CheckErrorMessage(mensagem);
    }

    [Then("I am redirected to the home page")]
    public async Task EntaoLoginComSucesso()
    {
        await Session.LoginPage.CheckSuccessMessage(Session.Settings.Login.Email);
    }

    private ScenarioSession Session => _scenarioContext.Get<ScenarioSession>();
}
