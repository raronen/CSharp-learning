using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.C__Performance.Basic.StringConcatenation
{
    /*
     *  Method A (Regular String) is slower 3.7 more than method B (String Builder).
     *  Why?
     *  1. It creates a new copy of a string each time.
     *  2. It leaves unreferenced pointers in the heap, work for GC.
     *  
     *  Regular string is better than StringBuilder lower than 4 additions.
     */
    public class StringConcatenation
    {
        private const int numRepeat = 10000000;
        public static long MeasureA()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string s = String.Empty;
            for (int i = 0;i < numRepeat;i++)
            {
                s = s + "";

            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        public static long MeasureB()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < numRepeat; i++)
            {
                sb.Append("a");
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        public static void Run()
        {
            // 1st run to eliminate any startup overhead
            MeasureA();
            MeasureB();

            // measurement run
            long stringDuration = MeasureA();
            long stringBuilderDuration = MeasureB();

            // Display results
            Console.WriteLine($"Regular string performance {stringDuration} microseconds");
            Console.WriteLine($"StringBuilder performance {stringBuilderDuration} microseconds");
            Console.WriteLine();
            Console.WriteLine("Method A is {0} times slower", 1.0 * stringBuilderDuration / stringDuration);
        }
    }
}
