using Aspire.Hosting;
using Aspire.Hosting.Testing;
using Xunit;

namespace WebApp.Tests
{
	public class AspireFixture : IAsyncLifetime
	{
		public Uri WebEndpoint { get; private set; }
		public Uri ApiEndpoint { get; private set; }
		private DistributedApplication? _app;

		public async Task InitializeAsync()
		{
			var builder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.PlaywrightDemo_AppHost>();
			_app = await builder.BuildAsync();

			await _app.StartAsync();

			WebEndpoint = _app.GetEndpoint("webfrontend");
			ApiEndpoint = _app.GetEndpoint("apiservice");
		}

		public async Task DisposeAsync()
		{
			await (_app?.DisposeAsync() ?? ValueTask.CompletedTask);
		}
	}
}
