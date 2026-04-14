using play.Support;
using TechTalk.SpecFlow;

namespace play.Steps;

[Binding]
[Scope(Feature = "Register User")]
public class RegisterSteps
{
    private readonly ScenarioContext _scenarioContext;

    public RegisterSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given("I am on Register screen")]
    public async Task DadoQueEstouNaTelaDeCadastro()
    {
        await Session.HomePage.AccessRegister();
        await Session.RegisterPage.WaitUntilLoaded();
    }

    [Given("I fill name")]
    public async Task PreenchoNome()
    {
        await Session.RegisterPage.FillName(Session.RegisterUser.Name);
    }

    [Given("I fill valid register data")]
    public async Task PreenchoDadosValidosDeCadastro()
    {
        await Session.RegisterPage.FillName(Session.RegisterUser.Name);
        await Session.RegisterPage.FillEmail(Session.RegisterUser.Email);
        await Session.RegisterPage.FillPassword(Session.RegisterUser.Password);
    }

    [Given(@"I fill e-mail ""(.*)""")]
    public async Task PreenchoEmail(string email)
    {
        await Session.RegisterPage.FillEmail(email);
    }

    [Given(@"I fill password ""(.*)""")]
    public async Task PreenchoSenha(string password)
    {
        await Session.RegisterPage.FillPassword(password);
    }

    [When("I click on Register")]
    public async Task QuandoClicoEmCadastrar()
    {
        await Session.RegisterPage.ClickRegister();
    }

    [Then(@"I see message ""(.*)"" on Register")]
    public async Task EntaoVejoMensagemNoCadastro(string message)
    {
        await Session.RegisterPage.CheckErrorMessage(message);
    }

    [Then("I am redirected after register")]
    public async Task EntaoCadastroComSucesso()
    {
        await Session.RegisterPage.CheckSuccessMessage();
    }

    private ScenarioSession Session => _scenarioContext.Get<ScenarioSession>();
}
