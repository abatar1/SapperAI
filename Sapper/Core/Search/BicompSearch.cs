using System;
using System.Collections.Generic;
using System.Drawing;
using Sapper.Core.Primitives;

namespace Sapper.Core.Search
{
    public abstract class BicompSearch
    {
        protected Cell[][] Matrix;
        protected int Width { get; }
        protected int Height { get; }

        protected BicompSearch(Map map)
        {
            Width = map.Width;
            Height = map.Height;
            Matrix = map.Matrix;
        }

        protected virtual bool IsBadCell(Point p)
        {
            return Matrix[p.X][p.Y].IsOutOfBorder;
        }

        protected readonly HashSet<Point> AllowedNeighbors = new HashSet<Point>();

        protected virtual int TryNeighbour(Point current, Point p, int value)
        {
            if (IsBadCell(p) || current == p) return 0;

            AllowedNeighbors.Add(p);
            return 1;
        }

        protected int TryNeighbours(Point current, int searchingValue)
        {
            var result = 0;
            result += TryNeighbour(current, new Point(Math.Max(current.X - 1, 0), current.Y), searchingValue);
            result += TryNeighbour(current, new Point(Math.Min(current.X + 1, Width - 1), current.Y), searchingValue);
            result += TryNeighbour(current, new Point(current.X, Math.Max(current.Y - 1, 0)), searchingValue);
            result += TryNeighbour(current, new Point(current.X, Math.Min(current.Y + 1, Height - 1)), searchingValue);
            return result;
        }
    }
}
