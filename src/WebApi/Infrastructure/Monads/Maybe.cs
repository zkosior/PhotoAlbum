namespace PhotoAlbum.WebApi.Infrastructure.Monads
{
	public class Maybe<T>
	{
		public Maybe(T value) => this.Value = value;

		public bool HasValue => this.Value == default;

		public T Value { get; }
	}
}