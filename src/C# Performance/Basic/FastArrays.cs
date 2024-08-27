using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.C__Performance.Basic.FastArrays
{
    public class FastArrays
    {
        public static void Run()
        {
            // There are 3 types of arrays in C#:

            // 1-dimenstional array:
            // declaration
            int[] arr = new int[4]; //  <--- Fastest, beause of built in support.
            // access:
            int i = arr[2];

            // 2-dimensional array:
            int[,] arr2 = new int[2, 2]; // <--- Slowest, just a regular class
            int j = arr2[1, 1];

            // jagged array:
            int[][] arr3 = new int[2][];  // <--- 2nd fastest, because its array of arrays, utilizing built in support.
            arr3[0] = new int[2];
            arr3[1] = new int[1];

            int k = arr3[1][0];

            // Clean initial noise:
            MeasureA();
            MeasureB();
            MeasureC();

            // Measurements:
            long a = MeasureA();
            long b = MeasureB();
            long c = MeasureC();

            Console.WriteLine($"int[] time: {a}");
            Console.WriteLine($"int[,] time: {b}");
            Console.WriteLine($"int[][] time: {c}");

            Console.ReadKey();
        }

        private const int numElements = 10000;
        public static long MeasureA()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            int[] list = new int[numElements * numElements];
            for (int i = 0; i < numElements; i++)
            {
                list[i] = i;
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        public static long MeasureB()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            int[,] list = new int[numElements, numElements];
            for (int i = 0; i < numElements; i++)
            {
                for (int j = 0; j < numElements; j++)
                {
                    list[i,j] = 1;
                }
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        public static long MeasureC()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            int[][] list = new int[numElements][];
            for (int i = 0; i < numElements; i++)
            {
                // Big disatvantage for jagged arrays, because you need to initialize the 1st level array
                list[i] = new int[numElements];
                for (int j = 0; j < numElements; j++)
                {
                    list[i][j] = 1;
                }
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
    }
}
