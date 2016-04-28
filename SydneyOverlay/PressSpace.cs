using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SydneyOverlay
{
    public class PressSpace
    {
        //put here purely so that I don't have to 
        //sort out a whole bunch of conflicts with the
        //System.Windows.Forms || .Controls references
        public static void Press()
        {
            SendKeys.SendWait(" ");
        }
    }
}
