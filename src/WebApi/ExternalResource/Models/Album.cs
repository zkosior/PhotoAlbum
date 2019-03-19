namespace PhotoAlbum.WebApi.ExternalResource.Models
{
	using System.Runtime.Serialization;

	[DataContract]
	public class Album
	{
		[DataMember(Name = "id")]
		public int Id { get; set; }

		[DataMember(Name = "userId")]
		public int UserId { get; set; }

		[DataMember(Name = "title")]
		public string Title { get; set; }
	}
}