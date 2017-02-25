using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Soliter.Core;
using Soliter.Properties;

namespace Soliter.Gui
{
    public class SetupGui : Form
    {
        public GameOptions Options { get; private set; }
        private readonly List<TextBox> _textBoxes = new List<TextBox>();

        public SetupGui()
        {
            const int yOffset = 20;
            const int buttonXSize = 50;
            const int xOffset = 150;

            var properties = typeof(GameOptions).GetProperties();
            InitializeComponent(xOffset * 2, yOffset * (properties.Length + 2));
            var pos = 0;

            foreach (var property in properties)
            {
                var label = new Label {Text = property.Name, Left = 0, Top = pos, Size = new Size(xOffset, yOffset) };
                Controls.Add(label);
                var textBox = new TextBox {Left = xOffset, Top = pos, Size = new Size(xOffset, yOffset), Tag = property.Name};
                _textBoxes.Add(textBox);
                Controls.Add(textBox);
                pos += yOffset;
            }

            Options = new GameOptions();
            var setupButton = new Button
            {
                Text = Resources.SetupGui_Setup,
                Left = Width / 2 - buttonXSize / 2,
                Top = pos,
                Size = new Size(buttonXSize, yOffset),
            };
            setupButton.Click += (sender, args) =>
            {          
                foreach (var text in _textBoxes)
                {
                    var prop = Options.GetType().GetProperty(text.Tag.ToString());
                    var value = Convert.ChangeType(text.Text, prop.PropertyType);
                    prop.SetValue(Options, value, null);
                }
                Close();
            };

            pos += yOffset;
            var defaultButton = new Button
            {
                Text = Resources.SetupGui_Default,
                Left = Width / 2 - buttonXSize / 2,
                Top = pos,
                Size = new Size(buttonXSize, yOffset),
            };
            defaultButton.Click += (sender, args) =>
            {
                Options = new GameOptions
                {
                    GeneratorProbability = 0.1,
                    Height = 20,
                    Width = 20,
                    NumberOfBombs = 50,
                    PlayerController = "ai.dll"
                };
                Close();
            };

            Controls.Add(setupButton);
            Controls.Add(defaultButton);
        }

        private void InitializeComponent(int width, int height)
        {
            this.SuspendLayout();
            // 
            // SetupGui
            // 
            this.ClientSize = new System.Drawing.Size(width, height);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "SetupGui";
            Text = "Setup";
            this.ResumeLayout(false);
        }
    }
}
