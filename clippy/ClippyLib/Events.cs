using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClippyLib
{
    public class EditorResponseEventArgs : System.EventArgs
    {
        public string ResponseString { get; set; }

        public bool RequiresUserAction { get; set; }
    }
}
