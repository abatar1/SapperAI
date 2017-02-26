using System;
using System.Drawing;
using Sapper.Core.Primitives;

namespace Sapper.Core
{
    public class Map
    {
        private readonly Random _generator;

        public Cell[][] Matrix { get; private set; }
        public int Width { get; }
        public int Height { get; }

        public Map(int width, int height)
        {
            Width = width;
            Height = height;

            Matrix = new Cell[Width][];
            for (var x = 0; x < Width; x++)
            {       
                Matrix[x] = new Cell[Height];         
                for (var y = 0; y < Height; y++)
                {
                    Matrix[x][y] = new Cell(new Point(x, y), 0);
                }
            }

            _generator = new Random();
        }

        private void ProbChangeNeighbour(Point point, int range, double prob)
        {
            var x = point.X;
            var y = point.Y;
            if (!Matrix[x][y].IsOutOfBorder)
                Matrix[x][y].RandomChange(_generator, prob);
        }

        private void ProbCreateBorder(int range, double prob)
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    if (Matrix[x][y].IsOnBorder(Width, Height) && range == 0)
                    {
                        Matrix[x][y].RandomChange(_generator, prob);
                        continue;
                    }
                    if (!Matrix[x][y].IsOutOfBorder) continue;

                    ProbChangeNeighbour(new Point(Math.Max(x - 1, 0), y), range, prob);
                    ProbChangeNeighbour(new Point(Math.Min(x + 1, Width - 1), y), range, prob);
                    ProbChangeNeighbour(new Point(x, Math.Max(y - range, 0)), range, prob);
                    ProbChangeNeighbour(new Point(x, Math.Min(y + range, Height - 1)), range, prob);
                }
            }
        }

        private void ProbCreateBombs(int numberOfBombs)
        {
            while (numberOfBombs > 0)
            {
                var x = _generator.Next(Width);
                var y = _generator.Next(Height);

                if (Matrix[x][y].IsOutOfBorder || Matrix[x][y].IsBomb)
                    continue;

                Matrix[x][y].ToBomb();
                numberOfBombs--;
            }            
        }

        private void UpAllNeighboursValue(Point p)
        {
            for (var x = Math.Max(p.X - 1, 0); x <= Math.Min(p.X + 1, Width - 1); x++)
            {
                for (var y = Math.Max(p.Y - 1, 0); y <= Math.Min(p.Y + 1, Height - 1); y++)
                {
                    if (Matrix[x][y].IsValue) Matrix[x][y].Value += 1;
                }
            }                
        }

        private void CalculateValues()
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    if (Matrix[x][y].IsBomb) UpAllNeighboursValue(new Point(x, y));
                }
            }
        }

        public void Generate(int numberOfBombs, double prob)
        {
            if (numberOfBombs > Width * Height) throw new ArgumentException("Too many bombs!");

            var steps = (int) (Width / 2.0);
            var step = prob / steps;
            for (var i = 0; i < steps; i++)
            {
                var range = i == 0 ? 0 : 1;
                ProbCreateBorder(range, prob);
                prob -= step;
            }
            var bs = new BicompSearchEngine(this);
            Matrix = bs.BicompSearch();

            ProbCreateBombs(numberOfBombs);
            CalculateValues();
        }

        public bool IsBomb(Point point)
        {
            return Matrix[point.X][point.Y].IsBomb;
        }
    }
}
