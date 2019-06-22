namespace PhotoAlbum.WebApi.ExternalResource.HttpClients
{
	using PhotoAlbum.WebApi.ExternalResource.Models;
	using PhotoAlbum.WebApi.Infrastructure.Failure;
	using System.Collections.Generic;
	using System.Net;
	using System.Net.Http;
	using System.Runtime.Serialization.Json;
	using System.Threading.Tasks;

	public class ExternalClient : IExternalClient
	{
		private readonly HttpClient client;

		public ExternalClient(HttpClient client)
		{
			this.client = client;
		}

		public async Task<Either<List<Album>, ErrorResponse>> GetAlbums()
		{
			var url = "albums";

			var serializer = new DataContractJsonSerializer(typeof(List<Album>));
			var albums = serializer.ReadObject(
				await this.client.GetStreamAsync(url)) as List<Album>;

			return albums;
		}

		public async Task<Either<List<Album>, ErrorResponse>> GetAlbumsByUserId(int userId)
		{
			if (userId < 0)
			{
				return new ErrorResponse
				{
					StatusCode = HttpStatusCode.InternalServerError,
					Message = "UserId cannot be negative.",
				};
			}

			var url = $"albums?userId={userId}";

			var serializer = new DataContractJsonSerializer(typeof(List<Album>));
			var albums = serializer.ReadObject(
				await this.client.GetStreamAsync(url)) as List<Album>;

			return albums;
		}

		public async Task<Either<List<Photo>, ErrorResponse>> GetPhotos()
		{
			var url = "photos";

			var serializer = new DataContractJsonSerializer(typeof(List<Photo>));
			var photos = serializer.ReadObject(
				await this.client.GetStreamAsync(url)) as List<Photo>;

			return photos;
		}

		public async Task<Either<List<Photo>, ErrorResponse>> GetPhotosByAlbumId(int albumId)
		{
			var url = $"photos?albumId={albumId}";

			var serializer = new DataContractJsonSerializer(typeof(List<Photo>));
			var photos = serializer.ReadObject(
				await this.client.GetStreamAsync(url)) as List<Photo>;

			return photos;
		}
	}
}