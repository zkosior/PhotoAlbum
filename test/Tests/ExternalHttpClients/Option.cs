namespace PhotoAlbum.Tests.ExternalHttpClients
{
	public class Option<T>
	{
		public Option(T value)
		{
			this.Value = value;
		}

		public bool HasValue => this.Value == default;

		public T Value { get; }
	}
}
