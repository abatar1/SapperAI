using System.Drawing;
using Sapper.Core;
using Sapper.Core.Primitives;

namespace SapperAI
{
    public class PlayerBot : IPlayerController
    {
        public Turn MakeTurn(FieldView view)
        {
            var p = new Point(view.Random.Next(view.Width - 1), view.Random.Next(view.Height - 1));
            return new Turn(Turn.States.Open, p, p.ToString());
        }
    }
}
