namespace PhotoAlbum.Tests.Mapping
{
	using AutoMapper;
	using PhotoAlbum.WebApi.Mapping;
	using Xunit;

	[Trait("TestCategory", "Unit")]
	public class MappingTests
	{
		[Fact]
		public void ProfilesAreComplete()
		{
			CreateMapper().ConfigurationProvider.AssertConfigurationIsValid();
		}

		private static IMapper CreateMapper()
		{
			return new MapperConfiguration(
					mc => mc.AddProfile(new MapperProfile()))
				.CreateMapper();
		}
	}
}