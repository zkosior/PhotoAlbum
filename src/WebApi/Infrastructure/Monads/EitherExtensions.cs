namespace PhotoAlbum.WebApi.Infrastructure.Monads
{
    using System;
    using System.Threading.Tasks;

	public static class EitherExtensions
	{
		public static Either<T, TR> OnSuccess<TL, TR, T>(
			this Either<TL, TR> item,
			Func<TL, Either<T, TR>> f) => item.Match(
				f,
				Either<T, TR>.CreateRight);

		public static Either<T, TR> OnSuccess<TL, TR, T>(
			this Either<TL, TR> item,
			Func<TL, T> f) => item.Match(
				x => Either<T, TR>.CreateLeft(f(x)),
				Either<T, TR>.CreateRight);

		public static async Task<Either<T, TR>> OnSuccess<TL, TR, T>(
			this Either<TL, TR> item,
			Func<TL, Task<Either<T, TR>>> f) => await item.Match(
				f,
				x => Task.FromResult(Either<T, TR>.CreateRight(x)));

		public static async Task<Either<T, TR>> OnSuccess<TL, TR, T>(
			this Task<Either<TL, TR>> task,
			Func<TL, Either<T, TR>> f) => (await task).Match(
				f,
				Either<T, TR>.CreateRight);

		public static async Task<Either<T, TR>> OnSuccess<TL, TR, T>(
			this Task<Either<TL, TR>> task,
			Func<TL, T> f) => (await task).Match(
				x => Either<T, TR>.CreateLeft(f(x)),
				Either<T, TR>.CreateRight);

		public static async Task<Either<T, TR>> OnSuccess<TL, TR, T>(
			this Task<Either<TL, TR>> task,
			Func<TL, Task<Either<T, TR>>> f) => await (await task).Match(
				f,
				x => Task.FromResult(Either<T, TR>.CreateRight(x)));

		public static Maybe<TL> ToMaybe<TL, TR>(
			this Either<TL, TR> either) => either.Match(
			p => new Maybe<TL>(p),
			p => new Maybe<TL>(default));
	}
}