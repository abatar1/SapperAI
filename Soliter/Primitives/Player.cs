using Soliter.Views;

namespace Soliter.Interface
{
    public class Player
    {
        public Player(FieldView view, IPlayerController playerController, IMessageReporter messageReporter)
        {
            MessageReporter = messageReporter;
            _playerController = playerController;
            View = view;
        }

        public void Tick()
        {
            _playerController.MakeTurn(View, MessageReporter).Apply(this);
        }

        public FieldView View { get; }
        private readonly IPlayerController _playerController;
        public IMessageReporter MessageReporter { get; }

        public bool IsDestroyed { get; internal set; }
        public bool IsWon { get; internal set; }
    }
}
