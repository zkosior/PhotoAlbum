namespace PhotoAlbum.WebApi.Services
{
	using AutoMapper;
	using PhotoAlbum.WebApi.Contracts.V1;
	using PhotoAlbum.WebApi.ExternalResource.HttpClients;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	public class PhotoService
	{
		private readonly IExternalClient client;
		private readonly IMapper mapper;

		public PhotoService(
			IExternalClient client,
			IMapper mapper)
		{
			this.client = client;
			this.mapper = mapper;
		}

		public async Task<List<Photo>> GetAllPhotos()
		{
			return this.MatchPhotosToAlbums(
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
			return this.MatchPhotosToAlbums(albums, photos);
		}

		private List<Photo> MatchPhotosToAlbums(
			IEnumerable<ExternalResource.Models.Album> albums,
			IEnumerable<ExternalResource.Models.Photo> photos)
		{
			return (from album in albums
					join photo in photos
						on album.Id equals photo.AlbumId
					select this.mapper.Map<Photo>(Tuple.Create(album, photo))) // not the most efficient, but shows decoupling
				.ToList();
		}
	}
}