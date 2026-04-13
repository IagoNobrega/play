using Microsoft.Playwright;
using NUnit.Framework;

namespace play.Pages;

public class LoginPage
{
    private readonly IPage _page;

    public LoginPage(IPage page)
    {
        _page = page;
    }

    public async Task FillEmail(string email)
    {
        await _page.Locator("#user").ClickAsync();
        await _page.Locator("#user").FillAsync(email);
    }

    public async Task FillPassword(string password)
    {
        await _page.Locator("#password").ClickAsync();
        await _page.Locator("#password").FillAsync(password);
    }

    public async Task DoLogin()
    {
        await _page.GetByRole(AriaRole.Button, new() { Name = "login" }).ClickAsync();
    }

    public async Task CheckErrorMessage(string message)
    {
        var elemento = _page.GetByText(message);
        await elemento.WaitForAsync();
        Assert.That(await elemento.IsVisibleAsync(), Is.True,
            $"Mensagem '{message}' não encontrada.");
    }

    public async Task CheckSuccessMessage(string email)
    {
        var titulo = _page.Locator("#swal2-title");
        var corpo  = _page.Locator("#swal2-html-container");

        await titulo.WaitForAsync();
        Assert.That(await titulo.InnerTextAsync(), Does.Contain("Login realizado"));
        Assert.That(await corpo.InnerTextAsync(),  Does.Contain($"Olá, {email}"));
    }

    public async Task TakeScreenshot(string filename)
    {
        await _page.ScreenshotAsync(new() { Path = $"{filename}.png" });
        Console.WriteLine($"Screenshot salvo: {filename}.png");
    }
}