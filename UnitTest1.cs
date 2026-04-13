using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace play;

[TestFixture]
public class TesteGoogle : PageTest
{
    [Test]
    public async Task AbrirGoogleEPesquisar()
    {
        // Abre o Google
        await Page.GotoAsync("https://www.google.com");

        // Aceita cookies se aparecer o botão
        var botaoCookies = Page.Locator("button:has-text('Aceitar tudo')");
        if (await botaoCookies.IsVisibleAsync())
            await botaoCookies.ClickAsync();

        // Digita na barra de pesquisa
        await Page.Locator("textarea[name='q']").FillAsync("Playwright C#");

        // Pressiona Enter
        await Page.Locator("textarea[name='q']").PressAsync("Enter");

        // Espera os resultados carregarem
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Verifica que o título da página contém "Playwright"
        await Expect(Page).ToHaveTitleAsync(new System.Text.RegularExpressions.Regex("Playwright"));

        // Tira um print da tela
        await Page.ScreenshotAsync(new() { Path = "resultado.png" });

        Console.WriteLine("✓ Teste concluído! Screenshot salvo em resultado.png");
    }
}