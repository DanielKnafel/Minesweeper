using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class Form1 : Form
    {
        #region Members
        private BlockGrid grid; // The logic of the game
        private int size, numberOfBombs, numberOfFlaggedBombs, numerOfRevealedCells;
        private int buffer; // A buffer from the edges of the game board in pixels
        private MyButton[,] buttonArray;
        private Size buttonSize;
        private Timer gameTimer;
        private int seconds, minutes;
        private Label timerLabel, numberOfBombsLabel;
        private bool openingFormClosed, userLostTheGame;
        private Font font;
        private Button smileyButton;
        private List<MyButton> gameTilesToDisable;
        private ComboBox newGameComboBox;
        #endregion
        public bool OpeningFormClosed // Property for openingFormClosed 
        {
            get { return this.openingFormClosed; }
        }
        public Form1() // Create a new game (used for first game)
        {
            using (OpeningForm openingForm = new OpeningForm()) // Get the size and number of bombs of the game from the opening form
            {
                if (openingForm.ShowDialog() == DialogResult.OK)
                {
                    this.size = openingForm.SelectedMode.Size;
                    this.numberOfBombs = openingForm.SelectedMode.NumberOfBombs;
                    this.openingFormClosed = false;
                }
                else
                    openingFormClosed = true;
            }

            InitializeForm();
        }
        public Form1(GameSizeAndBombs newGame) // Create a new game (used for second+ games)
        {
            this.size = newGame.Size;
            this.numberOfBombs = newGame.NumberOfBombs;

            InitializeForm();
        }
        private void InitializeForm() // Set all of the forms members
        {
            InitializeComponent();
            userLostTheGame = false;
            this.seconds = 0;
            this.minutes = 0;
            this.Icon = Properties.Resources.icon_ih7_icon;
            this.gameTilesToDisable = new List<MyButton>(size * size);
            this.font = new Font("Cambria", 12);
            this.buttonSize = new Size(30, 30);
            this.buffer = 5;
            // Initialize the game timer
            this.gameTimer = new Timer();
            this.gameTimer.Interval = 1000;
            this.gameTimer.Tick += new EventHandler(TimerTick);
            // Adjust the size of the form to the array
            this.Size = new Size(buttonSize.Width * (size + 1), buttonSize.Height * (size + 4));
            // Dont allow the user to resize the form
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            // Initialize all the labels and the smiley button
            SetNewGameComboBox();
            SetNumberOfBombsLabel();
            SetSmileyButton();
            SetTimerLabel();
            // Initialize the button grid and game logic
            InitiateNewGame();
        }
        private void RestartGame(GameSizeAndBombs previousGame) // Start a new game in a new form with the size and number of bombs specified 
        {
            Form1 newForm = new Form1(previousGame);
            newForm.Location = this.Location;
            newForm.Show();
            this.Dispose();
            return;

        }
        private void RestartGame() // The user selected to start a new game form the dropbox
        {
            this.gameTimer.Stop();
            bool openingFormClosed1;
            GameSizeAndBombs newGame = new GameSizeAndBombs();

            using (OpeningForm openingForm = new OpeningForm()) // Get the size and number of bombs of the game from the opening form
            {
                openingForm.Location = this.Location;
                if (openingForm.ShowDialog() == DialogResult.OK)
                {
                    newGame.Size = openingForm.SelectedMode.Size;
                    newGame.NumberOfBombs = openingForm.SelectedMode.NumberOfBombs;
                    openingFormClosed1 = false;
                }
                else // The use didn not submit the opening form
                {
                    openingFormClosed1 = true;
                    this.gameTimer.Start();
                }
            }

            if (openingFormClosed1)
                return;
            else
            {
                Form1 newForm = new Form1(newGame);
                newForm.Location = this.Location;
                newForm.Show();
                this.Dispose();
            }
        }
        private void SetNumberOfBombsLabel() // Sets the number of bombs label's design and location
        {
            this.numberOfBombsLabel = new Label();
            this.numberOfBombsLabel.Size = new Size(buttonSize.Width * 2, buttonSize.Height);
            this.numberOfBombsLabel.Location = new Point(size * buttonSize.Width - numberOfBombsLabel.Width + buffer, buffer);
            this.numberOfBombsLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.numberOfBombsLabel.Font = this.font;
            this.numberOfBombsLabel.BorderStyle = BorderStyle.Fixed3D;
            numberOfBombsLabel.Text = numberOfBombs.ToString();
            Controls.Add(numberOfBombsLabel);
        }
        private void SetSmileyButton()  // Sets the smiley button's design, location and function
        {
            this.smileyButton = new Button();
            this.smileyButton.Size = this.buttonSize;
            this.smileyButton.Location = new Point(this.Width / 2 - smileyButton.Width / 2 - buffer * 2, buffer);
            this.smileyButton.Image = Properties.Resources.happy_smiley;
            this.smileyButton.Click += new EventHandler(SmileyButtonClick); // Restart the game if the user clicks the smiley button
            Controls.Add(this.smileyButton);
        }
        private void SmileyButtonClick (object sender, EventArgs e) //Smiley button's function
        {
            if (this.userLostTheGame)
                RestartGame(new GameSizeAndBombs(this.size, this.numberOfBombs));
            else
            {
                this.gameTimer.Stop();
                DialogResult result = MessageBox.Show("Are you sure you want to restart?", "New Game", MessageBoxButtons.YesNo);
                switch (result)
                {
                    case DialogResult.Yes:
                        RestartGame(new GameSizeAndBombs(this.size, this.numberOfBombs));
                        break;
                    default:
                        this.gameTimer.Start();
                        break;
                }
            }
        }
        private void SetTimerLabel() // Sets the timer label's design and location
        {
            this.timerLabel = new Label();
            this.timerLabel.Size = new Size(buttonSize.Width * 2, buttonSize.Height);
            this.timerLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.timerLabel.Text = "00:00";
            this.timerLabel.Font = this.font;
            this.timerLabel.Location = new Point(this.Width / 2 - timerLabel.Width / 2 - buffer * 2, (size + 1) * buttonSize.Height + buffer * 2);
            this.timerLabel.BorderStyle = BorderStyle.Fixed3D;
            Controls.Add(timerLabel);
        }
        private void SetNewGameComboBox() // Set the location, design and function of the new game combobox
        {
            newGameComboBox = new ComboBox();
            newGameComboBox.Items.Add("New Game");
            newGameComboBox.Font = this.font;
            newGameComboBox.Size = new Size(this.buttonSize.Width * 3, this.buttonSize.Height);
            newGameComboBox.Location = new Point(buffer, buffer);
            newGameComboBox.Margin = new Padding(0);
            newGameComboBox.SelectedIndexChanged += (s, e) =>
            {
                ComboBox combobox = (ComboBox)s;
                if (combobox.SelectedIndex == 0)
                    RestartGame();
                newGameComboBox.SelectedItem = null;
            };
            Controls.Add(newGameComboBox);
        }
        private void TimerTick(object sender, EventArgs e) // Update the timer label with elapsed time every second 
        {
            seconds++;
            if (seconds == 60)
            {
                if (minutes == 99)
                    this.gameTimer.Stop();
                else
                {
                    minutes++;
                    seconds = 0;
                }
            }
            string elapsedTime = String.Format("{0:00}:{1:00}", this.minutes, this.seconds);
            timerLabel.Text = elapsedTime;
        }
        private void InitiateNewGame() // Initiates a new game
        {
            this.grid = new BlockGrid(size, numberOfBombs);
            numberOfFlaggedBombs = 0;
            numerOfRevealedCells = 0;
            InitiateButtonArray();
            this.gameTimer.Start();
        }
        private void InitiateButtonArray() // Initiates the button array 
        {
            this.buttonArray = new MyButton[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    SetButton(i, j);
                }
            }
        }
        private void SetButton(int i, int j) // Sets each button's design, location and click events
        {
            MyButton b = new MyButton(i, j);
            b.Size = buttonSize;
            b.Location = new Point(buffer + i * buttonSize.Width, buffer + smileyButton.Location.Y + smileyButton.Size.Height + j * buttonSize.Height);
            b.Font = this.font;
            b.Image = Properties.Resources.unopened;
            b.MouseDown += new MouseEventHandler(ButtonMouseDown); // Left and right click events
            b.FlatStyle = FlatStyle.Popup;
            this.buttonArray[i, j] = b;
            gameTilesToDisable.Add(b);
            Controls.Add(b);
        }
        private void ButtonMouseDown(object sender, MouseEventArgs e) // Left and right click event for buttons
        {
            MyButton b = (MyButton)sender;

            if (e.Button == MouseButtons.Right) // Right click event
                RightClick(b);
            else if (e.Button == MouseButtons.Left) // Left click event
                LeftClick(b);
            else if (e.Button == MouseButtons.Middle) // Middle Click event
                MiddleClick(b);
        }
        private void RightClick(MyButton b) // Right click event
        {
            int x = b.ButtonLocantion.X, y = b.ButtonLocantion.Y;
            if (!b.IsFalgged) // Flag a tile if it not flagged yet
            {
                if (!b.IsClicked)
                {
                    if (numberOfFlaggedBombs < numberOfBombs)
                    {
                        buttonArray[x, y].Image = Properties.Resources.Minesweeper_flag_svg;
                        b.IsFalgged = true;
                        numberOfFlaggedBombs++;
                    }
                }
            }
            else // Unflag a tile
            {
                buttonArray[x, y].Image = Properties.Resources.Minesweeper_unopened_square_svg;
                b.IsFalgged = false;
                if (numberOfFlaggedBombs > 0)
                    numberOfFlaggedBombs--;
            }
            numberOfBombsLabel.Text = (numberOfBombs - numberOfFlaggedBombs).ToString(); // Update the number of bombs left label atfter flagging
        }
        private void LeftClick(MyButton b) // Left click event
        {
            Point p = b.ButtonLocantion;
            if (!b.IsFalgged) // If the cell in not flagged
            {
                if (!b.IsClicked) // If the cell hasn't been revealed yet
                {
                    if (grid.IsBlockFromGridABomb(p))
                    {
                        b.Image = Properties.Resources.minesweeper_bomb;
                        GameLost();
                    }
                    else
                    {
                        MakeWoahFace(); // Make the smiley button woah for a few moments
                        RevealButton(b);
                        if (grid.GetNumberOfBombsAroundBlock(p) == 0)
                            RevealEmptyCellsAround(p);
                    }
                }
            }
        }
        private void MiddleClick(MyButton b) // Middle Click event
        {
            int x = b.ButtonLocantion.X, y = b.ButtonLocantion.Y;
            int flaggedBombsCounter = 0;
            List<MyButton> buttonsToReveal = new List<MyButton>(9);

            if (!b.IsFalgged)
            {
                if (this.buttonArray[x, y].IsClicked)
                {
                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            if (!(i == 0 && j == 0)) // Dont check the block itself
                            {
                                x = b.ButtonLocantion.X;
                                y = b.ButtonLocantion.Y;
                                if ((x + i >= 0) && (y + j >= 0) && (x + i < size) && (y + j < size))
                                {
                                    x += i;
                                    y += j;
                                    if (this.buttonArray[x, y].IsFalgged)
                                    {
                                        if (grid.IsBlockFromGridABomb(x, y))
                                            flaggedBombsCounter++;
                                        else
                                            GameLost();
                                    }
                                    else if (!this.buttonArray[x, y].IsClicked)
                                        buttonsToReveal.Add(this.buttonArray[x, y]);
                                }
                            }
                        }
                    }
                    if (flaggedBombsCounter == grid.GetNumberOfBombsAroundBlock(b.ButtonLocantion.X, b.ButtonLocantion.Y))
                    {
                        MakeWoahFace();
                        foreach (MyButton button in buttonsToReveal)
                        {
                            RevealButton(button);
                            if (grid.GetNumberOfBombsAroundBlock(button.ButtonLocantion) == 0)
                                RevealEmptyCellsAround(button.ButtonLocantion);
                        }
                    }
                }
            }
        }
        private void RevealButton(MyButton b) // Reveales the number of bombs around a cell
        {
            int x = b.ButtonLocantion.X;
            int y = b.ButtonLocantion.Y;
            int numberOfBombsAroundTile;

            // Reveal the number of bombs around the tile if its not 0
            numberOfBombsAroundTile = this.grid.GetNumberOfBombsAroundBlock(x, y);
            b.Image = GetNumberImage(numberOfBombsAroundTile);
            // Change the tile's appearance
            b.IsClicked = true;
            b.FlatAppearance.BorderSize = 0;
            b.FlatStyle = FlatStyle.Flat;
            gameTilesToDisable.Remove(b);
            //Check winning condition
            this.numerOfRevealedCells++;
            if (this.numerOfRevealedCells == size * size - numberOfBombs)
                GameWon();
        }
        private Image GetNumberImage(int numberOfBombsAroundTile) //Return the proper image for a tile with coresponding number image
        {
            switch (numberOfBombsAroundTile)
            {
                case 1:
                    return Properties.Resources._1;
                case 2:
                    return Properties.Resources._2;
                case 3:
                    return Properties.Resources._3;
                case 4:
                    return Properties.Resources._4;
                case 5:
                    return Properties.Resources._5;
                case 6:
                    return Properties.Resources._6;
                case 7:
                    return Properties.Resources._7;
                case 8:
                    return Properties.Resources._8;
                default:
                    return Properties.Resources._0;
            }
        }
        private void RevealEmptyCellsAround(Point location) // Reveals all the empty tiles around a given tile. used for 0 tiles
        {
            int x, y;

            foreach (Point p in grid.GetEmptyCellsAroundList(location))
            {
                x = p.X;
                y = p.Y;
                if (!this.buttonArray[x, y].IsClicked) // If the checked cell has not been revealed yet
                {
                    RevealButton(this.buttonArray[x, y]);
                    if (grid.GetNumberOfBombsAroundBlock(p) == 0)
                        RevealEmptyCellsAround(p);
                }
            }
        }
        private void GameWon() // Game won event
        {
            if (this.gameTimer.Enabled)
                this.gameTimer.Stop();
            DialogResult result = MessageBox.Show("You Win!!! \nStart A New Game?", "Congratulations", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
                RestartGame();
            else
                DisableAllButtons();
        }
        private void GameLost() // Game lost event
        {
            if (this.gameTimer.Enabled)
                this.gameTimer.Stop();
            this.smileyButton.Image = Properties.Resources.dead;
            foreach (Point p in grid.BombsLocations)
            {
                buttonArray[p.X, p.Y].Image = Properties.Resources.minesweeper_bomb;
            }
            DisableAllButtons();
            this.userLostTheGame = true; 
            DialogResult result = MessageBox.Show("It's a BOMB \nStart A New Game?", "Game Over", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
                RestartGame(new GameSizeAndBombs(this.size, this.numberOfBombs));
        }
        private void DisableAllButtons() // Disablses all active buttons in the array
        {
            foreach (MyButton b in this.gameTilesToDisable)
            {
                b.MouseDown -= new MouseEventHandler(ButtonMouseDown);
                b.FlatAppearance.BorderSize = 0; // Remove the borders around the button on mouse hover
            }
        }
        private void MakeWoahFace() // Change the smiley button image to a woah face for a few moments
        {
            this.smileyButton.Image = Properties.Resources.woah;
            Wait(150); // Hold the Woah face for 150ms 
            this.smileyButton.Image = Properties.Resources.happy_smiley;
        }
        private void Wait(int milliseconds) // Wait a few moments for the woah face to show
        {
            Timer timer1 = new Timer();
            if (milliseconds == 0 || milliseconds < 0) // The time to wait must be greater than 0
                return;
            timer1.Interval = milliseconds;
            timer1.Enabled = true;
            timer1.Start();
            timer1.Tick += (s, e) =>
            {
                timer1.Stop();
                timer1.Enabled = false;
            };
            while (timer1.Enabled) // Don't freeze the game while the animation is playing
            {
                Application.DoEvents();
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e) // Confirm user wants to close the game
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown)
                return;
            if (!this.userLostTheGame)
            {
                if (this.gameTimer.Enabled)
                    this.gameTimer.Stop();
                switch (MessageBox.Show(this, "Are you sure you want to close?", "Closing", MessageBoxButtons.YesNo))
                {
                    case DialogResult.No:
                        e.Cancel = true;
                        this.gameTimer.Start();
                        break;
                    default:
                        this.Dispose();
                        Application.Exit();
                        break;
                }
            }
            else
                Application.Exit();
        }
    }
}
