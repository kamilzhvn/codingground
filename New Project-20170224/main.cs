using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;

namespace Snake
{
     class Food
    {
        public Point location;
        public char sign = '♠';
        public Food()
        {
            location = new Point(new Random().Next() % 30, new Random().Next() % 30);
        }
        public void Draw()
        {
            Console.SetCursorPosition(location.x, location.y);
            Console.Write(sign);
        }
    }
    
    class Point
    {
        public int x;
        public int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            Point o = obj as Point;
            if (this.x == o.x && this.y == o.y)
                return true;
            return false;
        }
        
        public override int GetHashCode()
        {
       

            return base.GetHashCode();

        }
    }
    
    class Worm
    {
        public char sign = '*';
        List<Point> body = new List<Point>();
        public bool isAlive = true;
        public Worm()
        {
            body.Add(new Point(10, 10));
        }
        public void Draw()
        {
            for (int i = 0; i < body.Count; ++i)
            {
                Console.SetCursorPosition(body[i].x,body[i].y);
                Console.Write(sign);
            }
        }

        public void Move(int dx, int dy)
        {
            if (body[0].x + dx < 0) return;
            if (body[0].y + dy < 0) return;
            if (body[0].x + dx > 40) return;
            if (body[0].y + dy > 40) return;

            for (int i = body.Count - 1; i > 0; --i)
            {
                body[i].x = body[i - 1].x;
                body[i].y = body[i - 1].y;
            }

            body[0].x = body[0].x + dx;
            body[0].y = body[0].y + dy;

        }

        public bool CanEat(Food food)
        {
            if (body[0].Equals(food.location))
            {
                body.Add(food.location);
                return true;
            }
            return false;
        }
    }
    
    class Wall
    {
        public char sign = '#';
        public List<Point> bricks = new List<Point>();
        public Wall(int level)
        {
            string fname = string.Format(@"Levels\level{0}.txt",level);
            using(FileStream fs = new FileStream(fname,FileMode.OpenOrCreate,FileAccess.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    int colNumber = 0;
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        for (int rowNumber = 0; rowNumber < line.Length; ++rowNumber)
                        {
                            if (line[rowNumber] == '#')
                            {
                                bricks.Add(new Point(rowNumber, colNumber));
                            }
                        }

                        colNumber++;
                    }
                }
            }
        }

        public void Draw()
        {
            for (int i = 0; i < bricks.Count; ++i)
            {
                Console.SetCursorPosition(bricks[i].x, bricks[i].y);
                Console.Write(sign);
            }
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Worm worm = new Worm();
            Food food = new Food();
            Wall wall = new Wall(1);
            
            while (worm.isAlive)
            {
                Console.Clear();
                worm.Draw();
                food.Draw();
                wall.Draw();
                ConsoleKeyInfo pressedKey = Console.ReadKey();
                switch (pressedKey.Key)
                {
                    case ConsoleKey.UpArrow:
                        worm.Move(0, -1);
                        break;
                    case ConsoleKey.DownArrow:
                        worm.Move(0, 1);
                        break;
                    case ConsoleKey.LeftArrow:
                        worm.Move(-1, 0);
                        break;
                    case ConsoleKey.RightArrow:
                        worm.Move(1, 0);
                        break;
                    case ConsoleKey.Escape:
                        worm.isAlive = false;
                        break;
                }

                if (worm.CanEat(food))
                {
                    food = new Food();
                }
            }
        }
    }
}