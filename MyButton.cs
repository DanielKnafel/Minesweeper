using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Minesweeper
{
    class MyButton : Button
    {
        private Point buttonLocantion; // Represents the button's location in the grid
        private bool isFalgged; // Keeps track if the cell has been flagged of not
        private bool isClicked; // Keeps track if the cell has been revealed or not

        #region Properties
        public Point ButtonLocantion
        {
            get { return this.buttonLocantion; }
        }
        public bool IsFalgged
        {
            get { return isFalgged; }
            set { this.isFalgged = value; }
        }
        public bool IsClicked
        {
            get { return isClicked; }
            set { this.isClicked = value; }
        }
        #endregion
        public MyButton(int x, int y) // Sets the button's position in the grid when the button is initialised 
        {
            this.buttonLocantion = new Point(x, y);
            this.isClicked = false;
            this.isFalgged = false;
        }
    }
}
