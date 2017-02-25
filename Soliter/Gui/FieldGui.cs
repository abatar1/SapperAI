using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Soliter.Core;
using Soliter.Properties;

namespace Soliter.Gui
{
    public class FieldGui : Form
    {
        private List<CellCheckBox> _cells = new List<CellCheckBox>();
        private Label _statusLabel;
        private Engine _engine;

        private bool isGenerated = false;

        private const int CellSize = 20;

        public FieldGui(Engine engine)
        {
            _engine = engine;
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
                        Tag = new Point(x, y),
                    };
                    _cells.Add(button);
                    Controls.Add(button);
                }
            }
            _statusLabel = new Label
            {
                Text = Resources.FieldGui_Press_space_to_generate,
                Left = 0,
                Top = height * CellSize,
                Size = new Size(CellSize * width, CellSize),
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign = ContentAlignment.MiddleLeft
            };
            Controls.Add(_statusLabel);
        }


        private async void FieldGui_SpaceKeyPress(object sender, KeyPressEventArgs e)
        {
            var tasks = new List<Task>();

            if (e.KeyChar == (char) Keys.Space)
            {
                if (isGenerated == false)
                {
                    _statusLabel.Text = Resources.FieldGui__Generating;
                    isGenerated = true;

                    _engine.GenerateMap();
                    var map = _engine.Map.Matrix;

                    tasks.AddRange(_cells.Select(cellBox => Task.Run(() =>
                    {
                        var pos = (Point) cellBox.Tag;
                        var cell = map[pos.X][pos.Y];
                        if (!cell.IsOutOfBorder) cellBox.Checked = true;
                        if (cell.IsBomb)
                        {
                            cellBox.BackgroundImage = new Bitmap(Resources.BombImage);
                            cellBox.BackgroundImageLayout = ImageLayout.Stretch;
                        }
                        else if (cell.IsValue)
                        {
                            cellBox.Text = cell.Value.ToString();
                        }
                    })));

                    await Task.WhenAll(tasks);
                    
                    _statusLabel.Text = Resources.FieldGui__Press_space_to_start_the_game;
                }
                else
                {
                    _statusLabel.Text = Resources.FieldGui__Prepearing_board;

                    tasks.Clear();
                    tasks.AddRange(_cells.Select(cellBox => Task.Run(() =>
                    {
                        cellBox.Checked = false;
                        cellBox.BackgroundImage = null;
                        cellBox.Text = "";
                    })));
                    await Task.WhenAll(tasks);

                    _statusLabel.Text = Resources.FieldGui_Game_started;
                }
            }                
        }

        private void InitializeComponent(int width, int height)
        {
            this.SuspendLayout();
            // 
            // FieldGui
            // 
            this.ClientSize = new System.Drawing.Size(width * CellSize, height * CellSize);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.KeyPress += FieldGui_SpaceKeyPress;
            this.Name = "FieldGui";
            Text = Resources.FieldGui_Solitaire;
            this.ResumeLayout(false);
        }
    }
}
