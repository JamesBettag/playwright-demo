using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using Xunit;

namespace WebApp.Tests.WebTests;

public class WebTests(WebAppFixture webAppFixture) : PageTest, IClassFixture<WebAppFixture>
{
    [Fact]
    public async Task Home_HasTitle()
    {
        // Use Playwright to navigate to the running web app
        await Page.GotoAsync(webAppFixture.Endpoint.ToString());

        await Expect(Page).ToHaveTitleAsync("Home");
    }

    [Fact]
    public async Task Counter_Counts()
    {
	    await Page.GotoAsync(webAppFixture.Endpoint.ToString());

	    await Page.GetByRole(AriaRole.Link, new() { Name = "Counter" }).ClickAsync();

	    for (var i = 0; i < 5; i++)
	    {
		    await Page.ClickAsync("text=Click me");
		    await Expect(Page.GetByRole(AriaRole.Status)).ToHaveTextAsync($"Current count: {i + 1}");
		}

		await Expect(Page.GetByRole(AriaRole.Status)).ToHaveTextAsync("Current count: 5");
	}

    [Fact]
    public async Task Weather_HasValidColumns()
    {
        await Page.GotoAsync(webAppFixture.Endpoint.ToString());

        await Page.GetByRole(AriaRole.Link, new() { Name = "Weather" }).ClickAsync();

        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Weather" })).ToBeVisibleAsync();

        await Expect(Page.GetByText("This component demonstrates showing data loaded from a backend API service.")).ToBeVisibleAsync();

        await Expect(Page.Locator("table")).ToBeVisibleAsync();

        await Expect(Page.Locator("th", new() { HasText = "Date" })).ToBeVisibleAsync();
        await Expect(Page.Locator("th", new() { HasText = "Temp. (C)" })).ToBeVisibleAsync();
        await Expect(Page.Locator("th", new() { HasText = "Temp. (F)" })).ToBeVisibleAsync();
        await Expect(Page.Locator("th", new() { HasText = "Summary" })).ToBeVisibleAsync();
    }
}
