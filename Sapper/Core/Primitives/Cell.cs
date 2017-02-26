using System;
using System.Drawing;

namespace Sapper.Core.Primitives
{
    public class Cell
    {
        public int Value { get; set; }
        public Point Position { get; }
        public bool IsFogOfWar { get; set; }
        public bool IsMarked { get; set; }

        public bool IsOutOfBorder => Value == OutOfBorderValue;
        public bool IsBomb => Value == BombValue;
        public bool IsValue => Value < OutOfBorderValue;        

        private const int OutOfBorderValue = 9;
        private const int BombValue = 10;

        private const int DefaultValue = 100;

        public void ToOutOfBorder()
        {
            Value = OutOfBorderValue;
        }

        public void ToBomb()
        {
            Value = BombValue;
        }

        public Cell(Point position, int value = DefaultValue)
        {
            Value = value;
            Position = position;
        }

        public void RandomChange(Random generator, double prob)
        {
            if (generator.NextDouble() < prob) Value = OutOfBorderValue;
        }

        public bool IsOnBorder(int width, int height, int step = 0)
        {
            return Position.X == step 
                || Position.Y == step
                || Position.X == width - 1 - step
                || Position.Y == height - 1 - step;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
