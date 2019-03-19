namespace PhotoAlbum.Tests.Services
{
	using AutoFixture.Xunit2;
	using FluentAssertions;
	using NSubstitute;
	using PhotoAlbum.WebApi.ExternalResource.HttpClients;
	using PhotoAlbum.WebApi.ExternalResource.Models;
	using PhotoAlbum.WebApi.Services;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Xunit;

	[Trait("TestCategory", "Unit")]
	public class GetAllPhotosTests
	{
		private readonly IExternalClient client =
			Substitute.For<IExternalClient>();

		[Theory]
		[AutoData]
		public async Task WhenExternalResultsMatch_ReturnsPhotos(
			PhotoAlbum.WebApi.Contracts.V1.Photo response)
		{
			this.client.GetAlbums().Returns(new List<Album>
			{
				new Album
				{
					Id = response.AlbumId,
					UserId = response.UserId,
					Title = response.AlbumTitle,
				}
			});
			this.client.GetPhotos().Returns(new List<Photo>
			{
				new Photo
				{
					AlbumId = response.AlbumId,
					Id = response.PhotoId,
					Title = response.PhotoTitle,
					Url = response.Url,
					ThumbnailUrl = response.ThumbnailUrl,
				}
			});

			var result = await new PhotoService(this.client).GetAllPhotos();
			result.Should().HaveCount(1);
			result.Single().Should().BeEquivalentTo(response);
		}

		[Theory]
		[AutoData]
		public async Task WhenExternalResultsDontMatch_ReturnsEmptyCollection(
			Album album,
			Photo photo)
		{
			album.Id = 1;
			photo.AlbumId = 2;
			this.client.GetAlbums().Returns(new List<Album> { album });
			this.client.GetPhotos().Returns(new List<Photo> { photo });

			var result = await new PhotoService(this.client).GetAllPhotos();
			result.Should().BeEmpty();
		}

		[Fact]
		public async Task WhenExternalResultsEmpty_ReturnsEmptyCollection()
		{
			this.client.GetAlbums().Returns(new List<Album>());
			this.client.GetPhotos().Returns(new List<Photo>());

			var result = await new PhotoService(this.client).GetAllPhotos();
			result.Should().BeEmpty();
		}
	}
}