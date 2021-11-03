using Quoridor.Models.General;
using Quoridor.Models.Interfaces;
using System;
using System.Linq;

namespace Quoridor.Models
{
    public class Field : IField
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        IFieldNode[,] cells;

        public Field(int width = 9, int height = 9)
        {
            if (width < 3 || width > 25)
            {
                throw new ArgumentException($"The {nameof(width)} must be between 3 and 25 ", nameof(width));
            }
            if (height < 3 || height > 25)
            {
                throw new ArgumentException($"The {nameof(height)} must be between 3 and 25 ", nameof(height));
            }
            Width = width;
            Height = height;

            CreateNodes();
            ConnectNodes();
        }

        public IFieldNode GetCell(int w, int h) => cells[w, h];

        void CreateNodes()
        {
            cells = new IFieldNode[Width, Height];
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                    cells[i, j] = new FieldNode(i, j);
        }
        void ConnectNodes()
        {
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                {
                    if (i > 0)
                        cells[i, j].Neighbors.Set(Direction.Left, cells[i - 1, j]);
                    if (j > 0)
                        cells[i, j].Neighbors.Set(Direction.Up, cells[i, j - 1]);

                    if (i < Width - 1)
                        cells[i, j].Neighbors.Set(Direction.Right, cells[i + 1, j]);
                    if (j < Height - 1)
                        cells[i, j].Neighbors.Set(Direction.Down, cells[i, j + 1]);
                }
        }

        public void Reset()
        {
            CreateNodes();
            ConnectNodes();
        }
        public void ResetPreviousPathNodes()
        {
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                    cells[i, j].PreviousPathNode = default;
        }

        public IFieldNode[] GetWalkableAdjacentNodes(IFieldNode current) =>
            current.Neighbors.GetAll().Where(adjacentNode => current.IsItPossibleMoveTo(adjacentNode)).ToArray();
    }
}