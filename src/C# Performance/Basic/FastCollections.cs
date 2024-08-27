using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.C__Performance.Basic.FastCollections
{
    // Generic collection here are 6 times faster!
    // Why?
    // In System.Collection, including ArrayList, each element in ArrayList is a *reference* to a BOX integer that is located elsewhere in the heap.
    // Inlike System.Collection.Generic which its elements is actual int.
    // - Avoid collections in System.Collection.
    //
    // And giving initial size is the fastest!
    // And for Array event betters, becaues CIL has special supports for them.
    //
    // If you don't know the size in advance - use Generic.
    // If you do know the size in advance - use array.

    public class FastCollections
    {
        private const int numElements = 10000000;

        public static long MethodA()
        {
            ArrayList list = new ArrayList();
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            for (int i = 0; i < numElements; i++)
            {
                list.Add(i);
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        public static long MethodB()
        {
            List<int> list = new List<int>();
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            for (int i = 0; i < numElements; i++)
            {
                list.Add(i);
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        public static long MethodC()
        {
            List<int> list = new List<int>(numElements);
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            for (int i = 0; i < numElements; i++)
            {
                list.Add(i);
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        public static long MethodD()
        {
            int[] list = new int[numElements];
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            for (int i = 0; i < numElements; i++)
            {
                list[i] = i;
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        public static void Run()
        {
            // Cleanup initial noise
            MethodA();
            MethodB();
            MethodC();
            MethodD();

            long a = MethodA();
            long b = MethodB();
            long c = MethodC();
            long d = MethodD();

            Console.WriteLine($"Method A took {a} milliseconds");
            Console.WriteLine($"Method B took {b} milliseconds");
            Console.WriteLine($"Method C took {c} milliseconds");
            Console.WriteLine($"Method D took {d} milliseconds");

            Console.ReadKey();
        }
    }
}
