using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Minesweeper
{
    public enum BlockType {Normal, Bomb};

    public class Block
    {
        private BlockType typeOfBlock;
        private int numberOfBombsAroundBlock;
        private List<Point> blocksAroundThatAreEmpty;

        #region Properties
        public int NumberOfBombsAroundBlock 
        {
            get { return numberOfBombsAroundBlock; }
            set { numberOfBombsAroundBlock = value; }
        }
        public BlockType TypeOfBlock 
        {
            get { return typeOfBlock; }
            set { typeOfBlock = value; }
        }

        public List<Point> BlocksAroundThatAreEmpty
        {
            get { return this.blocksAroundThatAreEmpty; }
        }
        #endregion

        public Block(BlockType type)
        {
            this.typeOfBlock = type;
            this.numberOfBombsAroundBlock = 0;
            this.blocksAroundThatAreEmpty = new List<Point>(8);
        }
        public bool IsBomb()
        {
            if (typeOfBlock == BlockType.Bomb)
                return true;
            else
                return false;
        }
        public void AddAPointToTheList(int x, int y)
        {
            this.blocksAroundThatAreEmpty.Add(new Point (x,y));
        }
    }
}
