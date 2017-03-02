using System;
using System.Drawing;
using Sapper.Core.Primitives;
using Sapper.Core.Search;

namespace Sapper.Core
{
    public class FieldView
    {       
        public int Width { get; }
        public int Height { get; }
        public Random Random { get; }

        private readonly Cell[][] _matrix;
        private readonly BicompValueSearch _searcher;

        public Cell CellAt(Point point)
        {
            var cell = _matrix[point.X][point.Y];
            return cell.IsFogOfWar ? new Cell(point) : _matrix[point.X][point.Y];
        }

        public FieldView(Map map)
        {
            Random = new Random();          
            Width = map.Width;
            Height = map.Height;

            _matrix = map.Matrix;
            _searcher = new BicompValueSearch(map);
        }

        public bool IsFirstTurn { get; private set; } = true;

        public void RefreshView(Turn turn)
        {
            IsFirstTurn = false;

            var pos = turn.Position;
            switch (turn.State)
            {
                case Turn.States.Mark:
                    _matrix[pos.X][pos.Y].IsMarked = !_matrix[pos.X][pos.Y].IsMarked;
                    break;

                case Turn.States.Open:
                    _matrix[pos.X][pos.Y].IsFogOfWar = false;

                    if (_matrix[pos.X][pos.Y].Value != 0) return;

                    foreach (var position in _searcher.ValueFloodFill(0, pos))
                    {
                        _matrix[position.X][position.Y].IsFogOfWar = false;
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
