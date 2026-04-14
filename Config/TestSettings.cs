namespace play.Config;

public sealed class TestSettings
{
    public string BaseUrl { get; set; } = "https://automationpratice.com.br/";
    public BrowserSettings Browser { get; set; } = new();
    public LoginSettings Login { get; set; } = new();
    public RegisterSettings Register { get; set; } = new();
}

public sealed class BrowserSettings
{
    public bool Headless { get; set; } = true;
}

public sealed class LoginSettings
{
    public string Email { get; set; } = "iago@teste.com.br";
    public string Password { get; set; } = "123456";
}

public sealed class RegisterSettings
{
    public string DefaultName { get; set; } = "Iago Teste";
    public string DefaultPassword { get; set; } = "123456";
    public string EmailDomain { get; set; } = "teste.com.br";
}
