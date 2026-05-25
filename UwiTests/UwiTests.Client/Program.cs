using System;
using System.Windows.Forms;

namespace UwiTests
{
    static class Program
    {
            [STAThread]
            static void Main()
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Запуск MainForm как глав форма приложения
                Application.Run(new MainForm());
            }          
    }
}