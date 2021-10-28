using System;

namespace Quoridor.Models.General.Either
{
    /// <summary>
    /// Functional data to represent a discriminated union of two possible types.
    /// </summary>
    /// <typeparam name="TL">Type of "Left" item.</typeparam>
    /// <typeparam name="TR">Type of "Right" item.</typeparam>
    public class EitherLeftOrRight<TL, TR>
    {
        public bool IsLeft { get => isLeft; }

        private readonly TL left;
        private readonly TR right;
        private readonly bool isLeft;

        public EitherLeftOrRight(TL left)
        {
            this.left = left;
            isLeft = true;
        }
        public EitherLeftOrRight(TR right)
        {
            this.right = right;
            isLeft = false;
        }
        public T Match<T>(Func<TL, T> leftFunc, Func<TR, T> rightFunc)
        {
            if (leftFunc is null)
            {
                throw new ArgumentNullException(nameof(leftFunc));
            }
            if (rightFunc is null)
            {
                throw new ArgumentNullException(nameof(rightFunc));
            }

            return isLeft ? leftFunc(left) : rightFunc(right);
        }
        public TL LeftOrDefault() => Match(l => l, r => default);
        public TR RightOrDefault() => Match(l => default, r => r);

        public static implicit operator EitherLeftOrRight<TL, TR>(TL left) => new EitherLeftOrRight<TL, TR>(left);
        public static implicit operator EitherLeftOrRight<TL, TR>(TR right) => new EitherLeftOrRight<TL, TR>(right);
    }
}
