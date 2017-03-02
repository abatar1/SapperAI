using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Sapper.Core.Search
{
    public class BicompValueSearch : BicompSearch
    {
        public BicompValueSearch(Map map) : base(map)
        {
        }

        protected override bool IsBadCell(Point p)
        {
            return base.IsBadCell(p) || Matrix[p.X][p.Y].Value != 0 || Matrix[p.X][p.Y].Tag != 0;
        }

        protected override int TryNeighbour(Point current, Point p, int value)
        {
            if (base.TryNeighbour(current, p, value) == 0) return 0;

            Matrix[p.X][p.Y].Tag = value;
            return 1;
        }   

        public HashSet<Point> ValueFloodFill(int value, Point pivot)
        {
            var lookup = new List<Point> { pivot };
            var result = new HashSet<Point>();
            while (true)
            {
                var layerCount = 0;
                layerCount += lookup.Sum(current => TryNeighbours(current, 1));
                if (layerCount == 0) break;
                lookup = new List<Point>(AllowedNeighbors);
                foreach (var neighbor in AllowedNeighbors)
                {
                    result.Add(neighbor);
                }

                AllowedNeighbors.Clear();               
            }

            var circuit = new HashSet<Point>();
            foreach (var neighbor in result)
            {
                for (var x = Math.Max(neighbor.X - 1, 0); x <= Math.Min(neighbor.X + 1, Width - 1); x++)
                {
                    for (var y = Math.Max(neighbor.Y - 1, 0); y <= Math.Min(neighbor.Y + 1, Height - 1); y++)
                    {
                        if (Matrix[x][y].Value != 0 && Matrix[x][y].IsValue) circuit.Add(new Point(x, y));
                    }
                }
            }
            foreach (var neighbor in circuit)
            {
                result.Add(neighbor);
            }
            return result;
        }
    }
}
