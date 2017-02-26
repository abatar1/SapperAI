using System;
using System.Windows.Forms;
using Sapper.Core;
using Sapper.Gui;

namespace Sapper
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();

            var setup = new SetupGui();
            Application.Run(setup);
            Application.Run(new FieldGui(new Engine(setup.Options)));
        }
    }
}
