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
        }

        protected override bool ShowFocusCues => false;
    }
}
