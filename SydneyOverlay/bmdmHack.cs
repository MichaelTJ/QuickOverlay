using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace SydneyOverlay
{
    class bmdmHack
    {
        public static void PressCtrlG()
        {
            SendKeys.SendWait("^g");
        }
    }
}
