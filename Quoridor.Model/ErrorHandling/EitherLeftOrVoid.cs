using System;

namespace Quoridor.ErrorHandling
{
    /// <summary>
    /// Functional data to represent a discriminated union of void and one possible type.
    /// </summary>
    /// <typeparam name="TL">Type of "Left" item.</typeparam>
    public class EitherLeftOrVoid<TL>
    {
        public bool IsLeft { get => isLeft; }

        private readonly TL left;
        private readonly bool isLeft;

        public EitherLeftOrVoid()
        {
            this.isLeft = false;
        }
        public EitherLeftOrVoid(TL left)
        {
            this.left = left;
            this.isLeft = true;
        }

        public T Match<T>(Func<TL, T> leftFunc)
        {
            if (leftFunc == null)
            {
                throw new ArgumentNullException(nameof(leftFunc));
            }

            return this.isLeft ? leftFunc(this.left) : default;
        }
        public TL LeftOrDefault() => this.Match(l => l);

        public static implicit operator EitherLeftOrVoid<TL>(TL left) => new EitherLeftOrVoid<TL>(left);
    }
}
