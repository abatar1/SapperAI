using System;
using System.Diagnostics;
using System.Threading;
using Sapper.Core.Primitives;

namespace Sapper.Core
{
    public class Engine
    {
        public Map Map { get; }
        public GameOptions GameOptions;

        public event EventHandler<TurnEventArgs> TickEvent;

        public Engine(GameOptions options)
        {
            GameOptions = options;
            Map = new Map(GameOptions.Width, GameOptions.Height);
        }

        public enum GameStatus
        {
            Won,
            Lose,
            InProgress
        }

        private int InitRadius()
        {
            return 5;
        }

        private static int _correctMarks;

        private GameStatus GetGameStatus(Turn turn)
        {
            var matrix = Map.Matrix;
            var pos = turn.Position;
            if (turn.State == Turn.States.Open)
            {
                if (matrix[pos.X][pos.Y].IsBomb) return GameStatus.Lose;
            }
            if (turn.State == Turn.States.Mark)
            {
                if (matrix[pos.X][pos.Y].IsBomb)
                {
                    _correctMarks += matrix[pos.X][pos.Y].IsMarked ? 1 : -1;
                }

                if (_correctMarks == GameOptions.NumberOfBombs) return GameStatus.Won;
            }
            return GameStatus.InProgress;
        }

        public GameStatus RunGame()
        {
            var playerController = BotLoader.LoadPlayerController(GameOptions.PlayerController);           
            var player = new Player(new FieldView(Map), playerController);

            var isFirstTurn = true;
            _correctMarks = 0;

            while (true)
            {
                var timer = new Stopwatch();
                timer.Start();

                var turn = player.Tick();

                if (isFirstTurn)
                {
                    Map.Generate(GameOptions.NumberOfBombs, GameOptions.GeneratorProbability, turn.Position, InitRadius());
                    player.View = new FieldView(Map);                    
                    isFirstTurn = false;                   
                }
                else
                {
                    var status = GetGameStatus(turn);
                    if (status != GameStatus.InProgress)
                        return status;
                }

                player.View.RefreshView(turn);
                OnTick(player.View, turn.Message);

                timer.Stop();

                Thread.Sleep(GameOptions.DelayInMilliseconds - (int)timer.ElapsedMilliseconds);
            }                            
        }

        protected virtual void OnTick(FieldView view, string message)
        {
            var e = new TurnEventArgs(view, message);
            TickEvent?.Invoke(this, e);
        }
    }
}
