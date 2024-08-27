using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.C__Performance.Basic.ThrowingExceptionCont
{
    /*
     * Exception is more than 10x times slower! 
     * Better to check if condition exists rather than throw an exception.
     * 
     * Throws exception %10 of the time: 596ms
     * Adding extra '.Contains' check to avoid throwing exception: 39ms
     * 
     * 
     * Use exceptions for fatal conditions that require an abort.
     * Don't put try-catch blocks in deeply nested code.
     * Never user catch(Exception) to catch all exceptions, which also catch non-fatal conditions and it won't be immediatly obious why code is slow
     * If you write an API, don't use exceptions for non-critical boundary condition, like converting invalid data, or failing a lookup operation. Consider the tryParse example.
     * Never use exception to control the flow of your program.
    */
    public class ThrowingExceptionCont
    {
        private const int elements = 1000000;

        private static List<int> numbers = new List<int>();
        private static Dictionary<int, string> lookup = new Dictionary<int, string>
        {
            { 0, "zero" },
            { 1, "one" },
            { 2, "two" },
            { 3, "three" },
            { 4, "four" },
            { 5, "five" },
            { 6, "six" },
            { 7, "seven" },
            { 8, "eight" },
            { 9, "nine" }
        };

        public static void PrepareList()
        {
            Random random = new Random();
            for (int i = 0; i < elements; i++)
            {
                numbers.Add(random.Next(11)); // <--- For 10% of the times, it'll add the number 10
            }
        }

        public static long MeasureA()
        {
                Stopwatch stopwatch = Stopwatch.StartNew();
                stopwatch.Start();
                for (int i = 0; i < elements; i++)
                {
                    string s = null;
                    try
                    {
                        s = lookup[numbers[i]]; // <--- For 10% of the times, it'll add { 10: "ten" } which doesn't exists, and throw an exception
                    }
                    catch (KeyNotFoundException)
                    {

                    }
                }
                stopwatch.Stop();
                return stopwatch.ElapsedMilliseconds;
        }

        public static long MeasureB()
        {
            Stopwatch stopwatch= Stopwatch.StartNew();
            stopwatch.Start();
            for (int i = 0;i < elements; i++)
            {
                string s = null;
                int key = numbers[i];
                if (lookup.ContainsKey(key))  // Adding check to avoid exception being thrown for 10% of the cases.
                    s = lookup[key];
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
        public static void Run()
        {
            PrepareList();

            long a = MeasureA();
            long b = MeasureB();

            Console.WriteLine("Throws exception %10 of the time: " + a + "ms");
            Console.WriteLine("Adding extra '.Contains' check to avoid throwing exception: " + b + "ms");

            Console.ReadKey();
        }
    }
}
