namespace PhotoAlbum.Tests.ExternalHttpClients
{
	using PhotoAlbum.WebApi.Infrastructure.Failure;

	public static class EitherExtensions
	{
		public static Option<TSuccess> SuccessToOption<TSuccess, TFailure>
			(this Either<TSuccess, TFailure> either)
		{
			return either.Match(
				p => new Option<TSuccess>(p),
				p => new Option<TSuccess>(default));
		}
	}
}
