namespace PhotoAlbum.WebApi.ExternalResource.HttpClients
{
	using PhotoAlbum.WebApi.ExternalResource.Models;
	using PhotoAlbum.WebApi.Infrastructure.Failure;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public interface IExternalClient
	{
		Task<Either<List<Photo>, ErrorResponse>> GetPhotos();

		Task<Either<List<Photo>, ErrorResponse>> GetPhotosByAlbumId(int albumId);

		Task<Either<List<Album>, ErrorResponse>> GetAlbums();

		Task<Either<List<Album>, ErrorResponse>> GetAlbumsByUserId(int userId);
	}
}