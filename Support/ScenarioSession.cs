using Microsoft.Playwright;
using play.Config;
using play.Pages;

namespace play.Support;

public sealed class ScenarioSession
{
    public required IPlaywright Playwright { get; init; }
    public required IBrowser Browser { get; init; }
    public required IPage Page { get; init; }
    public required HomePage HomePage { get; init; }
    public required LoginPage LoginPage { get; init; }
    public required RegisterPage RegisterPage { get; init; }
    public required TestSettings Settings { get; init; }
    public required RegisterUserData RegisterUser { get; init; }
}
