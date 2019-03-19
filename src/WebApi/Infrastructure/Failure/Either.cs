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

			//Task<T> Match<T>(Func<TL, Task<T>> onLeft, Func<TR, Task<T>> onRight);
		}

		public static Either<TL, TR> Success(TL value)
		{
			return CreateLeft(value);
		}

		public static Either<TL, TR> Failure(TR value)
		{
			return CreateRight(value);
		}

		//public Either<T, TR> ContinueOnSuccess<T>(Func<TL, Either<T, TR>> function)
		//{
		//	return this.Match(function, Either<T, TR>.CreateRight);
		//}

		//public Task<Either<T, TR>> ContinueOnSuccess<T>(Func<TL, Task<Either<T, TR>>> function)
		//{
		//	return this.Match(function, x => Task.FromResult(Either<T, TR>.CreateRight(x)));
		//}

		internal static Either<TL, TR> CreateLeft(TL value)
		{
			return new Either<TL, TR>(new Left(value));
		}

		internal static Either<TL, TR> CreateRight(TR value)
		{
			return new Either<TL, TR>(new Right(value));
		}

		public T Match<T>(Func<TL, T> onLeft, Func<TR, T> onRight)
		{
			return this.imp.Match(onLeft, onRight);
		}

		//public Task<T> Match<T>(Func<TL, Task<T>> onLeft, Func<TR, Task<T>> onRight)
		//{
		//	return this.imp.Match(onLeft, onRight);
		//}

		public override bool Equals(object obj)
		{
			if (!(obj is Either<TL, TR> other))
			{
				return false;
			}

			return Equals(this.imp, other.imp);
		}

		public override int GetHashCode()
		{
			return this.imp.GetHashCode();
		}

		private sealed class Left : IEither
		{
			private readonly TL left;

			public Left(TL left)
			{
				this.left = left;
			}

			public T Match<T>(Func<TL, T> onLeft, Func<TR, T> onRight)
			{
				return onLeft(this.left);
			}

			//public Task<T> Match<T>(Func<TL, Task<T>> onLeft, Func<TR, Task<T>> onRight)
			//{
			//	return onLeft(this.left);
			//}

			public override bool Equals(object obj)
			{
				if (!(obj is Left other))
				{
					return false;
				}

				return Equals(this.left, other.left);
			}

			public override int GetHashCode()
			{
				return this.left.GetHashCode();
			}
		}

		private sealed class Right : IEither
		{
			private readonly TR right;

			public Right(TR right)
			{
				this.right = right;
			}

			public T Match<T>(Func<TL, T> onLeft, Func<TR, T> onRight)
			{
				return onRight(this.right);
			}

			//public Task<T> Match<T>(Func<TL, Task<T>> onLeft, Func<TR, Task<T>> onRight)
			//{
			//	return onRight(this.right);
			//}

			public override bool Equals(object obj)
			{
				if (!(obj is Right other))
				{
					return false;
				}

				return Equals(this.right, other.right);
			}

			public override int GetHashCode()
			{
				return this.right.GetHashCode();
			}
		}
	}

	public static class EitherExtensions
	{
		public static Either<T, TR> OnSuccess<TL, TR, T>(
			this Either<TL, TR> item,
			Func<TL, Either<T, TR>> f) =>
			item.Match(f, Either<T, TR>.CreateRight);

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
			Func<TL, Task<Either<T, TR>>> f) =>
			await (await task).Match(f, x => Task.FromResult(Either<T, TR>.CreateRight(x)));
	}
}