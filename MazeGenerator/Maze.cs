using System;
using System.Collections.Generic;
using System.Drawing;

namespace MazeGenerator
{
    class Maze
    {
        static readonly Color unvisited = Color.Gray;
        static readonly Random R = new Random();
        Bitmap _bitmap;

        public Maze(int width, int height, Color wall, Color floor)
        {
            Console.WriteLine(string.Format("Constructing {0} x {1} Maze", width, height));
            _bitmap = new Bitmap((width * 2) + 1, (height * 2) + 1);
            Prepare(wall, floor);
            //Save(@"C:\Users\aidas\Dropbox\MazeSolver\MazeGenerator\bin\Debug\debug.bmp");
            Generate(floor);
        }

        struct State
        {
            public List<Point> Shifts;
            public Point Location;
        }

        void Generate(Color floor)
        {
            Console.WriteLine("Generating Maze");

            Point[] Shifts = { new Point(1, 0), new Point(0, 1), new Point(-1, 0), new Point(0, -1) };

            Stack<State> S = new Stack<State>();

            State Initial = new State();
            Initial.Shifts = new List<Point>(Shifts);
            Initial.Location = new Point(1, 1);
            S.Push(Initial);

            int Unvisited = (_bitmap.Height - 1) * (_bitmap.Height - 1) / 2;
            int OnePercent = Unvisited / 200;
            if (OnePercent == 0) OnePercent++;

            while (S.Count > 0)
            {
                State currentState = S.Pop();
                if (currentState.Shifts.Count == 0) continue;
                Point Shift = currentState.Shifts[R.Next(currentState.Shifts.Count)];
                Point Location = new Point(currentState.Location.X, currentState.Location.Y);
                currentState.Shifts.Remove(Shift);
                S.Push(currentState);

                _bitmap.SetPixel(Location.X, Location.Y, floor);

                try
                {
                    if (_bitmap.GetPixel(Location.X + (Shift.X * 2), Location.Y + (Shift.Y * 2)).ToArgb() == unvisited.ToArgb())
                    {
                        if (Unvisited % OnePercent == 0)
                        {
                            if (Unvisited != (_bitmap.Height - 1) * (_bitmap.Height - 1) / 2)
                            {
                                Console.CursorTop--;
                            }
                            Console.WriteLine(string.Format("{0}%", 201 - (Unvisited / OnePercent)));
                        }
                        Unvisited--;
                        _bitmap.SetPixel(Location.X + Shift.X, Location.Y + Shift.Y, floor);

                        State newState = new State();
                        newState.Shifts = new List<Point>(Shifts);
                        newState.Location = new Point(Location.X + (Shift.X * 2), Location.Y + (Shift.Y * 2));
                        S.Push(newState);
                    }
                }
                catch (ArgumentOutOfRangeException) { }
            }
        }

        void Prepare(Color wall, Color floor)
        {
            Console.WriteLine("Creating Canvas");

            int Complete = 0;
            int OnePercent = (_bitmap.Width * _bitmap.Height) / 100;
            if (OnePercent == 0) OnePercent++;
            Console.WriteLine("0%");

            for (int y = 0; y < _bitmap.Height; y++)
            {
                for (int x = 0; x < _bitmap.Width; x++)
                {
                    if (++Complete % OnePercent == 0)
                    {
                        Console.CursorTop--;
                        Console.WriteLine(string.Format("{0}%", Complete / OnePercent));
                    }

                    if (x + 1 % 2 == 0 && y + 1 % 2 == 0)
                    {
                        _bitmap.SetPixel(x, y, unvisited);
                    }
                    else
                    {
                        _bitmap.SetPixel(x, y, wall);
                    }
                }
            }

            for (int x = 0; x < _bitmap.Width / 2; x++)
            {
                for (int y = 0; y < _bitmap.Height / 2; y++)
                {
                    _bitmap.SetPixel((x * 2) + 1, (y * 2) + 1, unvisited);
                }
            }

            _bitmap.SetPixel(1, 0, floor);
            _bitmap.SetPixel(_bitmap.Width - 2, _bitmap.Height - 1, floor);
        }

        public void Save(string file)
        {
            Console.WriteLine("Writing to disk");
            _bitmap.Save(file);
        }
    }
}
