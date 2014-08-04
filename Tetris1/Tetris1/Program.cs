using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Tetris1
{
    
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Model model1 = new Model(20, 18);
            View view1 = new View(401, 361, model1);
            Controller controller1 = new Controller();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(model1,view1));
        }
    }
}
