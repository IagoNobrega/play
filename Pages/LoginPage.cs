using Microsoft.Playwright;
using NUnit.Framework;

namespace play.Pages;

public class LoginPage
{
    private readonly IPage _page;
    private readonly ILocator _emailInput;
    private readonly ILocator _passwordInput;
    private readonly ILocator _loginButton;
    private readonly ILocator _alertTitle;
    private readonly ILocator _alertBody;

    public LoginPage(IPage page)
    {
        _page = page;
        _emailInput = _page.Locator("#user");
        _passwordInput = _page.Locator("#password");
        _loginButton = _page.GetByRole(AriaRole.Button, new() { Name = "login" });
        _alertTitle = _page.Locator("#swal2-title");
        _alertBody = _page.Locator("#swal2-html-container");
    }

    public async Task WaitUntilLoaded()
    {
        await _emailInput.WaitForAsync();
        await _passwordInput.WaitForAsync();
        await _loginButton.WaitForAsync();
    }

    public async Task FillEmail(string email)
    {
        await _emailInput.ClickAsync();
        await _emailInput.FillAsync(email);
    }

    public async Task FillPassword(string password)
    {
        await _passwordInput.ClickAsync();
        await _passwordInput.FillAsync(password);
    }

    public async Task DoLogin()
    {
        await _loginButton.ClickAsync();
    }

    public async Task CheckErrorMessage(string message)
    {
        var elemento = _page.GetByText(message);
        await elemento.WaitForAsync();
        Assert.That(await elemento.IsVisibleAsync(), Is.True,
            $"Mensagem '{message}' nao encontrada.");
    }

    public async Task CheckSuccessMessage(string email)
    {
        await _alertTitle.WaitForAsync();
        Assert.That(await _alertTitle.InnerTextAsync(), Does.Contain("Login realizado"));
        Assert.That(await _alertBody.InnerTextAsync(), Does.Contain(email));
    }

    public async Task TakeScreenshot(string filename)
    {
        var directory = Path.Combine(AppContext.BaseDirectory, "artifacts");
        Directory.CreateDirectory(directory);

        var path = Path.Combine(directory, $"{filename}.png");
        await _page.ScreenshotAsync(new() { Path = path, FullPage = true });
        Console.WriteLine($"Screenshot salvo: {path}");
    }
}
