using Microsoft.Playwright;

namespace play.Pages;

public class HomePage
{
    private readonly IPage _page;
    private const string URL = "https://automationpratice.com.br/";

    public HomePage(IPage page)
    {
        _page = page;
    }

    public async Task AccessLogin()
    {
        await _page.GotoAsync(URL);
        await _page.GetByRole(AriaRole.Link, new() { Name = " Login" }).ClickAsync();
    }

    public async Task AccessRegister()
    {
        await _page.GotoAsync(URL);
        await _page.GetByRole(AriaRole.Link, new() { Name = " Cadastro" }).ClickAsync();
    }
}