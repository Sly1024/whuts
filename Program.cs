using System;
using System.Diagnostics;
namespace whuts
{
    class Program
    {
        static void Main(string[] args)
        {
            var piece120 = new P3D[] {
                (0, 0, 0), (1, 0, 0), (2, 0, 0), (3, 0, 0),
                (0, 0, 1), (0, 1, 1), (0, 2, 1), (0, 3, 1),
            };

            var piece003 = new P3D[] {
                (0, 0, 0), (1, 0, 0), (1, 1, 0), (1, 1, 1),
                (2, 1, 1), (3, 1, 1), (2, 2, 1), (2, 2, 2),
            };

            var piece217 = new P3D[] {
                (0, 0, 0), (1, 0, 0), (2, 0, 0), (3, 0, 0),
                (0, 1, 0), (1, 0, -1), (1, 0, 1), (1, -1, -1),
            };

            var watch = new Stopwatch();
            var solver = new Solver(piece217);

            watch.Start();
            solver.FindFit();
            watch.Stop();

            Console.WriteLine("Time: " + watch.Elapsed);
        }
    }
}
