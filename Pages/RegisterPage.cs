using Microsoft.Playwright;
using NUnit.Framework;
using System.Globalization;
using System.Text;

namespace play.Pages;

public class RegisterPage
{
    private readonly IPage _page;
    private readonly ILocator _nameInput;
    private readonly ILocator _emailInput;
    private readonly ILocator _passwordInput;
    private readonly ILocator _registerButton;

    public RegisterPage(IPage page)
    {
        _page = page;
        _nameInput = _page.Locator("#user");
        _emailInput = _page.Locator("#email");
        _passwordInput = _page.Locator("#password");
        _registerButton = _page.GetByRole(AriaRole.Button, new() { Name = "Cadastrar" });
    }

    public async Task WaitUntilLoaded()
    {
        await _nameInput.WaitForAsync();
        await _emailInput.WaitForAsync();
        await _passwordInput.WaitForAsync();
        await _registerButton.WaitForAsync();
    }

    public async Task FillName(string name)
    {
        await _nameInput.ClickAsync();
        await _nameInput.FillAsync(name);
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

    public async Task ClickRegister()
    {
        await _registerButton.ClickAsync();
    }

    public async Task CheckErrorMessage(string message)
    {
        var elemento = _page.GetByText(message);
        await elemento.WaitForAsync();
        Assert.That(await elemento.IsVisibleAsync(), Is.True,
            $"Mensagem '{message}' não encontrada.");
    }

    public async Task AssertErrorMessage(string message)
    {
        var normalizedExpected = NormalizeText(message);
        var timeoutAt = DateTime.UtcNow.AddSeconds(10);
        var lastVisibleText = string.Empty;

        while (DateTime.UtcNow < timeoutAt)
        {
            lastVisibleText = await _page.Locator("body").InnerTextAsync();
            var normalizedVisibleText = NormalizeText(lastVisibleText);

            if (normalizedVisibleText.Contains(normalizedExpected))
            {
                return;
            }

            await Task.Delay(250);
        }

        Assert.Fail($"Mensagem '{message}' nao encontrada. Texto visivel atual: {lastVisibleText}");
    }

    public async Task CheckSuccessMessage()
    {
        var titulo = _page.GetByRole(AriaRole.Heading, new() { Name = "Cadastro realizado!" });
        await titulo.WaitForAsync();
        Assert.That(await titulo.IsVisibleAsync(), Is.True);
        await _page.GetByRole(AriaRole.Button, new() { Name = "OK" }).ClickAsync();
    }

    private static string NormalizeText(string value)
    {
        var normalized = value.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder();

        foreach (var character in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
            {
                builder.Append(char.ToLowerInvariant(character));
            }
        }

        return builder.ToString().Normalize(NormalizationForm.FormC);
    }
}
