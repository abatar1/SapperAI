using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Soliter.Primitives;

namespace Soliter.Core.Generator
{
    public class BicompSearchEngine
    {
        private ColoredCell[][] _matrix;
        private int _width;
        private int _height;

        internal class Island
        {
            public int Count { get; }
            public int Color { get; }

            public Island(int count, int currentColor)
            {
                Count = count;
                Color = currentColor;
            }
        }

        internal class ColoredCell : Cell
        {
            public int Color { get; set; }

            public ColoredCell(int value, Point position) : base(value, position)
            {
            }
        }

        public BicompSearchEngine(Map map)
        {
            _matrix = map.Matrix.Select(w => w.Select(h => new ColoredCell(h.Value, h.Position)).ToArray()).ToArray();
            _width = map.Width;
            _height = map.Height;
        }

        private List<Point> _allowedNeighbors = new List<Point>();

        private bool IsBadCell(Point p)
        {
            var cell = _matrix[p.X][p.Y];
            return cell.Color != 0 || cell.IsOutOfBorder;
        }

        private int TryNeighbour(Point cur, Point p, int color)
        {
            if (IsBadCell(p) || cur == p) return 0;

            _allowedNeighbors.Add(p);
            _matrix[p.X][p.Y].Color = color;
            return 1;
        }

        private int FloodFill(int currentColor, Point pivot)
        {
            var lookup = new List<Point> { pivot };
            var count = 1;
            while (true)
            {
                var layerCount = 0;
                foreach (var current in lookup)
                {
                    _matrix[current.X][current.Y].Color = currentColor;

                    layerCount += TryNeighbour(current, new Point(Math.Max(current.X - 1, 0), current.Y), currentColor);
                    layerCount += TryNeighbour(current, new Point(Math.Min(current.X + 1, _width - 1), current.Y), currentColor);
                    layerCount += TryNeighbour(current, new Point(current.X, Math.Max(current.Y - 1, 0)), currentColor);
                    layerCount += TryNeighbour(current, new Point(current.X, Math.Min(current.Y + 1, _height - 1)), currentColor);
                }
                if (layerCount == 0) break;
                count += layerCount;
                lookup = new List<Point>(_allowedNeighbors);
                _allowedNeighbors.Clear();
            }
            return count;
        }

        public Cell[][] BicompSearch()
        {
            var currentColor = 1;
            var islands = new List<Island>();

            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    var p = new Point(x, y);
                    if (IsBadCell(p)) continue;

                    var count  = FloodFill(currentColor, p);
                    islands.Add(new Island(count, currentColor));
                    currentColor++;
                }
            }
                          
            var maxCount = islands.Select(i => i.Count).Max();
            var maxColor = islands.Single(i => i.Count == maxCount).Color;

            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    if (_matrix[x][y].Color != maxColor)
                        _matrix[x][y].ToOutOfBorder();
                }
            }                              
            return _matrix;
        }
    }
}
