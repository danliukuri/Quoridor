using Quoridor.Models.Interfaces;
using System;

namespace Quoridor.Models
{
    public class Player : IPlayer
    {
        public string Name { get; private set; }
        public char Mark { get; private set; }
        public IFieldNode Position { get; set; }
        public int NumberOfWalls { get; set; }
  
        public Player(string name, char mark)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            }

            Name = name;
            Mark = mark;
        }
    }
}
