using Quoridor.Models.Interfaces;

namespace Quoridor.Models
{
    public class FieldNode : IFieldNode
    {
        public Neighbors<IFieldNode> Neighbors { get; set; } = new Neighbors<IFieldNode>();
        public Neighbors<IWall> Walls { get; set; } = new Neighbors<IWall>();
    }
}
