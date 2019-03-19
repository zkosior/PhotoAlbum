namespace PhotoAlbum.WebApi.Controllers.V1
{
	using Microsoft.AspNetCore.Mvc;
	using PhotoAlbum.WebApi.Contracts.V1;
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

		[HttpGet("photos")]
		public async Task<ActionResult<IEnumerable<Photo>>> GetPhotos()
		{
			return await this.service.GetAllPhotos();
		}
	}
}