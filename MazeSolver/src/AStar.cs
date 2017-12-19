using System.Drawing;
using Priority_Queue;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Threading.Tasks;

namespace MazeSolver.src
{
    class AStar
    {
        public static Node<Point> Solve(Maze M)
        {
            Stopwatch S = Stopwatch.StartNew();

            FastPriorityQueue<Node<Point>> Q = new FastPriorityQueue<Node<Point>>(M.Width * M.Height);

            Q.Enqueue(new Node<Point>(null, M.Start), M.Heuristic(M.Start));

            Node<Point> Solution = null;

            while (Q.Count != 0)
            {
                Node<Point> N = Q.Dequeue();

                M.SetState(State.Explored, N.Value.X, N.Value.Y);

                if (N.Value == M.End)
                {
                    Solution = N;
                    break;
                }

                foreach (Point Shift in Maze.Shifts)
                {
                    Point P = N.Value;
                    P.Offset(Shift);

                    //if (P.X < 0 || P.X >= M.Width || P.Y < 0 || P.Y >= M.Height) continue;

                    if (M.GetState(P.X, P.Y) == State.Explorable)
                    {
                        Q.Enqueue(new Node<Point>(N, P), M.Heuristic(P) /*+ N.Depth + 1*/);
                    }
                }
            }

            S.Stop();
            Console.WriteLine($"Solve - {S.ElapsedMilliseconds}ms");

            return Solution;
        }
    }
}
