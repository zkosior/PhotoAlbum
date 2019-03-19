namespace PhotoAlbum.WebApi.Services
{
	using PhotoAlbum.WebApi.Contracts.V1;
	using PhotoAlbum.WebApi.ExternalResource.HttpClients;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	public class PhotoService
	{
		private readonly IExternalClient client;

		public PhotoService(IExternalClient client)
		{
			this.client = client;
		}

		public async Task<List<Photo>> GetAllPhotos()
		{
			return MatchPhotosToAlbums(
				await this.client.GetAlbums(),
				await this.client.GetPhotos());
		}

		public async Task<List<Photo>> GetPhotosByUserId(int userId)
		{
			var albums = await this.client.GetAlbumsByUserId(userId);
			if (albums.Count == 0)
			{
				return null; // just an example of handling nulls. it could as well return empty collection
			}

			var photos = (await Task.WhenAll(
				from a in albums
				select this.client.GetPhotosByAlbumId(a.Id)))
				.SelectMany(p => p);
			return MatchPhotosToAlbums(albums, photos);
		}

		private static List<Photo> MatchPhotosToAlbums(
			IEnumerable<ExternalResource.Models.Album> albums,
			IEnumerable<ExternalResource.Models.Photo> photos)
		{
			return (from album in albums
					join photo in photos
						on album.Id equals photo.AlbumId
					select new Photo
					{
						UserId = album.UserId,
						AlbumId = album.Id,
						PhotoId = photo.Id,
						AlbumTitle = album.Title,
						PhotoTitle = photo.Title,
						Url = photo.Url,
						ThumbnailUrl = photo.ThumbnailUrl,
					}).ToList();
		}
	}
}