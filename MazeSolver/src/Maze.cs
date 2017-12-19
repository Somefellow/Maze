using FastBitmapLib;
using Gifed;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;

namespace MazeSolver.src
{
    class Maze
    {
        public static readonly Point[] Shifts = { new Point(0, -1), new Point(1, 0), new Point(0, 1), new Point(-1, 0) };

        static readonly Point _NullPoint = new Point(int.MinValue, int.MinValue);

        List<Grid<State>> _States;
        Point _Start = _NullPoint;
        Point _End = _NullPoint;

        public Point Start
        {
            get
            {
                return _Start;
            }
        }
        
        public Point End
        {
            get
            {
                return _End;
            }
        }

        void FindStartEnd()
        {
            for (int i = 0; i < _States.Last().Width; i++)
            {
                if (_States.Last().GetValue(i, 0) == State.Explorable)
                {
                    if (_Start == _NullPoint)
                    {
                        _Start = new Point(i, 0);
                    }
                    else if (_End == _NullPoint)
                    {
                        _End = new Point(i, 0);
                    }
                }

                if (_States.Last().GetValue(i, _States.Last().Height - 1) == State.Explorable)
                {
                    if (_Start == _NullPoint)
                    {
                        _Start = new Point(i, _States.Last().Height - 1);
                    }
                    else if (_End == _NullPoint)
                    {
                        _End = new Point(i, _States.Last().Height - 1);
                    }
                }
            }

            for (int i = 0; i < _States.Last().Height; i++)
            {
                if (_States.Last().GetValue(0, i) == State.Explorable)
                {
                    if (_Start == _NullPoint)
                    {
                        _Start = new Point(0, i);
                    }
                    else if (_End == _NullPoint)
                    {
                        _End = new Point(0, i);
                    }
                }

                if (_States.Last().GetValue(_States.Last().Width - 1, i) == State.Explorable)
                {
                    if (_Start == _NullPoint)
                    {
                        _Start = new Point(_States.Last().Width - 1, i);
                    }
                    else if (_End == _NullPoint)
                    {
                        _End = new Point(_States.Last().Width - 1, i);
                    }
                }
            }
        }

        public State GetState(int x, int y)
        {
            try
            {
                return _States.Last().GetValue(x, y);
            }
            catch (Exception)
            {
                return State.Unexplorable;
            }
        }

        public void SetState(State S, int x, int y)
        {
            _States.Add(new Grid<State>(_States.Last()));
            _States.Last().SetValue(S, x, y);
        }

        static Bitmap ConvertTo32bpp(Image img)
        {
            var bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppRgb);
            using (var gr = Graphics.FromImage(bmp))
                gr.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height));
            return bmp;
        }

        public Maze(string Filename)
        {
            // Start load bitmap
            Stopwatch S = Stopwatch.StartNew();

            FastBitmap B = new FastBitmap(ConvertTo32bpp(new Bitmap(Filename)));
            _States = new List<Grid<State>> { new Grid<State>(B.Width, B.Height) };
            S.Stop();
            Console.WriteLine($"Load  - {S.ElapsedMilliseconds}ms");
            // Finish load bitmap

            // Start parse bitmap
            S.Restart();

            B.Lock();

            Parallel.For(0, B.Width, x =>
            {
                Parallel.For(0, B.Height, y =>
                {
                    _States.Last().SetValue(B.GetPixelInt(x, y) == -1 ? State.Explorable : State.Unexplorable, x, y);
                });
            });

            FindStartEnd();

            B.Unlock();

            S.Stop();
            Console.WriteLine($"Parse - {S.ElapsedMilliseconds}ms");
            // Finish parse bitmap
        }

        public int Height
        {
            get
            {
                return _States.Last().Height;
            }
        }

        public int Width
        {
            get
            {
                return _States.Last().Width;
            }
        }

        public int Heuristic(int X, int Y)
        {
            return Math.Abs(End.X - X) + Math.Abs(End.Y - Y);
        }

        public int Heuristic(Point P)
        {
            return Heuristic(P.X, P.Y);
        }

        public void Save(string Filename, Point[] Solution)
        {
            // Start create bitmap
            Stopwatch S = Stopwatch.StartNew();

            /*Bitmap B = new Bitmap(Width, Height);

            using (var FastB = new FastBitmap(B))
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        B.SetPixel(x, y, _Walkable.GetValue(x, y) ? Color.White : Color.Black);
                    }
                }

                foreach (Point P in Solution)
                {
                    B.SetPixel(P.X, P.Y, Color.Red);
                }
            }

            S.Stop();
            Console.WriteLine($"Draw  - {S.ElapsedMilliseconds}ms");
            // Finish create bitmap

            // Start save bitmap
            S.Restart();

            //_Gif.Save(@"out\output.gif");
            B.Save(Filename);*/

            AnimatedGif Gif = new AnimatedGif();
            int Gap = 1;// _States.Count / 100;

            for (int i = 0; i < _States.Count; i += Gap)
            {
                Gif.AddFrame(GridToBitmap(_States[i]), 1);
            }

            Gif.Save("solution.gif");

            S.Stop();
            Console.WriteLine($"Save  - {S.ElapsedMilliseconds}ms");
            // Finish save bitmap
        }

        private Image GridToBitmap(Grid<State> grid)
        {
            Bitmap B = new Bitmap(grid.Width, grid.Height, PixelFormat.Format4bppIndexed);
            FastBitmap FB = new FastBitmap(B);

            FB.Lock();

            Parallel.For(0, grid.Width, x =>
            {
                Parallel.For(0, grid.Height, y =>
                {
                    switch (grid.GetValue(x, y))
                    {
                        case State.Unexplorable:
                            FB.SetPixel(x, y, Color.Black);
                            break;
                        case State.Explorable:
                            FB.SetPixel(x, y, Color.White);
                            break;
                        case State.Explored:
                            FB.SetPixel(x, y, Color.Red);
                            break;
                    }
                });
            });

            FB.Unlock();

            return B;
        }
    }
}
