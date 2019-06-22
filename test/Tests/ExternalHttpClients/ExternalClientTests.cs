namespace PhotoAlbum.Tests.ExternalHttpClients
{
	using PhotoAlbum.Tests.Helpers;
	using PhotoAlbum.WebApi.ExternalResource.HttpClients;
	using PhotoAlbum.WebApi.Infrastructure.Failure;
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
			var albums = await this.sut.GetAlbums();
			albums.OnSuccess(p =>
			{
				Assert.NotEmpty(p);
				Assert.NotEqual(0, p[0].Id);
				Assert.NotNull(p[0].Title);
				return (System.Collections.Generic.List<PhotoAlbum.WebApi.ExternalResource.Models.Album>)null;
			});
			//Assert.NotEmpty(albums);
			//Assert.NotEqual(0, albums[0].Id);
			//Assert.NotNull(albums[0].Title);
		}

		[Trait("TestCategory", "Integration")]
		[Fact]
		public async Task ReturnsAlbumsByUserId()
		{
			var albums = (await this.sut.GetAlbumsByUserId(1)).SuccessToOption();
			Assert.True(albums.HasValue);
			Assert.NotEmpty(albums.Value);
			Assert.NotEqual(0, albums.Value[0].Id);
			Assert.NotNull(albums.Value[0].Title);
		}

		[Trait("TestCategory", "Integration")]
		[Fact]
		public async Task ReturnsPhotos()
		{
			var photos = await this.sut.GetPhotos();
			photos.OnSuccess(p =>
			{
				Assert.NotEmpty(p);
				Assert.NotEqual(0, p[0].Id);
				Assert.NotNull(p[0].Title);
				return (System.Collections.Generic.List<PhotoAlbum.WebApi.ExternalResource.Models.Album>)null;
			});
			//Assert.NotEmpty(photos);
			//Assert.NotEqual(0, photos[0].Id);
			//Assert.NotNull(photos[0].Title);
		}

		[Trait("TestCategory", "Integration")]
		[Fact]
		public async Task ReturnsPhotosByAlbumId()
		{
			var photos = await this.sut.GetPhotosByAlbumId(1);
			photos.OnSuccess(p =>
			{
				Assert.NotEmpty(p);
				Assert.NotEqual(0, p[0].Id);
				Assert.NotNull(p[0].Title);
				return (System.Collections.Generic.List<PhotoAlbum.WebApi.ExternalResource.Models.Album>)null;
			});
			//Assert.NotEmpty(photos);
			//Assert.NotEqual(0, photos[0].Id);
			//Assert.NotNull(photos[0].Title);
		}
	}
}