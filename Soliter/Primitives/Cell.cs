using System;
using System.Drawing;

namespace Soliter.Primitives
{
    public class Cell
    {
        public int Value { get; set; }
        public Point Position { get; }

        public bool IsOutOfBorder => Value == OutOfBorderValue;
        public bool IsBomb => Value == BombValue;
        public bool IsValue => Value != OutOfBorderValue && Value != BombValue;

        private const string OutOfBorder = " ";
        private const int OutOfBorderValue = 10;
        private const string Bomb = "B";
        private const int BombValue = 11;

        public void ToOutOfBorder()
        {
            Value = OutOfBorderValue;
        }

        public void ToBomb()
        {
            Value = BombValue;
        }

        public Cell(int value, Point position)
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
