namespace PhotoAlbum.WebApi.ExternalResource.HttpClients
{
	using PhotoAlbum.WebApi.ExternalResource.Models;
	using PhotoAlbum.WebApi.Infrastructure.Failure;
	using System.Collections.Generic;
	using System.Net;
	using System.Net.Http;
	using System.Runtime.Serialization.Json;
	using System.Threading.Tasks;

#pragma warning disable CA2234 // Pass system uri objects instead of strings
	public class ExternalClient : IExternalClient
	{
		private readonly HttpClient client;

		public ExternalClient(HttpClient client)
		{
			this.client = client;
		}

		public async Task<Either<List<Album>, ErrorResponse>> GetAlbums() =>
			new DataContractJsonSerializer(typeof(List<Album>)).ReadObject(
				await this.client.GetStreamAsync("albums")) as List<Album>;

		public async Task<Either<List<Photo>, ErrorResponse>> GetPhotos() =>
			new DataContractJsonSerializer(typeof(List<Photo>)).ReadObject(
				await this.client.GetStreamAsync("photos")) as List<Photo>;

		public async Task<Either<List<Photo>, ErrorResponse>> GetPhotosByAlbumId(int albumId) =>
			new DataContractJsonSerializer(typeof(List<Photo>)).ReadObject(
				await this.client.GetStreamAsync($"photos?albumId={albumId}")) as List<Photo>;

		public async Task<Either<List<Album>, ErrorResponse>> GetAlbumsByUserId(int userId) =>
			userId < 0
				? IncorrectUserId()
				: await this.GetAlbumsForUser(userId);

		private static Either<List<Album>, ErrorResponse> IncorrectUserId() =>
			new ErrorResponse
			{
				StatusCode = HttpStatusCode.InternalServerError,
				Message = "UserId cannot be negative.",
			};

		private async Task<Either<List<Album>, ErrorResponse>> GetAlbumsForUser(int userId) =>
			new DataContractJsonSerializer(typeof(List<Album>)).ReadObject(
				await this.client.GetStreamAsync($"albums?userId={userId}")) as List<Album>;
	}
#pragma warning restore CA2234 // Pass system uri objects instead of strings
}