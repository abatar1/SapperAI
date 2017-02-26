namespace Sapper.Core.Primitives
{
    public class Player
    {
        public Player(FieldView view, IPlayerController playerController)
        {
            _playerController = playerController;
            View = view;
        }

        public Turn Tick()
        {           
            var turn = _playerController.MakeTurn(View);
            View.RefreshView(turn);
            return turn;
        }

        public FieldView View { get; }
        private readonly IPlayerController _playerController;
    }
}
