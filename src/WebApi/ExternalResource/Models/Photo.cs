namespace PhotoAlbum.WebApi.ExternalResource.Models
{
	using System.Runtime.Serialization;

#pragma warning disable CA1056 // Uri properties should not be strings

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
		public string Url { get; set; }

		[DataMember(Name = "thumbnailUrl")]
		public string ThumbnailUrl { get; set; }
	}

#pragma warning restore CA1056 // Uri properties should not be strings
}