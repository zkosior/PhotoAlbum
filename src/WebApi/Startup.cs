namespace PhotoAlbum.WebApi
{
	using HealthChecks.UI.Client;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Diagnostics.HealthChecks;
	using Microsoft.AspNetCore.Mvc.ApiExplorer;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using PhotoAlbum.WebApi.Infrastructure.Extensions;
	using PhotoAlbum.WebApi.Infrastructure.Logging;
	using Serilog;

	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			this.Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services
				.AddSingleton(Log.Logger)
				.RegisterConfigurations(this.Configuration)
				.RegisterServices()
				.AddMvcWithDefaults()
				.AddHealthChecks();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(
			IApplicationBuilder app,
			IApiVersionDescriptionProvider provider)
		{
			app
				.UseCorrelationIdHandler()
				.UseHealthChecks("/healthcheck", new HealthCheckOptions
				{
					Predicate = _ => true,
					ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
					//// I could add deep check for external endpoint
				})
				.UseInternalErrorHandlers()
				.UseSwaggerWithVersioning(provider)
				.UseMvc();
		}
	}
}