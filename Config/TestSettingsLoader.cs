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
        var path = Path.Combine(AppContext.BaseDirectory, "testsettings.json");
        if (!File.Exists(path))
        {
            return new TestSettings();
        }

        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<TestSettings>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new TestSettings();
    }
}
