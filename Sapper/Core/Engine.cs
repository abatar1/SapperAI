using System;
using System.Threading;
using Sapper.Core.Primitives;

namespace Sapper.Core
{
    public class Engine
    {
        public Map Map { get; private set; }
        public GameOptions GameOptions;

        public event EventHandler<TurnEventArgs> TickEvent;

        public Engine(GameOptions options)
        {
            GameOptions = options;
        }

        public void GenerateMap()
        {
            Map = new Map(GameOptions.Width, GameOptions.Height);
            Map.Generate(GameOptions.NumberOfBombs, GameOptions.GeneratorProbability);
        }

        public enum GameStatus
        {
            Won,
            Lose,
            InProgress
        }

        public GameStatus RunGame()
        {
            var playerController = BotLoader.LoadPlayerController(GameOptions.PlayerController);          
            var player = new Player(new FieldView(Map), playerController);
            var matrix = Map.Matrix;

            while (true)
            {
                var turn = player.Tick();

                var pos = turn.Position;
                var correctMarks = 0;
                if (turn.State == Turn.States.Open)
                {
                    if (matrix[pos.X][pos.Y].IsBomb) return GameStatus.Lose;
                }
                if (turn.State == Turn.States.Mark)
                {
                    if (matrix[pos.X][pos.Y].IsBomb)
                    {
                        correctMarks += matrix[pos.X][pos.Y].IsMarked ? 1 : -1;
                    }
                    
                    if (correctMarks == GameOptions.NumberOfBombs) return GameStatus.Won;
                }

                OnTick(player.View, turn.Message);
                Thread.Sleep(GameOptions.DelayInMilliseconds);
            }                            
        }

        protected virtual void OnTick(FieldView view, string message)
        {
            var e = new TurnEventArgs(view, message);
            TickEvent?.Invoke(this, e);
        }
    }
}
