namespace PhotoAlbum.WebApi.ExternalResource.Models
{
	using System;
	using System.Runtime.Serialization;

	[DataContract]
	public class Photo
	{
		[DataMember(Name = "id")]
		public int Id { get; set; }

		[DataMember(Name = "albumId")]
		public int AlbumId { get; set; }

		[DataMember(Name = "title")]
		public string Title { get; set; }

		[DataMember(Name = "url")]
		public Uri Url { get; set; }

		[DataMember(Name = "thumbnailUrl")]
		public Uri ThumbnailUrl { get; set; }
	}
}