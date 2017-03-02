using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Sapper.Core.Primitives;
using Sapper.Core.Search;

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

        private void ProbChangeNeighbour(Point point, double prob)
        {
            var x = point.X;
            var y = point.Y;
            if (!Matrix[x][y].IsOutOfBorder)
                Matrix[x][y].ProbSetBorder(_generator, prob);
        }

        private bool IsInRange(Point point, Point initPoint, int radius)
        {
            return Math.Abs(point.X - initPoint.X) < radius && Math.Abs(point.Y - initPoint.Y) < radius;
        }

        private void ProbCreateBorder(int range, double prob, Point initPoint, int radius)
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    if (Matrix[x][y].IsOnBorder(Width, Height) && range == 0)
                    {
                        Matrix[x][y].ProbSetBorder(_generator, prob);
                        continue;
                    }
                    if (!Matrix[x][y].IsOutOfBorder || IsInRange(new Point(x, y), initPoint, radius)) continue;

                    ProbChangeNeighbour(new Point(Math.Max(x - 1, 0), y), prob);
                    ProbChangeNeighbour(new Point(Math.Min(x + 1, Width - 1), y), prob);
                    ProbChangeNeighbour(new Point(x, Math.Max(y - range, 0)), prob);
                    ProbChangeNeighbour(new Point(x, Math.Min(y + range, Height - 1)), prob);
                }
            }
        }

        private void ProbCreateBombs(int numberOfBombs, Point initPoint, int radius)
        {
            var freeCells = new List<Cell>();
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    if (Matrix[x][y].IsValue && !IsInRange(new Point(x, y), initPoint, radius))
                    {
                        freeCells.Add(Matrix[x][y]);
                    }                       
                }
            }
            if (numberOfBombs > freeCells.Count) throw new ArgumentException("Too many bombs!");

            while (numberOfBombs > 0)
            {
                var pos = _generator.Next(freeCells.Count);
                var point = freeCells[pos].Position;
                Matrix[point.X][point.Y].ToBomb();
                freeCells.RemoveAt(pos);
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
                    if (Matrix[x][y].IsBomb)
                        UpAllNeighboursValue(new Point(x, y));
                }
            }
        }

        public void WriteToFile(string filename)
        {
            using (var stream = new StreamWriter(filename))
            {
                for (var x = 0; x < Width; x++)
                {
                    for (var y = 0; y < Height; y++)
                    {
                        stream.Write($"{Matrix[x][y]} ");
                    }
                    stream.Write(Environment.NewLine);
                }
            }
        }

        public void Generate(int numberOfBombs, double prob, Point initPoint, int radius)
        {           
            var steps = (int) (Width / 2.0);
            var step = prob / steps;

            for (var i = 0; i < steps; i++)
            {
                var range = i == 0 ? 0 : 1;
                ProbCreateBorder(range, prob, initPoint, radius);
                prob -= step;
            }
            var bs = new BicompColorSearch(this);
            Matrix = bs.BicompSearch();

            ProbCreateBombs(numberOfBombs, initPoint, radius);
            CalculateValues();
        }
    }
}
