using System.Text.Json;

namespace play.Config;

public static class TestSettingsLoader
{
    private static readonly Lazy<TestSettings> CachedSettings = new(LoadInternal);

    public static TestSettings Load()
    {
        return CachedSettings.Value;
    }

    private static TestSettings LoadInternal()
    {
        var environment = Environment.GetEnvironmentVariable("TEST_ENVIRONMENT") ?? "local";
        var defaultSettings = ReadSettingsFile("testsettings.json");
        var environmentSettings = ReadSettingsFile($"testsettings.{environment}.json");

        var settings = defaultSettings ?? new TestSettings();
        if (environmentSettings is not null)
        {
            settings = Merge(settings, environmentSettings);
        }

        settings.EnvironmentName = environment;
        return settings;
    }

    private static TestSettings? ReadSettingsFile(string fileName)
    {
        var path = Path.Combine(AppContext.BaseDirectory, fileName);
        if (!File.Exists(path))
        {
            return null;
        }

        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<TestSettings>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    private static TestSettings Merge(TestSettings current, TestSettings overrideSettings)
    {
        current.BaseUrl = overrideSettings.BaseUrl;
        current.Browser.Headless = overrideSettings.Browser.Headless;
        current.Login.Email = overrideSettings.Login.Email;
        current.Login.Password = overrideSettings.Login.Password;
        current.Register.DefaultName = overrideSettings.Register.DefaultName;
        current.Register.DefaultPassword = overrideSettings.Register.DefaultPassword;
        current.Register.EmailDomain = overrideSettings.Register.EmailDomain;
        return current;
    }
}
