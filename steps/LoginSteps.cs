using Microsoft.Playwright;
using TechTalk.SpecFlow;
using play.Pages;

namespace play.Steps;

[Binding]
public class LoginSteps
{
    private const string ValidEmail = "iago@teste.com.br";
    private const string ValidPassword = "123456";

    private readonly ScenarioContext _scenarioContext;
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private IPage? _page;
    private HomePage? _homePage;
    private LoginPage? _loginPage;

    public LoginSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [BeforeScenario]
    public async Task Setup()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new()
        {
            Headless = GetHeadlessMode()
        });
        _page = await _browser.NewPageAsync();
        _homePage = new HomePage(_page);
        _loginPage = new LoginPage(_page);
    }

    [AfterScenario]
    public async Task TearDown()
    {
        if (_scenarioContext.TestError is not null && _loginPage is not null)
        {
            var screenshotName = SanitizeFileName(_scenarioContext.ScenarioInfo.Title);
            await _loginPage.TakeScreenshot($"{screenshotName}-failure");
        }

        if (_page is not null)
        {
            await _page.CloseAsync();
        }

        if (_browser is not null)
        {
            await _browser.CloseAsync();
        }

        _playwright?.Dispose();
    }

    [Given("I am on Login screen")]
    public async Task DadoQueEstouNaTelaDeLogin()
    {
        await _homePage!.AccessLogin();
        await _loginPage!.WaitUntilLoaded();
    }

    [Given("I fill e-mail")]
    public async Task PreenchoEmail()
    {
        await _loginPage!.FillEmail(ValidEmail);
    }

    [Given(@"I fill e-mail ""(.*)""")]
    public async Task PreenchoEmailComValor(string email)
    {
        await _loginPage!.FillEmail(email);
    }

    [Given("I fill my credentials")]
    public async Task PreenchoMinhasCredenciais()
    {
        await _loginPage!.FillEmail(ValidEmail);
        await _loginPage.FillPassword(ValidPassword);
    }

    [When("I click on Login")]
    public async Task QuandoClicoEmLogin()
    {
        await _loginPage!.DoLogin();
    }

    [Then(@"I see the message ""(.*)""")]
    public async Task EntaoVejoMensagemDeErro(string mensagem)
    {
        await _loginPage!.CheckErrorMessage(mensagem);
    }

    [Then("I am redirected to the home page")]
    public async Task EntaoLoginComSucesso()
    {
        await _loginPage!.CheckSuccessMessage(ValidEmail);
    }

    private static bool GetHeadlessMode()
    {
        var value = Environment.GetEnvironmentVariable("PLAYWRIGHT_HEADLESS");
        return !string.Equals(value, "false", StringComparison.OrdinalIgnoreCase);
    }

    private static string SanitizeFileName(string value)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitized = new string(value.Select(character =>
            invalidChars.Contains(character) ? '_' : character).ToArray());

        return sanitized.Replace(' ', '_');
    }
}
