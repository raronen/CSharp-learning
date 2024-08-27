using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.C__Performance.Basic.ThrowingException
{
    // Overhead of throwing an exception is massive!
    // ~10,000 more times slower (if throwing exception each time)
    // --> never use exception in mission critical code
    //
    // We also need to make sure 3rd library doesn't throw exception..
    // Look at example: int.Parse vs int.TryParse!! 10x times slower
    //
    //
/*
Regular: 0ms
Just throw operation: 43ms             <--- slow!
int.Parse(throws exception) - 2221ms   <--- slow!
int.TryParse - 14ms
*/

    public class ThrowingException
    {
        private const int repetitions = 10000;
        public static long MeasureA()
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            int count = 0;
            for (int i = 0; i < repetitions; i++)
            {
                count = count + 1;
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        public static long MeasureB()
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            int count = 0;
            for (int i = 0; i < repetitions; i++)
            {
                try
                {
                    count = count + 1;
                    throw new InvalidOperationException();
                } catch (InvalidOperationException)
                {

                }
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }


        // Demonstrating on 3rd party code:
        private const int elements = 1000000;
        private const int digits = 5;

        private static char[] digitArray = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'X' };
        private static List<string> numbers = new List<string>();

        public static void PrepareList()
        {
            Random random = new Random();
            for (int i = 0; i < elements; i++)
            {
                StringBuilder sb = new StringBuilder();
                for (int d = 0; d < digits; d++)
                {
                    int index = random.Next(11);
                    sb.Append(digitArray[index]);
                }
                numbers.Add(sb.ToString());
            }
        }

        public static long MeasureThirdPartyA()
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            for (int i = 0; i < elements; i++)
            {
                try
                {
                    int.Parse(numbers[i]); // <--- Throws FormatException if doesn't successed
                }
                catch (FormatException)
                {
                }
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        public static long MeasureThirdPartyB()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for(int i = 0;i < elements; i++)
            {
                int result = 0;
                int.TryParse(numbers[i], out result);
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        public static void Run()
        {
            // Cleanupcode
            MeasureA();
            MeasureB();

            long a = MeasureA();
            long b = MeasureB();

            Console.WriteLine("Regular: " + a + "ms");
            Console.WriteLine("Just throw operation: " + b + "ms");

            PrepareList();

            long thirdPartyA = MeasureThirdPartyA();
            long thirdPartyB = MeasureThirdPartyB();

            Console.WriteLine($"int.Parse (throws exception) - {thirdPartyA}ms");
            Console.WriteLine($"int.TryParse - {thirdPartyB}ms");

            Console.ReadKey();
        }
    }
}
