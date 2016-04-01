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
        public class Utility
        {
            public static string WorkingOnReport = "We are working on report...Please wait !";
            public static string GeneratingReport = " Validation done.Now, generating report.";
            public static string ReportGenerationDone = "Report created and can be downloaded from location :";
            public static string ConfigKeptAtLoc = "Rewrite output file kept at location:";
        }

        public class Template
        {
            public static string ValidationTemplate = "Verification started for Rule{0}\n";
            public static string ValidationDoneTemplate = "Verification done for Rule{0}: result :{1}\n";
        }




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
