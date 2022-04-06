using NUnit.Framework;

namespace RendezVous.Application.IntegrationTests;

using static Testing;

public class TestBase
{
    [SetUp]
    public  async Task SetUp()
    {
        await ResetState();
    }
}

public class UserTestBase : TestBase
{
    private (string ProviderId, Guid Id) _currentUser;
    protected (string ProviderId, Guid Id) CurrentUser => _currentUser;
    
    [SetUp]
    public new async Task SetUp()
    {
        await base.SetUp();

        _currentUser = await RunAsUser();
    }
}
