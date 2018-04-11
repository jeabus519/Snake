using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Blockade
{
    class Snake
    {
        public string direction;
        public int length, speed; //Think of speed as period of snakes movement, in ms.
        public List<int> body;
        public Point head;
        public bool alive;

        public Snake(string _direction, int _speed, int _length, Point _head)
        {
            direction = _direction;
            speed = _speed; //speed is the number of milliseconds between movement. Therefore, lower = faster. Also, make it a multiple of 25 as gameTimer updates every 25ms.
            length = _length;
            head = new Point(_head.X, _head.Y);
            body = new List<int>();
            alive = true;
        }

        public Point GetFoodPoint(Snake snake, List<Point> field)
        {
            Random rnd = new Random();

            //creates a range of numbers corresponding to the field list count, excluding any numbers that are contained in Snake.
            var range = Enumerable.Range(0, field.Count).Where(i => !snake.body.Contains(i));

            //generates a random value between 0 and the number of unoccupied tiles in the field
            int index = rnd.Next(0, field.Count - snake.body.Count);

            //random value is used to pull a number from the range
            int x = range.ElementAt(index);
            //the method returns the point at the specified index location in field.
            return field[x];
        }

        public Tuple<bool, Point> Move(Snake snake, List<Point> field, int gameScreenLength, bool alive, Point food)
        {
            body.Add(field.IndexOf(head));

            //If player eats food, length+1 & choose new food point
            if (head == food)
            {
                length++;
                Point p = GetFoodPoint(snake, field);
                food.X = p.X;
                food.Y = p.Y;
            }

            #region Player loses
            if (head.X < 0 ||
                head.X > gameScreenLength - 1 ||
                head.Y < 0 ||
                head.Y > gameScreenLength - 1)
            {
                alive = false;
            }
            for (int i = 0; i < snake.body.Count() - 1; i++)
            {
                if (field.IndexOf(snake.head) == snake.body[i])
                {
                    alive = false;
                }
            }
            #endregion

            //Deletes last body chunk if snake exceeds its length
            if (snake.body.Count > snake.length)
            {
                snake.body.Remove(snake.body[0]);
            }

            //player input
            switch (direction)
            {
                case "right":
                    head.X = head.X + 1;
                    break;
                case "down":
                    head.Y = head.Y + 1;
                    break;
                case "left":
                    head.X = head.X - 1;
                    break;
                case "up":
                    head.Y = head.Y - 1;
                    break;
                default:
                    break;
            }

            return Tuple.Create(alive, food);
        }
    }
}
