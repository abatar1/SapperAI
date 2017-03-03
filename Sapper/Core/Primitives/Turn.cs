using System.Drawing;

namespace Sapper.Core.Primitives
{
    public class Turn
    {
        public Turn(States state, Point position, string message)
        {
            State = state;
            Position = position;
            Message = message;
        }

        public Point Position { get; }

        public States State { get; }

        public string Message { get; }

        public enum States { Mark, Open, Confused }                
    }
}
