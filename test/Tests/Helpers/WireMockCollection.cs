namespace PhotoAlbum.Tests.Helpers
{
	using Xunit;

	[CollectionDefinition("WireMock collection")]
	public class WireMockCollection : ICollectionFixture<WireMockFixture>
	{
	}
}