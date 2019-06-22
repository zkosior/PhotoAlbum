namespace PhotoAlbum.WebApi.Controllers.V1
{
	using Contracts.V1;
	using Microsoft.AspNetCore.Mvc;
	using PhotoAlbum.WebApi.Infrastructure.Filters;
	using PhotoAlbum.WebApi.Services;
	using System.Collections.Generic;
	using System.Net;
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

		[ProducesResponseType(typeof(List<Photo>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
		[NotFoundResultFilter]
		[HttpGet("photos")]
		public async Task<ActionResult<List<Photo>>> GetPhotos(int? userId) =>
			(userId.HasValue
				? await this.service.GetPhotosByUserId(userId.Value)
				: await this.service.GetAllPhotos())
			.Match(
				p => this.Ok(p) as ObjectResult,
				q => this.NotFound(q.Message) as ObjectResult);
	}
}