namespace PhotoAlbum.WebApi.Contracts.V1
{
	using System;

	public class Photo
	{
		public int UserId { get; set; }

		public int AlbumId { get; set; }

		public int PhotoId { get; set; }

		public string AlbumTitle { get; set; }

		public string PhotoTitle { get; set; }

		public Uri Url { get; set; }

		public Uri ThumbnailUrl { get; set; }
	}
}