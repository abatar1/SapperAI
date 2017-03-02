using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Sapper.Core;
using Sapper.Properties;

namespace Sapper.Gui
{
    public class FieldGui : Form
    {
        private readonly List<CellCheckBox> _cells = new List<CellCheckBox>();
        private readonly Label _statusLabel;
        private readonly Engine _engine;
        private readonly BackgroundWorker _gameWorker;

        private bool _isStarted;

        private const int CellSize = 20;

        public FieldGui(Engine engine)
        {
            #region Engine Init
            _engine = engine;
            _engine.TickEvent += (sender, args) =>
            {
                _gameWorker.ReportProgress(0, args);
            };
            #endregion

            #region Worker Init

            var status = Engine.GameStatus.InProgress;
            _gameWorker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true,
                WorkerReportsProgress = true
            };
            _gameWorker.DoWork += (sender, args) =>
            {
                status = _engine.RunGame();
            };
            _gameWorker.RunWorkerCompleted += (sender, args) =>
            {
                switch (status)
                {
                    case Engine.GameStatus.Lose:
                        _statusLabel.Text = Resources.FieldGui_You_lose_Press_space_and_try_again;
                        break;
                    case Engine.GameStatus.Won:
                        _statusLabel.Text = Resources.FieldGui_You_won_Press_space_and_try_again;
                        break;
                    case Engine.GameStatus.InProgress:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                _isStarted = false;
            };
            _gameWorker.ProgressChanged += GameWorker_ProgressChanged;
            #endregion          

            var width = engine.GameOptions.Width;
            var height = engine.GameOptions.Height;
            InitializeComponent(width, height + 1);

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var button = new CellCheckBox
                    {
                        Left = x * CellSize,
                        Top = y * CellSize,
                        Size = new Size(CellSize, CellSize),
                        Tag = new Point(x, y)
                    };
                    _cells.Add(button);
                    Controls.Add(button);
                }
            }

            _statusLabel = new Label
            {
                Text = Resources.FieldGui_Press_space_to_start_the_game,
                Left = 0,
                Top = height * CellSize,
                Size = new Size(CellSize * width, CellSize),
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign = ContentAlignment.MiddleLeft
            };
            Controls.Add(_statusLabel);
        }

        private void GameWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var turn = (TurnEventArgs) e.UserState;
            _statusLabel.Text = turn.Message;

            foreach (var cellBox in _cells)
            {
                var cell = turn.View.CellAt((Point)cellBox.Tag);

                if (cell.IsOutOfBorder)
                {
                    cellBox.Visible = false;
                    continue;
                }
                if (cell.IsMarked)
                {
                    cellBox.BackgroundImage = new Bitmap(Resources.FlagImage);
                    cellBox.BackgroundImageLayout = ImageLayout.Stretch; ;
                }
                if (cell.IsFogOfWar) continue;

                cellBox.Checked = true;

                if (cell.IsBomb)
                {
                    cellBox.BackgroundImage = new Bitmap(Resources.BombImage);
                    cellBox.BackgroundImageLayout = ImageLayout.Stretch;                   
                }
                if (cell.IsValue)
                {
                    if (cell.Value != 0) cellBox.Text = cell.Value.ToString();
                    cellBox.ForeColor = cell.Value < 5 ? Color.Green : Color.Red;
                }
            }
        }

        private void InitializeComponent(int width, int height)
        {
            SuspendLayout();
            // 
            // FieldGui
            //             
            ClientSize = new Size(width * CellSize, height * CellSize);
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPress += (sender, args) =>
            {
                if (args.KeyChar != (char)Keys.Space) return;
                if (_isStarted) return;

                _isStarted = true;
                _gameWorker.RunWorkerAsync();
            };
            Closing += (sender, args) =>
            {
                _gameWorker.CancelAsync();
            };
            Name = "FieldGui";
            Text = Resources.FieldGui_Sapper;
            ResumeLayout(false);
            StartPosition = FormStartPosition.CenterScreen;
        }
    }
}