using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitasLbi.RedirectTool.Constant
{
    public class Constant
    {
        public enum MesasgeColor
        {
            Green,
            Red,
            Default,
            InProcess
        }
        public enum Mesasge
        {
            [Description("Successfully done!")]
            Success,

            [Description("Error")]
            Fail,

            [Description("NotStarted")]
            NotStarted,

            [Description("InProgress")]
            InProgress
        }

    }
}
