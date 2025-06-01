using Aspire.Hosting.Testing;
using Aspire.Hosting;
using Xunit;

namespace WebApp.Tests.WebTests
{
	public class WebAppFixture : IAsyncLifetime
	{
		public Uri Endpoint { get; private set; }
		private DistributedApplication? _app;

		public async Task InitializeAsync()
		{
			var builder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.PlaywrightDemo_AppHost>();
			_app = await builder.BuildAsync();

			await _app.StartAsync();

			Endpoint = _app.GetEndpoint("webfrontend");
		}

		public async Task DisposeAsync()
		{
			await (_app?.DisposeAsync() ?? ValueTask.CompletedTask);
		}
	}
}
