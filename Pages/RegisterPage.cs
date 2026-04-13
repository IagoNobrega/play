using Microsoft.Playwright;
using NUnit.Framework;

namespace play.Pages;

public class RegisterPage
{
    private readonly IPage _page;

    public RegisterPage(IPage page)
    {
        _page = page;
    }

    public async Task FillName(string name)
    {
        await _page.Locator("#user").ClickAsync();
        await _page.Locator("#user").FillAsync(name);
    }

    public async Task FillEmail(string email)
    {
        await _page.Locator("#email").ClickAsync();
        await _page.Locator("#email").FillAsync(email);
    }

    public async Task FillPassword(string password)
    {
        await _page.Locator("#password").ClickAsync();
        await _page.Locator("#password").FillAsync(password);
    }

    public async Task ClickRegister()
    {
        await _page.GetByRole(AriaRole.Button, new() { Name = "Cadastrar" }).ClickAsync();
    }

    public async Task CheckErrorMessage(string message)
    {
        var elemento = _page.GetByText(message);
        await elemento.WaitForAsync();
        Assert.That(await elemento.IsVisibleAsync(), Is.True,
            $"Mensagem '{message}' não encontrada.");
    }

    public async Task CheckSuccessMessage()
    {
        var titulo = _page.GetByRole(AriaRole.Heading, new() { Name = "Cadastro realizado!" });
        await titulo.WaitForAsync();
        Assert.That(await titulo.IsVisibleAsync(), Is.True);
        await _page.GetByRole(AriaRole.Button, new() { Name = "OK" }).ClickAsync();
    }
}