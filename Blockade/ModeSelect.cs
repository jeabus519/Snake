using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Blockade
{
    public partial class ModeSelect : UserControl
    {
        public ModeSelect()
        {
            InitializeComponent();
        }

        int buttons;

        public Button genButton(string butText)
        {
            buttons++;
            Button b = new Button();
            this.Controls.Add(b);
            b.Text = butText;
            b.ForeColor = Color.White;
            b.BackColor = Color.Black;
            b.Font = new Font("Comic Sans MS", this.Height / 50, FontStyle.Bold);
            b.Size = new Size(this.Width / 3, this.Width / 10);
            b.Location = new Point((this.Width - b.Width) / 2, ((this.Height - b.Height) / 2) + buttons * b.Height);

            return b;
        }

        private void ModeSelect_Load(object sender, EventArgs e)
        {
            buttons = 0;
            #region Scale
            Form f = this.FindForm();
            if (f.Height < f.Width)
            {
                this.Height = f.Height;
                this.Width = f.Height;
            }
            else
            {
                this.Height = f.Width;
                this.Width = f.Width;
            }
            this.Location = new Point((f.Width / 2) - (this.Width / 2), (f.Height / 2) - (this.Height / 2));
            #endregion

            #region Labels
            /// if game just booted, or previous round was quit using esc: display title, playbutton text = play
            /// if round has just finished, display game over + score, playbutton text = play again
            Button snakeButton;
            if (Form1.firstTime)
            {
                snakeButton = genButton("Play");

                Label l = new Label();
                this.Controls.Add(l);
                l.Location = new Point(10, 10);
                l.Width = this.Width;
                l.Height = this.Height/2;
                l.Font = new Font("Comic Sans MS", this.Height / 10, FontStyle.Bold);
                l.ForeColor = Color.White;
                l.Text = "Snake";
                l.TextAlign = ContentAlignment.MiddleCenter;
            }
            else
            {
                snakeButton = genButton("Play again");

                Label l = new Label();
                this.Controls.Add(l);
                l.Location = new Point(0,this.Height/4);
                l.Width = this.Width;
                l.Height = this.Height/4;
                l.Font = new Font("Comic Sans MS", this.Height / 50, FontStyle.Bold);
                l.ForeColor = Color.White;
                l.Text = "Game over! \nYour Score: " + Form1.oldScore;
                l.TextAlign = ContentAlignment.MiddleCenter;
            }
#endregion

            snakeButton.Click += new EventHandler(this.snakeButton_Click);
            Button quitButton = genButton("Quit");
            quitButton.Click += new EventHandler(this.quitButton_Click);
            quitButton.BringToFront();
        }

        public void snakeButton_Click(object sender, System.EventArgs e)
        {
            Form f = this.FindForm();
            GameScreen gs = new GameScreen();
            f.Controls.Remove(this);
            f.Controls.Add(gs);
            gs.Focus();
        }
        public void quitButton_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }
    }
}
