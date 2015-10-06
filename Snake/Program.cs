using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snake
{
    class Program
    {
        static int xc = 0;
        static int yc = 0;
        static int width = 60;
        static int height = 40;
        static bool started = false;
        
        class Point
        {
            public int x = 0;
            public int y = 0;
            public Point(int x,int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        static Point[] body;
        static int shift = 1;

        static char[,] field;

        static int score = 0;

        static Random r;

        static void Main(string[] args)
        {

            r = new Random();
            body = new Point[1024];
            field = new char[width, height];
            Console.SetWindowSize(width+2, height+2);
            Console.SetBufferSize(width+2, height+2);

            Console.Clear();

            for (int i = 0; i < 10; i++ )
                SetPoint();

            Thread update = new Thread(Update);
            update.IsBackground = true;
            update.Start();

            while(true)
            {
                switch(Console.ReadKey(true).Key)
                {
                    case ConsoleKey.RightArrow:
                        if (xc != -1)
                        {
                            xc = 1;
                            yc = 0;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (yc != -1)
                        {
                            xc = 0;
                            yc = 1;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (xc != 1)
                        {
                            xc = -1;
                            yc = 0;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (yc != 1)
                        {
                            xc = 0;
                            yc = -1;
                        }
                        break;
                }
                started = true;
            }
        }

        static void Update()
        {

            int x = width / 2;
            int y = height / 2;
            int length = 6;
            x -= length;
            shift = length;
            for (int i = 0; i < length; i++ )
            {
                body[i] = new Point(x, y);
                x++;
            }
            while (true)
            {
                if (started)
                {
                    x += xc;
                    y += yc;

                    if (x < 1)
                        x += width;
                    if (y < 1)
                        y += height;
                    if (x > width)
                        x -= width;
                    if (y > height)
                        y -= height;


                    if (shift >= 1024)
                        shift -= 1024;

                    body[shift] = new Point(x, y);

                    if (field[x - 1, y - 1] == '☼')
                    {
                        score++;
                        Console.Title = "Score: " + score;
                        field[x - 1, y - 1] = ' ';
                        length += 1;
                        SetPoint();
                    }
                    else if (field[x - 1, y - 1] == 'x')
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine("YOU DED SON");
                        break;
                    }
                    field[x - 1, y - 1] = 'x';

                    int lastshift = shift - length;
                    if (lastshift < 0)
                        lastshift += 1024;

                    Point last = body[lastshift];
                    shift++;


                    Console.SetCursorPosition(x, y);
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write(" ");
                    Console.ResetColor();


                    if (last != null)
                    {
                        Console.SetCursorPosition(last.x, last.y);
                        field[last.x - 1, last.y - 1] = ' ';
                    }
                    Console.Write(" ");

                    Thread.Sleep(75);
                }
                else
                    Thread.Sleep(200);
            }
        }

        static void SetPoint()
        {
            int x = 1+r.Next(width);
            int y = 1+r.Next(height);
            field[x-1, y-1] = '☼';
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write('☼');
            Console.ResetColor();
        }
    }
}
