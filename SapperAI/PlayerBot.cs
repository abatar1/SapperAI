using System.Drawing;
using Sapper.Core;
using Sapper.Core.Primitives;

namespace SapperAI
{
    public class PlayerBot : IPlayerController
    {
        public Turn MakeTurn(FieldView levelView)
        {
            return new Turn(Turn.States.Open, Point.Empty, "Hi!");
        }
    }
}
