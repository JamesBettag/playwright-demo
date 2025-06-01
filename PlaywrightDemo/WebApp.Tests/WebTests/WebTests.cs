using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using Xunit;

namespace WebApp.Tests.WebTests;

public class WebTests(WebAppFixture webAppFixture) : PageTest, IClassFixture<WebAppFixture>
{
	private readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(15);

    [Fact]
    public async Task HasTitle()
    {
        // Use Playwright to navigate to the running web app
        await Page.GotoAsync(webAppFixture.Endpoint.ToString());

        await Expect(Page).ToHaveTitleAsync("Home");
    }
}
