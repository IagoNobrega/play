using play.Config;

namespace play.Support;

public static class TestDataFactory
{
    public static RegisterUserData CreateRegisterUser(TestSettings settings)
    {
        var uniqueValue = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");

        return new RegisterUserData
        {
            Name = $"{settings.Register.DefaultName} {uniqueValue}",
            Email = $"cadastro.{uniqueValue}@{settings.Register.EmailDomain}",
            Password = settings.Register.DefaultPassword
        };
    }
}
