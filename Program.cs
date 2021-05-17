using System;
using System.Diagnostics;
namespace whuts
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = new Stopwatch();
            watch.Start();

            for (int i = 0; i < Unfoldings.all.Length; i++)
            {
                P3D[] piece = Unfoldings.all[i];
                Console.Write("Tiling piece #" + (i + 1) + "... ");
                var solver = new Solver(piece);

                if (!solver.FindFit())
                {
                    Console.WriteLine("NO SOLUTION FOUND !!!");
                }
            }

            watch.Stop();
            Console.WriteLine("Time: " + watch.Elapsed);
        }
    }
}
