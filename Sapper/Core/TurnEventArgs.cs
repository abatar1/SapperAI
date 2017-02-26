namespace Sapper.Core
{
    public class TurnEventArgs
    {
        public TurnEventArgs( FieldView view, string message)
        {
            View = view;
            Message = message;
        }

        public FieldView View { get; }
        public string Message { get; }
    }
}
