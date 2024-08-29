using BenchmarkDotNet.Analysers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.C__Performance.Advanced.ArraysOnTheStack
{
    // Don't use arrays on stack! there's no real improvments! 
    public class ArraysOnTheStack
    {
        private const int repetitions = 5000;

        // Using int[] on HEAP
        public static long MeasureA(int elements)
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            int[] list = new int[elements];
            for (int i = 0; i < repetitions; i++)
            {
                for (int j = 0; j < elements; j++)
                {
                    list[j] = j;
                }
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        // Using int[] on STACK
        public static long MeasureB(int elements)
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            unsafe
            {
                int* list = stackalloc int[elements];
                for (int i = 0; i < repetitions; i++)
                {
                    for (int j = 0; j < elements; j++)
                    {
                        list[j] = j;
                    }
                }
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        public static void Run()
        {
            for (int elements = 1000; elements < 100000; elements += 2000)
            {
                long duration1 = MeasureA(elements);
                long duration2 = MeasureB(elements);
                Console.WriteLine("{0}\t{1}\t{2}", elements, duration1, duration2);
            }

            Console.ReadKey();
        }
    }
}
