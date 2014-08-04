using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Tetris1
{ 
    public class View
    {       
        public int screenWidth{ get; private set; }
        public int screenHeight { get; private set; }
        public int cellSize { get; private set; }

        public Color background;

        public View(int width, int height, Model model)
        {
            this.screenWidth = width;
            this.screenHeight = height;
            this.cellSize = this.screenHeight / model.cells.GetLength(0);
            this.background = Color.White;
        }

//---------------------------------------------------------------------------------

       
    }
//---------------------------------------------------------------------------------

    
}
