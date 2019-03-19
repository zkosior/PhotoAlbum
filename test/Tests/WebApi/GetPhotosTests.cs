namespace PhotoAlbum.Tests.WebApi
{
	using AutoFixture.Xunit2;
	using FluentAssertions;
	using Microsoft.AspNetCore.Mvc.Testing;
	using Newtonsoft.Json;
	using PhotoAlbum.Tests.Helpers;
	using PhotoAlbum.WebApi;
	using PhotoAlbum.WebApi.ExternalResource.Models;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Net.Http;
	using System.Threading.Tasks;
	using WireMock.RequestBuilders;
	using WireMock.ResponseBuilders;
	using Xunit;

	[Collection("WireMock collection")]
	[Trait("TestCategory", "Component")]
	public class GetPhotosTests
		: IClassFixture<WebApplicationFactory<Startup>>
	{
		private readonly HttpClient client;
		private readonly WireMockFixture wiremock;

		public GetPhotosTests(
			WebApplicationFactory<Startup> webApiFixture,
			WireMockFixture wiremockFixture)
		{
			this.client = webApiFixture.Create(
				new Dictionary<string, string>
				{
					["ExternalEndpoint:BaseAddress"] =
						wiremockFixture.Configuration.BaseAddress.AbsoluteUri,
					["ExternalEndpoint:Timeout"] =
						wiremockFixture.Configuration.Timeout.ToString(),
				},
				_ => { })
				.CreateClient();
			this.wiremock = wiremockFixture;
			this.wiremock?.Server.Reset();
		}

		[Theory]
		[AutoData]
		public async Task WhenSuccessFromExternalResource_ReturnsResults(
			PhotoAlbum.WebApi.Contracts.V1.Photo response)
		{
			this.wiremock.Server
				.Given(Request.Create().UsingGet().WithPath("/albums"))
				.RespondWith(Response.Create().WithStatusCode(200).WithBody(
					JsonConvert.SerializeObject(new List<Album>
					{
						new Album
						{
							Id = response.AlbumId,
							UserId = response.UserId,
							Title = response.AlbumTitle,
						}
					})));
			this.wiremock.Server
				.Given(Request.Create().UsingGet().WithPath("/photos"))
				.RespondWith(Response.Create().WithStatusCode(200).WithBody(
					JsonConvert.SerializeObject(new List<Photo>
					{
						new Photo
						{
							AlbumId = response.AlbumId,
							Id = response.PhotoId,
							Title = response.PhotoTitle,
							Url = response.Url,
							ThumbnailUrl = response.ThumbnailUrl,
						}
					})));

			var result = await this.client.GetAsync("/v1/photos");

			Assert.Equal(HttpStatusCode.OK, result.StatusCode);
			var photos = await result.Content
					.ReadAsAsync<List<PhotoAlbum.WebApi.Contracts.V1.Photo>>();
			photos.Should().HaveCount(1);
			photos.Single().Should().BeEquivalentTo(response);
		}

		[Fact]
		public async Task WhenExternalResultsEmpty_ReturnsEmptyCollection()
		{
			this.wiremock.Server
				.Given(Request.Create().UsingGet().WithPath("/albums"))
				.RespondWith(Response.Create().WithStatusCode(200).WithBody(
					JsonConvert.SerializeObject(new List<Album>())));
			this.wiremock.Server
				.Given(Request.Create().UsingGet().WithPath("/photos"))
				.RespondWith(Response.Create().WithStatusCode(200).WithBody(
					JsonConvert.SerializeObject(new List<Photo>())));

			var result = await this.client.GetAsync("/v1/photos");

			Assert.Equal(HttpStatusCode.OK, result.StatusCode);
			var photos = await result.Content
				.ReadAsAsync<List<PhotoAlbum.WebApi.Contracts.V1.Photo>>();
			photos.Should().HaveCount(0);
		}
	}
}