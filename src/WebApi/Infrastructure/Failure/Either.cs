namespace PhotoAlbum.WebApi.Infrastructure.Failure
{
	using System;
	using System.Threading.Tasks;

	public sealed class Either<TL, TR>
	{
		private readonly IEither imp;

		private Either(IEither imp)
		{
			this.imp = imp;
		}

		private interface IEither
		{
			T Match<T>(Func<TL, T> onLeft, Func<TR, T> onRight);
		}

		public static implicit operator Either<TL, TR>(TL item) => CreateLeft(item);

		public static implicit operator Either<TL, TR>(TR item) => CreateRight(item);

		//public static Either<TL, TR> Success(TL value) => CreateLeft(value);

		//public static Either<TL, TR> Failure(TR value) => CreateRight(value);

		public T Match<T>(Func<TL, T> onLeft, Func<TR, T> onRight) =>
			this.imp.Match(onLeft, onRight);

		internal static Either<TL, TR> CreateLeft(TL value) =>
			new Either<TL, TR>(new Left(value));

		internal static Either<TL, TR> CreateRight(TR value) =>
			new Either<TL, TR>(new Right(value));

		public override bool Equals(object obj) =>
			!(obj is Either<TL, TR> other) ? false : Equals(this.imp, other.imp);

		public override int GetHashCode() => this.imp.GetHashCode();

		private sealed class Left : IEither
		{
			private readonly TL left;

			public Left(TL left)
			{
				this.left = left;
			}

			public T Match<T>(Func<TL, T> onLeft, Func<TR, T> onRight) =>
				onLeft(this.left);

			public override bool Equals(object obj) =>
				!(obj is Left other) ? false : Equals(this.left, other.left);

			public override int GetHashCode() => this.left.GetHashCode();
		}

		private sealed class Right : IEither
		{
			private readonly TR right;

			public Right(TR right)
			{
				this.right = right;
			}

			public T Match<T>(Func<TL, T> onLeft, Func<TR, T> onRight) =>
				onRight(this.right);

			public override bool Equals(object obj) =>
				!(obj is Right other) ? false : Equals(this.right, other.right);

			public override int GetHashCode() => this.right.GetHashCode();
		}
	}

	public static class EitherExtensions
	{
		public static Either<T, TR> OnSuccess<TL, TR, T>(
			this Either<TL, TR> item,
			Func<TL, Either<T, TR>> f) =>
			item.Match(f, Either<T, TR>.CreateRight);

		public static Either<T, TR> OnSuccess<TL, TR, T>(
			this Either<TL, TR> item,
			Func<TL, T> f) =>
			item.Match(x => Either<T, TR>.CreateLeft(f(x)), Either<T, TR>.CreateRight);

		public static async Task<Either<T, TR>> OnSuccess<TL, TR, T>(
			this Either<TL, TR> item,
			Func<TL, Task<Either<T, TR>>> f) =>
			await item.Match(f, x => Task.FromResult(Either<T, TR>.CreateRight(x)));

		public static async Task<Either<T, TR>> OnSuccess<TL, TR, T>(
			this Task<Either<TL, TR>> task,
			Func<TL, Either<T, TR>> f) =>
			(await task).Match(f, Either<T, TR>.CreateRight);

		public static async Task<Either<T, TR>> OnSuccess<TL, TR, T>(
			this Task<Either<TL, TR>> task,
			Func<TL, T> f) =>
			(await task).Match(x => Either<T, TR>.CreateLeft(f(x)), Either<T, TR>.CreateRight);

		public static async Task<Either<T, TR>> OnSuccess<TL, TR, T>(
			this Task<Either<TL, TR>> task,
			Func<TL, Task<Either<T, TR>>> f) =>
			await (await task).Match(f, x => Task.FromResult(Either<T, TR>.CreateRight(x)));
	}
}