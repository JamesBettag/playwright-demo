using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using Xunit;

namespace WebApp.Tests.WebTests;

public class WebTests(WebAppFixture webAppFixture) : PageTest, IClassFixture<WebAppFixture>, IAsyncLifetime
{
    
    private IBrowser _browser;
    private IBrowserContext _context;
    private IPage _page;

    public async Task InitializeAsync()
    {
        // Create an instance of Playwright
        var playwright = await Microsoft.Playwright.Playwright.CreateAsync();

        // Launch the browser in non-headless mode with SlowMo
        _browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false, // Set to false to visualize the browser
            SlowMo = 3000      // Add a 500ms delay between each action
        });

        _context = await _browser.NewContextAsync();
        _page = await _context.NewPageAsync();
    }

    public async Task DisposeAsync()
    {
        await _page.CloseAsync();
        await _context.CloseAsync();
        await _browser.CloseAsync();
    }
    
    [Fact]
    public async Task Home_HasTitle()
    {
        await Page.GotoAsync(webAppFixture.Endpoint.ToString());

        await Expect(Page).ToHaveTitleAsync("Home");
    }

    [Fact]
    public async Task Counter_Counts()
    {
        await Page.GotoAsync(webAppFixture.Endpoint.ToString());

        await Page.GetByRole(AriaRole.Link, new() { Name = "Counter" }).ClickAsync();

        var i = 0;
        do
        {
            // this will wait until the page has the expected value, or timeout (default 5s)
	        await Expect(Page.GetByRole(AriaRole.Status)).ToHaveTextAsync($"Current count: {i}");
			await Page.ClickAsync("text=Click me");
			
			i++;
        } while (i <= 5);

        await Expect(Page.GetByRole(AriaRole.Status)).ToHaveTextAsync($"Current count: {i}");
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
