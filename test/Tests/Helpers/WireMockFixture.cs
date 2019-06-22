namespace PhotoAlbum.Tests.Helpers
{
	using PhotoAlbum.WebApi.ExternalResource;
	using System;
	using WireMock.Server;
	using WireMock.Settings;

	public sealed class WireMockFixture : IDisposable
	{
		public WireMockFixture()
		{
			this.Configuration = new ExternalEndpoint
			{
				BaseAddress = new Uri("http://localhost:8443"),
				Timeout = new TimeSpan(0, 0, 2, 0),
			};
			this.Server = FluentMockServer.Start(
				new FluentMockServerSettings
				{
					Urls = new[]
					{
						this.Configuration
							.BaseAddress
							.AbsoluteUri
					}
				});
		}

		public FluentMockServer Server { get; private set; }

		public ExternalEndpoint Configuration { get; set; }

		public void Dispose()
		{
			if (this.Server != null)
			{
				this.Server.Stop();
				this.Server = null;
			}
		}
	}
}