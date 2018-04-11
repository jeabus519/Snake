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
    public partial class GameScreen : UserControl
    {
        Snake snake;
        public GameScreen()
        {
            InitializeComponent();
        }

        int scale; //value that gameScreenLength is multiplied by to fit screen
        int gameScreenLength = 20; //original resolution
        int millis = 0; //for keeping track of milliseconds
        int startLength = 5; //snakes initial length

        List<Point> field = new List<Point>(); //list that will contain all points (x, y) | 0 <= x, y <= gamesreenlength
        Point food = new Point(10, 15); //foods starting position

        SolidBrush wBrush = new SolidBrush(Color.White);
        SolidBrush bBrush = new SolidBrush(Color.Black);

        //scales original resolution up to the display size
        public void Scale()
        {
            Form f = this.FindForm();
            int width = f.Width - (f.Width % gameScreenLength);
            int height = f.Height - (f.Height % gameScreenLength);
            int scaleX = width / gameScreenLength;
            int scaleY = height / gameScreenLength;

            if (scaleX > scaleY)
            {
                scale = scaleY;
                this.Width = height;
                this.Height = height;
            }
            else
            {
                scale = scaleX;
                this.Width = width;
                this.Height = width;
            }
            this.Location = new Point((f.Width / 2) - (this.Width / 2), (f.Height / 2) - (this.Height / 2));
        }

        //generates a list of all points in the original resolution
        public void GenerateField()
        {
            for (int x = 0; x < gameScreenLength; x++)
            {
                for (int y = 0; y < gameScreenLength; y++)
                {
                    Point p = new Point(x, y);
                    field.Add(p);
                }
            }
        }

        private void GameScreen_Load(object sender, EventArgs e)
        {
            Scale();
            GenerateField();
            Point startPoint = new Point(3, 2);

            snake = new Snake("down", 75, startLength, startPoint);
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            if (millis % snake.speed == 0)
            {
                var moveReturn = snake.Move(snake, field, gameScreenLength, snake.alive, food);
                snake.alive = moveReturn.Item1;
                food = moveReturn.Item2;

                //if player lost
                if (!snake.alive)
                {
                    gameTimer.Enabled = false;
                    Form1.firstTime = false;
                    Form1.oldScore = snake.length - startLength;

                    Form f = this.FindForm();
                    f.Controls.Remove(this);
                    ModeSelect ms = new ModeSelect();
                    f.Controls.Add(ms);
                    ms.Focus();
                    return;
                }
                Refresh();
            }

            /// I initially planned to have atleast one mode with multiple snakes on screen.
            /// By using this millisecond setup (instead of just setting gameTimer.Interval equal to snake.speed),
            /// I would've been able to have them all move at different speeds, using one timer.
            /// I didn't add that mode in the end, but that's why this is here.
            millis = millis + gameTimer.Interval;
            if(millis == 1000)
            {
                millis = 0;
            }
        }

        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            foreach(Point q in field)
            {
                Point p = new Point(q.X * scale, q.Y * scale);
                if (snake.body.Contains(field.IndexOf(q)))
                {
                    e.Graphics.FillRectangle(wBrush, p.X, p.Y, scale, scale);
                }
                else if (q == food)
                {
                    e.Graphics.FillEllipse(wBrush, p.X, p.Y, scale, scale);
                }
                else
                {
                    e.Graphics.FillRectangle(bBrush, p.X, p.Y, scale, scale);
                }
            }
        }

        //player input
        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                //the if statements stop the player from turning 180 degrees; i.e. if they are moving left, they cannot turn to face right.
                case Keys.Left:
                    if (snake.body[snake.body.Count() - 1] != field.IndexOf(snake.head) - gameScreenLength)
                    {
                        snake.direction = "left";
                    }
                    break;
                case Keys.Down:
                    if (snake.body[snake.body.Count() - 1] != field.IndexOf(snake.head) + 1)
                    {
                        snake.direction = "down";
                    }
                    break;
                case Keys.Right:
                    if (snake.body[snake.body.Count() - 1] != field.IndexOf(snake.head) + gameScreenLength)
                    {
                        snake.direction = "right";
                    }
                    break;
                case Keys.Up:
                    if (snake.body[snake.body.Count() - 1] != field.IndexOf(snake.head) - 1)
                    {
                        snake.direction = "up";
                    }
                    break;
                case Keys.Escape:
                    gameTimer.Enabled = false;
                    Form1.firstTime = true;

                    Form f = this.FindForm();
                    ModeSelect ms = new ModeSelect();
                    f.Controls.Remove(this);
                    f.Controls.Add(ms);
                    ms.Focus();
                    break;
                default:
                    break;
            }
        }
    }
}
