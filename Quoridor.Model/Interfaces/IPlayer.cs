namespace Quoridor.Models.Interfaces
{
    public interface IPlayer : INamed
    {
        IFieldNode Position { get; set; }

        int NumberOfWalls { get; set; }
    }
}
