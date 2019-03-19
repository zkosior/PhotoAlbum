namespace PhotoAlbum.WebApi.Infrastructure.Extensions
{
	using AutoMapper;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.ApiExplorer;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Options;
	using MoreLinq;
	using Newtonsoft.Json.Converters;
	using PhotoAlbum.WebApi.ExternalResource;
	using PhotoAlbum.WebApi.ExternalResource.HttpClients;
	using PhotoAlbum.WebApi.Services;
	using Swashbuckle.AspNetCore.Swagger;
	using System;
	using System.Linq;
	using System.Net.Http;

	public static class ServiceExtensions
	{
		public static IServiceCollection RegisterConfigurations(
			this IServiceCollection services,
			IConfiguration config)
		{
			return services
				.Configure<ExternalEndpoint>(
					config.GetSection("ExternalEndpoint"));
		}

		public static IServiceCollection RegisterServices(
			this IServiceCollection services)
		{
			return services
				.AddAutoMapper(typeof(ServiceExtensions))
				.AddTransient<PhotoService>()
				.AddHttpClients();
		}

		public static IServiceCollection AddMvcWithDefaults(
			this IServiceCollection services)
		{
			services
				.AddMvc(options =>
				 {
					 options.RespectBrowserAcceptHeader = true;
					 options.ReturnHttpNotAcceptable = true;
				 })
				.AddXmlSerializerFormatters()
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
				.AddJsonOptions(options =>
				{
					options.SerializerSettings.Converters.Add(
						new StringEnumConverter());
				});
			services
				.AddApiVersioning(options =>
				{
					options.ReportApiVersions = true;
				})
				.AddVersionedApiExplorer(options =>
				{
					options.GroupNameFormat = "'v'VVV";
					options.SubstituteApiVersionInUrl = true;
				})
				.AddSwaggerWithVersioning("Shopping Cart Web Api");
			return services;
		}

		private static IServiceCollection AddSwaggerWithVersioning(
			this IServiceCollection services,
			string swaggerPageTitle)
		{
			services.AddSwaggerGen(options =>
			{
				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					var descriptions = provider
						.GetRequiredService<IApiVersionDescriptionProvider>()
						.ApiVersionDescriptions;
					(from x in descriptions
					 orderby x.GroupName descending
					 select x)
					.ForEach(item =>
					{
						options.SwaggerDoc(
							item.GroupName,
							CreateInfoForApiVersion(item, swaggerPageTitle));
					});
				}
				options.CustomSchemaIds(x => x.FullName);
				options.DescribeAllEnumsAsStrings();
				options.ResolveConflictingActions(apiDescriptions =>
					(from x in apiDescriptions
					 orderby x.GroupName descending
					 select x).First());
			});
			return services;
		}

		private static Info CreateInfoForApiVersion(
			ApiVersionDescription description,
			string swaggerPageTitle)
		{
			var info = new Info
			{
				Title = swaggerPageTitle,
				Version = description.ApiVersion.ToString()
			};
			if (description.IsDeprecated)
			{
				info.Description += " This API version has been deprecated.";
			}

			return info;
		}

		private static IServiceCollection AddHttpClients(
			this IServiceCollection services)
		{
			services.AddHttpClient<IExternalClient, ExternalClient>(ConfigureHttpClient);

			return services;
		}

		private static void ConfigureHttpClient(
			IServiceProvider services,
			HttpClient client)
		{
			var httpClientOptions = services
				.GetRequiredService<IOptions<ExternalEndpoint>>()
				.Value;
			client.BaseAddress = httpClientOptions.BaseAddress;
			client.Timeout = httpClientOptions.Timeout;
		}
	}
}