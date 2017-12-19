using System;
using System.Diagnostics;
using System.Drawing;

namespace MazeSolver.src
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch S = Stopwatch.StartNew();
            
            Maze M = new Maze(args[0]);

            Node<Point> Solution = AStar.Solve(M);

            if (Solution != null)
            {
                M.Save(args[0] + ".solved.bmp", Solution.ToArray());

                S.Stop();
                Console.WriteLine($"Total - {S.ElapsedMilliseconds}ms");
            }
            else
            {
                Console.WriteLine("No Solution...");
            }
        }
    }
}
