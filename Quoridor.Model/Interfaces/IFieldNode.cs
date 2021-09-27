namespace Quoridor.Models.Interfaces
{
    public interface IFieldNode 
    {
        Neighbors<IFieldNode> Neighbors { get; set; } 
        Neighbors<IWall> Walls { get; set; }
    }
}
