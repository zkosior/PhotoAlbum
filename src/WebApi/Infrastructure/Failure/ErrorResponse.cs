namespace PhotoAlbum.WebApi.Infrastructure.Failure
{
	using System.Net;

	public class ErrorResponse
	{
		public HttpStatusCode StatusCode { get; set; }

		public string Message { get; set; }
	}
}