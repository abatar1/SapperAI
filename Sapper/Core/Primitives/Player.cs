using Sapper.Core.View;

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
            return turn;
        }

        public FieldView View { get; set; }
        private readonly IPlayerController _playerController;
    }
}
