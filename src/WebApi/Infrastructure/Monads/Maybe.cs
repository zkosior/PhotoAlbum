namespace PhotoAlbum.WebApi.Infrastructure.Monads
{
    using System;

	public class Maybe<T>
	{
		public Maybe() => this.HasItem = false;

		public Maybe(T item)
			=> this.HasItem = (this.Item = item) != null;

		internal bool HasItem { get; }

		internal T Item { get; }

		public Maybe<TResult> Select<TResult>(Func<T, TResult> selector) =>
			this.HasItem
			? new Maybe<TResult>(selector(this.Item))
			: new Maybe<TResult>();

		public T GetValueOrFallback(T fallbackValue) =>
			this.HasItem ? this.Item : fallbackValue;

		public override bool Equals(object obj) =>
			obj is Maybe<T> other && Equals(this.Item, other.Item);

		public override int GetHashCode() =>
			this.HasItem ? this.Item.GetHashCode() : 0;
	}
}