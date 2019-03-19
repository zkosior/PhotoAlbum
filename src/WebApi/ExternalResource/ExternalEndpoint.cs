namespace PhotoAlbum.WebApi.ExternalResource
{
	using System;

	public class ExternalEndpoint
	{
		public Uri BaseAddress { get; set; }

		public string AuthorizationToken { get; set; }

		public TimeSpan Timeout { get; set; }
	}
}