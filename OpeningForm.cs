using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class OpeningForm : Form
    {
        private GameSizeAndBombs selectedMode;
        private Font font;
        public OpeningForm()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.icon_ih7_icon;
            selectedMode = new GameSizeAndBombs();
            this.Size = new Size(300,250);
            this.font = new Font("Cambria", 12);
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            SetButtons();
        }
        
        private void SetButtons()
        {
            this.OKButton.AutoSize = true;

            this.gameModeLabel.Font = this.font;
            this.easyRadioButton.Font = this.font;
            this.mediumRadioButton.Font = this.font;
            this.hardRadioButton.Font = this.font;
            this.OKButton.Font = this.font;

            this.gameModeLabel.Location = new Point(this.Size.Width / 2 - gameModeLabel.Size.Width / 2, 20);
            this.easyRadioButton.Location = new Point(gameModeLabel.Location.X, gameModeLabel.Location.Y + gameModeLabel.Size.Height + 10);
            this.mediumRadioButton.Location = new Point(easyRadioButton.Location.X, easyRadioButton.Location.Y + easyRadioButton.Size.Height + 10);
            this.hardRadioButton.Location = new Point(mediumRadioButton.Location.X, mediumRadioButton.Location.Y + mediumRadioButton.Size.Height + 10);
            this.OKButton.Location = new Point(hardRadioButton.Location.X + hardRadioButton.Size.Width / 3, hardRadioButton.Location.Y + hardRadioButton.Size.Height + 10);
        }

        public GameSizeAndBombs SelectedMode
        {
            get { return this.selectedMode; }
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.selectedMode.Size = 10;
            this.selectedMode.NumberOfBombs = 15;
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.selectedMode.Size = 15;
            this.selectedMode.NumberOfBombs = 30;
        }

        private void RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.selectedMode.Size = 25;
            this.selectedMode.NumberOfBombs = 50;
        }
    }
}
