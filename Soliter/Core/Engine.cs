using Soliter.Interface;
using Soliter.Views;

namespace Soliter.Core
{
    public class Engine
    {
        public Map Map { get; private set; }

        public GameOptions GameOptions;

        public Engine(GameOptions options)
        {
            GameOptions = options;
        }

        public void GenerateMap()
        {
            Map = new Map(GameOptions.Width, GameOptions.Height);
            Map.Generate(70, 0.2);
        }

        public void RunGame(GameOptions options)
        {
            var playerController = BotLoader.LoadPlayerController(GameOptions.PlayerController);          
            var player = new Player(new FieldView(Map), playerController, new MessageReporter());

            while (!player.IsDestroyed || !player.IsWon)
            {
                player.Tick();
            }            
        }
    }
}
