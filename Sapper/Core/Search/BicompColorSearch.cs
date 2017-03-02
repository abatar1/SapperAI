using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Sapper.Core.Primitives;

namespace Sapper.Core.Search
{
    public class BicompColorSearch : BicompSearch
    {
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

        public BicompColorSearch(Map map) : base(map)
        {
        }

        protected override bool IsBadCell(Point p)
        {
            return base.IsBadCell(p) || Matrix[p.X][p.Y].Tag != 0;
        }

        protected override int TryNeighbour(Point current, Point p, int value)
        {
            if (base.TryNeighbour(current, p, value) == 0) return 0;

            Matrix[p.X][p.Y].Tag = value;
            return 1;
        }        

        private int ColorFloodFill(int currentColor, Point pivot)
        {
            var lookup = new List<Point> {pivot};
            var count = 1;
            while (true)
            {
                var layerCount = 0;
                foreach (var current in lookup)
                {
                    Matrix[current.X][current.Y].Tag = currentColor;
                    layerCount += TryNeighbours(current, currentColor);
                }
                if (layerCount == 0) break;
                count += layerCount;
                lookup = new List<Point>(AllowedNeighbors);
                AllowedNeighbors.Clear();
            }
            return count;
        }        

        public Cell[][] BicompSearch()
        {
            var currentColor = 1;
            var islands = new List<Island>();

            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var p = new Point(x, y);
                    if (IsBadCell(p)) continue;

                    var count  = ColorFloodFill(currentColor, p);
                    islands.Add(new Island(count, currentColor));
                    currentColor++;
                }
            }
                          
            var maxCount = islands.Select(i => i.Count).Max();
            var maxColor = islands.Single(i => i.Count == maxCount).Color;

            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    if (Matrix[x][y].Tag != maxColor)
                    {
                        Matrix[x][y].ToOutOfBorder();                       
                    }
                    Matrix[x][y].Tag = 0;
                }
            }

            return Matrix;
        }
    }
}
