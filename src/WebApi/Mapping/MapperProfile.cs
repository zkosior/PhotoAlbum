namespace PhotoAlbum.WebApi.Mapping
{
	using AutoMapper;
	using PhotoAlbum.WebApi.ExternalResource.Models;
	using System;

	public class MapperProfile : Profile
	{
		public MapperProfile()
		{
			this.CreateMap<Tuple<Album, Photo>, Contracts.V1.Photo>()
				.ForMember(
					dest => dest.AlbumId,
					opt => opt.MapFrom(src => src.Item1.Id))
				.ForMember(
					dest => dest.UserId,
					opt => opt.MapFrom(src => src.Item1.UserId))
				.ForMember(
					dest => dest.AlbumTitle,
					opt => opt.MapFrom(src => src.Item1.Title))
				.ForMember(
					dest => dest.PhotoId,
					opt => opt.MapFrom(src => src.Item2.Id))
				.ForMember(
					dest => dest.PhotoTitle,
					opt => opt.MapFrom(src => src.Item2.Title))
				.ForMember(
					dest => dest.Url,
					opt => opt.MapFrom(src => src.Item2.Url))
				.ForMember(
					dest => dest.ThumbnailUrl,
					opt => opt.MapFrom(src => src.Item2.ThumbnailUrl));
		}
	}
}