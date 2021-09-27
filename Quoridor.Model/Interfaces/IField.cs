namespace Quoridor.Models.Interfaces
{
    public interface IField
    {
        int Height { get; }
        int Width { get; }

        IFieldNode GetCell(int w, int h);
        void Reset();
    }
}