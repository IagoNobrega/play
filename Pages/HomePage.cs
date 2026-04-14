using Microsoft.Playwright;

namespace play.Pages;

public class HomePage
{
    private readonly IPage _page;
    private readonly string _baseUrl;

    public HomePage(IPage page, string baseUrl)
    {
        _page = page;
        _baseUrl = baseUrl;
    }

    public async Task AccessLogin()
    {
        await _page.GotoAsync(_baseUrl);
        await _page.GetByRole(AriaRole.Link, new() { Name = " Login" }).ClickAsync();
    }

    public async Task AccessRegister()
    {
        await _page.GotoAsync(_baseUrl);
        await _page.GetByRole(AriaRole.Link, new() { Name = " Cadastro" }).ClickAsync();
    }
}
