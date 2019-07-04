namespace PhotoAlbum.WebApi.Services
{
	using AutoMapper;
	using PhotoAlbum.WebApi.Contracts.V1;
	using PhotoAlbum.WebApi.ExternalResource.HttpClients;
	using PhotoAlbum.WebApi.Infrastructure.Failure;
	using PhotoAlbum.WebApi.Infrastructure.Monads;
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

		public Task<Either<List<Photo>, ErrorResponse>> GetAllPhotos() =>
			this.client.GetAlbums().OnSuccess(
				albums => this.client.GetPhotos().OnSuccess(
					photos => this.MatchPhotosToAlbums(albums, photos)));

		public Task<Either<List<Photo>, ErrorResponse>> GetPhotosByUserId(int userId) =>
			this.client.GetAlbumsByUserId(userId).OnSuccess(this.GetPhotosFromAlbum);

		private static Either<List<Photo>, ErrorResponse> UserNotFound() =>
			//// just an example of handling nulls. it could as well return empty collection
			new ErrorResponse
			{
				StatusCode = System.Net.HttpStatusCode.NotFound,
				Message = "failure",
			};

		private static Either<List<ExternalResource.Models.Photo>, ErrorResponse> AggregatePhotos(
			Either<List<ExternalResource.Models.Photo>, ErrorResponse>[] photos)
		{
			Either<List<ExternalResource.Models.Photo>, ErrorResponse> tmp = null;
			foreach (var photo in photos)
			{
				if (tmp == null)
				{
					tmp = photo;
					continue;
				}

				tmp = tmp.OnSuccess(p => photo.OnSuccess(q =>
				{
					p.AddRange(q);
					return p;
				}));
			}

			return tmp;
		}

		private async Task<Either<List<Photo>, ErrorResponse>> GetPhotosFromAlbum(
			List<ExternalResource.Models.Album> albums) =>
				albums.Count == 0
					? UserNotFound()
					: AggregatePhotos(
					await Task.WhenAll(
						from a in albums
						select this.client.GetPhotosByAlbumId(a.Id)))
					.OnSuccess(p => this.MatchPhotosToAlbums(albums, p));

		private List<Photo> MatchPhotosToAlbums(
			IEnumerable<ExternalResource.Models.Album> albums,
			IEnumerable<ExternalResource.Models.Photo> photos) =>
				(from album in albums
				 join photo in photos
					 on album.Id equals photo.AlbumId
				 select this.mapper.Map<Photo>(Tuple.Create(album, photo))) // not the most efficient, but shows decoupling
				.ToList();
	}
}