namespace PhotoAlbum.Tests.Helpers
{
	using Microsoft.Extensions.Configuration;
	using PhotoAlbum.WebApi.ExternalResource;
	using System;

	internal class TestEnvironment
	{
		public static ExternalEndpoint GetExternalEndpointConfiguration()
		{
			var configuration = new ConfigurationBuilder()
				.AddUserSecrets<TestEnvironment>()
				.AddEnvironmentVariables()
				.Build();

			var externalEndpoint = new ExternalEndpoint();
			configuration.GetSection("ExternalEndpoint").Bind(externalEndpoint);

			if (string.IsNullOrWhiteSpace(externalEndpoint.BaseAddress.AbsoluteUri))
			{
				throw new ArgumentException(
					"BaseAddress string was not found." +
					" Either provide it with User Secrets Manager or Environment variable.");
			}

			return externalEndpoint;
		}
	}
}