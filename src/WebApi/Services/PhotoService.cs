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
			return
				(from album in await this.client.GetAlbums()
				 join photo in await this.client.GetPhotos()
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