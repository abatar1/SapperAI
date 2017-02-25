using System.Drawing;
using System.Windows.Forms;

namespace Soliter.Gui
{
    public class CellCheckBox : CheckBox
    {
        public CellCheckBox()
        {
            SetStyle(ControlStyles.Selectable, false);
            Appearance = Appearance.Button;
            AutoCheck = false;
            TextAlign = ContentAlignment.MiddleCenter;
        }

        protected override bool ShowFocusCues => false;
    }
}
