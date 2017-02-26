using System;
using System.Drawing;
using Sapper.Core.Primitives;

namespace Sapper.Core
{
    public class FieldView
    {       
        public Cell[][] Field { get; }
        public int Width { get; }
        public int Height { get; }

        private BicompSearchEngine _searcher;

        public FieldView(Map map)
        {
            _searcher = new BicompSearchEngine(map);
            Width = map.Width;
            Height = map.Height;
            Field = new Cell[Width][];
            for (var x = 0; x < Width; x++)
            {
                Field[x] = new Cell[Height];
                for (var y = 0; y < Height; y++)
                {
                    Field[x][y] = new Cell(new Point(x, y));
                    if (map.Matrix[x][y].IsOutOfBorder)
                    {
                        Field[x][y].ToOutOfBorder();
                    }                      
                    else
                    {
                        Field[x][y].IsFogOfWar = true;
                    }
                }
            }
        }

        public void RefreshView(Turn turn)
        {
            var pos = turn.Position;
            switch (turn.State)
            {
                case Turn.States.Mark:
                    Field[pos.X][pos.Y].IsMarked = !Field[pos.X][pos.Y].IsMarked;
                    break;

                case Turn.States.Open:
                    Field[pos.X][pos.Y].IsFogOfWar = false;

                    if (!Field[pos.X][pos.Y].IsValue) return;
                    if (Field[pos.X][pos.Y].Value != 0) return;

                    foreach (var position in _searcher.ValueFloodFill(0, pos))
                    {
                        Field[position.X][position.Y].IsFogOfWar = false;
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
