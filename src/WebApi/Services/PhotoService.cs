namespace PhotoAlbum.WebApi.Services
{
	using AutoMapper;
	using PhotoAlbum.WebApi.Contracts.V1;
	using PhotoAlbum.WebApi.ExternalResource.HttpClients;
	using PhotoAlbum.WebApi.Infrastructure.Failure;
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

		public Task<Either<List<Photo>, ErrorResponse>> GetAllPhotos()
		{
			return this.client.GetAlbums().OnSuccess(
				albums => this.client.GetPhotos().OnSuccess(
					photos => Either<List<Photo>, ErrorResponse>.Success(
						this.MatchPhotosToAlbums(albums, photos))));
		}

		public Task<Either<List<Photo>, ErrorResponse>> GetPhotosByUserId(int userId)
		{
			return this.client.GetAlbumsByUserId(userId).OnSuccess(async
				albums =>
				{
					if (albums.Count == 0)
					{
						// just an example of handling nulls. it could as well return empty collection
						return Either<List<Photo>, ErrorResponse>.Failure(
							new ErrorResponse
							{
								StatusCode = System.Net.HttpStatusCode.NotFound,
								Message = "failure",
							});
					}

					var photos = AggregatePhotos(
						await Task.WhenAll(
							from a in albums
							select this.client.GetPhotosByAlbumId(a.Id)));

					return photos.OnSuccess(p =>
					Either<List<Photo>, ErrorResponse>.Success(
						this.MatchPhotosToAlbums(albums, p)));
				});
		}

		private static Either<List<ExternalResource.Models.Photo>, ErrorResponse> AggregatePhotos(
			Either<List<ExternalResource.Models.Photo>, ErrorResponse>[] photos)
		{
			return photos.Aggregate((m, n) =>
			{
				var accumulator = new List<ExternalResource.Models.Photo>();
				m.OnSuccess(o =>
				{
					accumulator.AddRange(o);
					return Either<List<ExternalResource.Models.Photo>, ErrorResponse>.Success(o);
				});
				n.OnSuccess(o =>
				{
					accumulator.AddRange(o);
					return Either<List<ExternalResource.Models.Photo>, ErrorResponse>.Success(o);
				});
				return Either<List<ExternalResource.Models.Photo>, ErrorResponse>.Success(accumulator);
			});
		}

		private List<Photo> MatchPhotosToAlbums(
			IEnumerable<ExternalResource.Models.Album> albums,
			IEnumerable<ExternalResource.Models.Photo> photos)
		{
			return
				(from album in albums
				 join photo in photos
					 on album.Id equals photo.AlbumId
				 select this.mapper.Map<Photo>(Tuple.Create(album, photo))) // not the most efficient, but shows decoupling
				.ToList();
		}
	}
}