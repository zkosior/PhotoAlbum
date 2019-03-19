namespace PhotoAlbum.WebApi.Controllers.V1
{
	using Microsoft.AspNetCore.Mvc;
	using PhotoAlbum.WebApi.Contracts.V1;
	using PhotoAlbum.WebApi.Infrastructure.Filters;
	using PhotoAlbum.WebApi.Services;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	[Produces("application/json")]
	[ApiVersion("1")]
	[ApiController]
	[Route("v{version:apiVersion}")]
	public class PhotosController : ControllerBase
	{
		private readonly PhotoService service;

		public PhotosController(PhotoService service)
		{
			this.service = service;
		}

		[NotFoundResultFilter]
		[HttpGet("photos")]
		public async Task<ActionResult<IEnumerable<Photo>>> GetPhotos(int? userId)
		{
			return userId.HasValue
				? await this.service.GetPhotosByUserId(userId.Value)
				: await this.service.GetAllPhotos();
		}
	}
}