using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public class GameSizeAndBombs // An object that stores the game size and number of bombs to create a game
    {
        private int size, numberOfBombs;

        public GameSizeAndBombs()
        {
            this.size = 0;
            this.numberOfBombs = 0;
        }

        public GameSizeAndBombs(int previousSize, int previousNumberOfBombs)
        {
            this.size = previousSize;
            this.numberOfBombs = previousNumberOfBombs;
        }
        public int Size
        {
            get { return this.size; }
            set { this.size = value; }
        }
        public int NumberOfBombs
        {
            get { return this.numberOfBombs; }
            set { this.numberOfBombs = value; }
        }

    }
}
