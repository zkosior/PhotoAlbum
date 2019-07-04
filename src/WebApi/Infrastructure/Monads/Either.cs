namespace PhotoAlbum.WebApi.Infrastructure.Monads
{
	using System;

	public sealed class Either<TL, TR>
	{
		private readonly IEither imp;

		private Either(IEither imp) => this.imp = imp;

		private interface IEither
		{
			TResult Match<TResult>(
				Func<TL, TResult> onLeft,
				Func<TR, TResult> onRight);
		}

#pragma warning disable CA2225 // Operator overloads have named alternates
		public static implicit operator Either<TL, TR>(TL item) =>
			CreateLeft(item);

		public static implicit operator Either<TL, TR>(TR item) =>
			CreateRight(item);
#pragma warning restore CA2225 // Operator overloads have named alternates

		public TResult Match<TResult>(
			Func<TL, TResult> onLeft,
			Func<TR, TResult> onRight) => this.imp.Match(onLeft, onRight);

		public override bool Equals(object obj) =>
			obj is Either<TL, TR> other && Equals(this.imp, other.imp);

		public override int GetHashCode() => this.imp.GetHashCode();

		internal static Either<TL, TR> CreateLeft(TL value) =>
			new Either<TL, TR>(new Left(value));

		internal static Either<TL, TR> CreateRight(TR value) =>
			new Either<TL, TR>(new Right(value));

		private sealed class Left : IEither
		{
			private readonly TL left;

			public Left(TL left) => this.left = left;

			public TResult Match<TResult>(
				Func<TL, TResult> onLeft,
				Func<TR, TResult> onRight) => onLeft(this.left);

			public override bool Equals(object obj) =>
				obj is Left other && Equals(this.left, other.left);

			public override int GetHashCode() => this.left.GetHashCode();
		}

		private sealed class Right : IEither
		{
			private readonly TR right;

			public Right(TR right) => this.right = right;

			public TResult Match<TResult>(
				Func<TL, TResult> onLeft,
				Func<TR, TResult> onRight) => onRight(this.right);

			public override bool Equals(object obj) =>
				obj is Right other && Equals(this.right, other.right);

			public override int GetHashCode() => this.right.GetHashCode();
		}
	}
}