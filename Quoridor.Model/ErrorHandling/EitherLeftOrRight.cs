using System;

namespace Quoridor.ErrorHandling
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
            this.isLeft = true;
        }
        public EitherLeftOrRight(TR right)
        {
            this.right = right;
            this.isLeft = false;
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

            return this.isLeft ? leftFunc(this.left) : rightFunc(this.right);
        }
        public TL LeftOrDefault() => this.Match(l => l, r => default);
        public TR RightOrDefault() => this.Match(l => default, r => r);

        public static implicit operator EitherLeftOrRight<TL, TR>(TL left) => new EitherLeftOrRight<TL, TR>(left);
        public static implicit operator EitherLeftOrRight<TL, TR>(TR right) => new EitherLeftOrRight<TL, TR>(right);
    }
}
