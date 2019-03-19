namespace PhotoAlbum.WebApi.Contracts.V1
{
	public class Photo
	{
		public int UserId { get; set; }

		public int AlbumId { get; set; }

		public int PhotoId { get; set; }

		public string AlbumTitle { get; set; }

		public string PhotoTitle { get; set; }

#pragma warning disable CA1056 // Uri properties should not be strings

		public string Url { get; set; }

		public string ThumbnailUrl { get; set; }
#pragma warning restore CA1056 // Uri properties should not be strings
	}
}