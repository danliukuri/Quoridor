using System;

namespace Quoridor.Models.General.Either
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
            isLeft = false;
        }
        public EitherLeftOrVoid(TL left)
        {
            this.left = left;
            isLeft = true;
        }

        public T Match<T>(Func<TL, T> leftFunc)
        {
            if (leftFunc == null)
            {
                throw new ArgumentNullException(nameof(leftFunc));
            }

            return isLeft ? leftFunc(left) : default;
        }
        public TL LeftOrDefault() => Match(l => l);

        public static implicit operator EitherLeftOrVoid<TL>(TL left) => new EitherLeftOrVoid<TL>(left);
    }
}
