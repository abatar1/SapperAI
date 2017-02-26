using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Sapper.Core;
using Sapper.Properties;

namespace Sapper.Gui
{
    public class SetupGui : Form
    {
        public GameOptions Options { get; private set; }
        private readonly List<TextBox> _textBoxes = new List<TextBox>();

        public SetupGui()
        {
            const int yOffset = 20;
            const int buttonXSize = 50;
            const int xOffset = 200;

            var options = new GameOptions();
            var properties = options.GetType().GetProperties();
            var pos = 0;

            InitializeComponent(xOffset * 2, yOffset * (properties.Length + 1));           

            foreach (var property in properties)
            {
                var displayName = ((DisplayNameAttribute)property.GetCustomAttribute(typeof(DisplayNameAttribute), true)).DisplayName;
                var label = new Label
                {
                    Text = displayName,
                    Left = 0,
                    Top = pos,
                    Size = new Size(xOffset, yOffset),
                    BorderStyle = BorderStyle.FixedSingle
                };
                Controls.Add(label);
                var textBox = new TextBox
                {
                    Left = xOffset,
                    Top = pos,
                    Size = new Size(xOffset, yOffset),
                    Tag = property.Name,
                    Text = property.GetValue(options, null).ToString()
                };
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
                Size = new Size(buttonXSize, yOffset)
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
                    GeneratorProbability = 0.3,
                    Height = 20,
                    Width = 20,
                    NumberOfBombs = 70,
                    PlayerController = "ai.dll"
                };
                Close();
            };

            Controls.Add(setupButton);
        }

        private void InitializeComponent(int width, int height)
        {
            SuspendLayout();
            // 
            // SetupGui
            // 
            ClientSize = new Size(width, height);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "SetupGui";
            Text = Resources.SetupGui_Setup;
            KeyPress += (sender, args) =>
            {
                if (args.KeyChar != (char)Keys.Enter) return;

                Close();
            };
            ResumeLayout(false);
            StartPosition = FormStartPosition.CenterScreen;
        }
    }
}
