namespace PhotoAlbum.WebApi.ExternalResource.HttpClients
{
	using PhotoAlbum.WebApi.ExternalResource.Models;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	internal interface IExternalClient
	{
		Task<List<Photo>> GetPhotos();

		Task<List<Photo>> GetPhotosByAlbumId(int albumId);

		Task<List<Album>> GetAlbums();

		Task<List<Album>> GetAlbumsByUserId(int userId);
	}
}