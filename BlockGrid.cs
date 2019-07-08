using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Minesweeper
{
    public class BlockGrid
    {
        private static int size; // The length and height of the game array
        private Block[,] blockArray;
        private static int numberOfBombs; // The amount of bombs in the game
        private List<Point> bombsLocations; // Records all the locations of the bombs in the game

        #region Properties
        public List<Point> BombsLocations
        {
            get { return this.bombsLocations; }
        }
        #endregion
        public BlockGrid(int gridSize, int numberOfBombsInGame) // Constructor
        {

            size = gridSize;
            blockArray = new Block[size, size];
            numberOfBombs = numberOfBombsInGame;
            bombsLocations = new List<Point>(numberOfBombsInGame);
            InitializeGrid();
            SetNumberOfBombsForBlock();
        }
        private void InitializeGrid() // Initialize all the cells in the grid and place bombs at random cells
        {
            var rnd = new Random(); // Used for randomizing the location of the bombs
            int bombCounter = 0, a, b;

            for (int i = 0; i < size; i++) // Initialize all the cells
            {
                for (int j = 0; j < size; j++)
                {
                    this.blockArray[i, j] = new Block(BlockType.Normal);
                }
            }

            while (bombCounter < numberOfBombs) // Place the bombs in the grid
            {
                a = rnd.Next(0, size);
                b = rnd.Next(0, size);
                if (!blockArray[a, b].IsBomb())
                {
                    blockArray[a, b].TypeOfBlock = BlockType.Bomb;
                    bombsLocations.Add(new Point(a, b));
                    bombCounter++;
                }
            }
        }
        private void SetNumberOfBombsForBlock() // Counts the amount of bombs around for each block
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (!blockArray[i, j].IsBomb())
                        blockArray[i, j].NumberOfBombsAroundBlock = CountBombsAroundBlock(i, j);
                }
            }
        }
        public int GetNumberOfBombsAroundBlock(Point p) // Return the number of bombs around a block in the grid
        {
            return blockArray[p.X, p.Y].NumberOfBombsAroundBlock;
        }
        public int GetNumberOfBombsAroundBlock(int x, int y) // Return the number of bombs around a block in the grid
        {
            return blockArray[x, y].NumberOfBombsAroundBlock;
        }
        public bool IsBlockFromGridABomb(Point p) // Checks if a block in the grid is a bomb
        {
            return blockArray[p.X, p.Y].IsBomb();
        }
        public bool IsBlockFromGridABomb(int x, int y) // Checks if a block in the grid is a bomb
        {
            return blockArray[x, y].IsBomb();
        }
        private int CountBombsAroundBlock(int x, int y) // Count the amount of bombs for a cell at position [x,y] in the grid
        {
            int counter = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (!(i == 0 && j == 0)) //Dont check the block itself
                    {
                        if ((x + i >= 0) && (y + j >= 0) && (x + i < size) && (y + j < size))
                        {
                            if (blockArray[x + i, y + j].IsBomb())
                            {
                                counter++;
                            }
                            if (blockArray[x, y].NumberOfBombsAroundBlock == 0) // If the block at x,y is not surrounded by bombs, add all of its neighbors to the list of tiles to reveal if the cell is clicked
                            {
                                this.blockArray[x, y].AddAPointToTheList(x + i, y + j);
                            }

                        }
                    }
                }
            }
            return counter;
        }
        public List<Point> GetEmptyCellsAroundList(Point p) // Returns a list of all empty cells around a block in the grid
        {
            return this.blockArray[p.X, p.Y].BlocksAroundThatAreEmpty;
        }
    }
}
