using System;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Game;

class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new GameForm());
    }
}