using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace MazeGenerator
{
    class Program
    {
        static Point ParseArgs(string[] args)
        {
            int width = -1;
            int height = -1;
            if (args.Length != 2 ||
                !int.TryParse(args[0], out width) ||
                !int.TryParse(args[1], out height) ||
                width <= 0 || height <= 0)
            {
                Console.WriteLine(string.Format("Usage: {0} <width> <height>", Process.GetCurrentProcess().ProcessName));
                Environment.Exit(1);
            }
            if (width < 30 || height < 30)
            {
                Console.WriteLine("WARNING: Using sizes lower than 30 results in unpredictable behaviour.");
            }
            return new Point(width, height);
        }

        static void Main(string[] args)
        {
            Stopwatch S = Stopwatch.StartNew();
            Point size = ParseArgs(args);
            new Maze(size.X, size.Y, Color.Black, Color.White).Save("maze.bmp");
            S.Stop();
            Console.WriteLine(string.Format("Elapsed Time {0:00}:{1:00}:{2:00}.{3:00}", S.Elapsed.Hours, S.Elapsed.Minutes, S.Elapsed.Seconds, S.Elapsed.Milliseconds / 10));
        }
    }
}
