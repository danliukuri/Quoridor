namespace Quoridor.Models.General
{
    public class Neighbors<T> where T : class
    {
        const int count = 4;
        readonly T[] neighbors = new T[count];

        public T[] GetAll() => neighbors;
        public T Get(Direction direction) => neighbors[(int)direction];
        public T Set(Direction direction, T value) => neighbors[(int)direction] = value;
    }
}
