using System.Drawing;
using System.Linq;
using Sapper.Core;
using Sapper.Core.Primitives;
using Sapper.Core.View;

namespace SapperAI
{
    public class PlayerBot : IPlayerController
    {
        public Turn MakeTurn(FieldView view)
        {
            if (view.IsFirstTurn)
            {
                var point = new Point(view.Random.Next(view.Width - 1), view.Random.Next(view.Height - 1));
                return new Turn(Turn.States.Open, point, "Hello, human!");
            }

            var viewedcells = view.GetView();
            return new Turn(Turn.States.Confused, Point.Empty, "I'm so confused :(!");
        }
    }
}
