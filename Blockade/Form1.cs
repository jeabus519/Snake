/// created by : Michael Peterman
/// date       : April 10, 2018
/// description: Snake. You move up, down, left or right to eat food, while trying to not eat yourself or a wall. Food makes you bigger.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Blockade
{
    public partial class Form1 : Form
    {
        public static bool firstTime = true;
        public static int oldScore;

        public Form1()
        {
            InitializeComponent();

            //fullscreen
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            ModeSelect ms = new ModeSelect();
            this.Controls.Add(ms);
        }
    }
}
