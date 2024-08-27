using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.C__Performance.Basic.ArrayFlattening
{
    // Flattening is quicker by 1.5-2!
    public class ArrayFlattening
    {
        private const int numElements = 10000;
        public static long MethodA()
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            int[,] list = new int[numElements, numElements];
            for (int i = 0; i < numElements; i++)
            {
                for (int j = 0; j < numElements; j++)
                {
                    list[i, j] = 1;
                }
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        public static long MethodB()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int[] list = new int[numElements * numElements];
            for(int i = 0;i < numElements;i++)
            {
                for(int j = 0;j < numElements;j++)
                {
                    int index = numElements * i + j; // <-- despite this multiplication, flatten array is quicker!
                    list[index] = 1;
                }
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }
        public static void Run()
        {
            // Initial cleanup
            MethodA();
            MethodB();

            long a = MethodA();
            long b = MethodB();

            Console.WriteLine($"Not flatten: {a}");
            Console.WriteLine($"Flatten: {b}");

            Console.ReadKey();
        }
    }
}
