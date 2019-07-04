namespace PhotoAlbum.Tests.ExternalHttpClients
{
	using PhotoAlbum.Tests.Helpers;
	using PhotoAlbum.WebApi.ExternalResource.HttpClients;
	using PhotoAlbum.WebApi.Infrastructure.Monads;
	using System.Net.Http;
	using System.Threading.Tasks;
	using Xunit;

	public class ExternalClientTests
	{
		private readonly ExternalClient sut;

		public ExternalClientTests()
		{
			this.sut = new ExternalClient(
				new HttpClient
				{
					BaseAddress = TestEnvironment
						.GetExternalEndpointConfiguration().BaseAddress
				});
		}

		[Trait("TestCategory", "Integration")]
		[Fact]
		public async Task ReturnsAlbums()
		{
			var albums = (await this.sut.GetAlbums()).ToMaybe().Value;
			Assert.NotEmpty(albums);
			Assert.NotEqual(0, albums[0].Id);
			Assert.NotNull(albums[0].Title);
		}

		[Trait("TestCategory", "Integration")]
		[Fact]
		public async Task ReturnsAlbumsByUserId()
		{
			var albums = (await this.sut.GetAlbumsByUserId(1)).ToMaybe().Value;
			Assert.NotEmpty(albums);
			Assert.NotEqual(0, albums[0].Id);
			Assert.NotNull(albums[0].Title);
		}

		[Trait("TestCategory", "Integration")]
		[Fact]
		public async Task ReturnsPhotos()
		{
			var photos = (await this.sut.GetPhotos()).ToMaybe().Value;
			Assert.NotEmpty(photos);
			Assert.NotEqual(0, photos[0].Id);
			Assert.NotNull(photos[0].Title);
		}

		[Trait("TestCategory", "Integration")]
		[Fact]
		public async Task ReturnsPhotosByAlbumId()
		{
			var photos = (await this.sut.GetPhotosByAlbumId(1)).ToMaybe().Value;
			Assert.NotEmpty(photos);
			Assert.NotEqual(0, photos[0].Id);
			Assert.NotNull(photos[0].Title);
		}
	}
}