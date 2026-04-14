using Microsoft.Playwright;
using play.Config;
using play.Pages;
using play.Support;
using TechTalk.SpecFlow;

namespace play.Hooks;

[Binding]
public sealed class TestHooks
{
    private readonly ScenarioContext _scenarioContext;

    public TestHooks(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [BeforeScenario]
    public async Task Setup()
    {
        var settings = TestSettingsLoader.Load();
        Console.WriteLine($"Running tests on environment '{settings.EnvironmentName}' using base URL '{settings.BaseUrl}'.");
        var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new()
        {
            Headless = GetHeadlessMode(settings)
        });
        var page = await browser.NewPageAsync();

        var session = new ScenarioSession
        {
            Playwright = playwright,
            Browser = browser,
            Page = page,
            HomePage = new HomePage(page, settings.BaseUrl),
            LoginPage = new LoginPage(page),
            RegisterPage = new RegisterPage(page),
            Settings = settings,
            RegisterUser = TestDataFactory.CreateRegisterUser(settings)
        };

        _scenarioContext.Set(session);
    }

    [AfterScenario]
    public async Task TearDown()
    {
        if (!_scenarioContext.TryGetValue(out ScenarioSession? session) || session is null)
        {
            return;
        }

        if (_scenarioContext.TestError is not null)
        {
            var screenshotName = SanitizeFileName(_scenarioContext.ScenarioInfo.Title);
            await session.Page.ScreenshotAsync(new()
            {
                Path = GetArtifactPath($"{screenshotName}-failure.png"),
                FullPage = true
            });
        }

        await session.Page.CloseAsync();
        await session.Browser.CloseAsync();
        session.Playwright.Dispose();
    }

    private static bool GetHeadlessMode(TestSettings settings)
    {
        var value = Environment.GetEnvironmentVariable("PLAYWRIGHT_HEADLESS");
        return value is null
            ? settings.Browser.Headless
            : !string.Equals(value, "false", StringComparison.OrdinalIgnoreCase);
    }

    private static string SanitizeFileName(string value)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitized = new string(value.Select(character =>
            invalidChars.Contains(character) ? '_' : character).ToArray());

        return sanitized.Replace(' ', '_');
    }

    private static string GetArtifactPath(string fileName)
    {
        var directory = Path.Combine(AppContext.BaseDirectory, "artifacts");
        Directory.CreateDirectory(directory);
        return Path.Combine(directory, fileName);
    }
}
