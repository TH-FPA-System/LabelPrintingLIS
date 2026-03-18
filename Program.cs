using System;
using System.Windows.Forms;

namespace ZebraPrinterGUI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // THIS MUST MATCH YOUR FORM NAME
            Application.Run(new Form1());
        }
    }
}
