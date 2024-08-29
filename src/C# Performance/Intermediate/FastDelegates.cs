using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.C__Performance.Intermediate.FastDelegates
{
    /*
        Manual calls: 323ms         <-- faster!
        Unicast delegates: 415ms
        Multicast delegates: 1205ms <-- slower! 
    
        The reason: behind the scene, a delegeate, is implemented, by MulticastDelegate class. This class is optimized for Unicast delegate, 
        it uses a method on a target proprty to directly call a single method.
        But, for multicast delegate, it uses an invokation list. An internal generic list to each method.
        The overhead of stepping throw the invokation list is what cause this large diff in performance (x2-3 slower)
    

        - Use delegates in your code where it's convenient.
        - Remove delegate sfrom mission critical code sections for a 9% performance boost.
        - *Always avoid* multicast delegates in mission critical code because they are x2-3 slower.
    */
    public class FastDelegates
    {
        public const int REPETITIONS = 1000000;
        public const int EXPERIMENTS = 100;

        public delegate void AddDelegate(int a, int b, out int result);

        public static void Add1(int a, int b, out int result)
        {
            result = a + b;
        }

        public static void Add2(int a, int b, out int result)
        {
            result = a + b;
        }

        // Call Add1 and Add2 manualyl
        public static long Measure1()
        {
            int result = 0;
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            for (int i = 0; i < REPETITIONS; i++)
            {
                Add1 (1234, 2345, out result);
                Add2 (1234, 2345, out result);
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        // Call Add1 and Add2 using 2 unicat delegates
        public static long Measure2()
        {
            int result = 0;
            Stopwatch sw = Stopwatch.StartNew();
            AddDelegate add1 = Add1;
            AddDelegate add2 = Add2;
            sw.Start();
            for (int i = 0; i < REPETITIONS; i++)
            {
                add1(1234, 2345, out result);
                add2(1234, 2345, out result);
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        // Call Add1 and Add2 using 1 multicast delegate
        public static long Measure3()
        {
            int result = 0;
            Stopwatch sw = Stopwatch.StartNew();
            AddDelegate multi = Add1;
            multi += Add2;
            sw.Start();
            for (int i = 0; i < REPETITIONS; i++)
            {
                multi(1234, 2345, out result);
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        public static void Run()
        {
            long manual = 0;
            long unicast = 0;
            long multicast = 0;
            for (int i = 0; i < EXPERIMENTS; i++)
            {
                manual += Measure1();
                unicast += Measure2();
                multicast += Measure3();
            }

            Console.WriteLine($"Manual calls: {manual}ms");
            Console.WriteLine($"Unicast delegates: {unicast}ms");
            Console.WriteLine($"Multicast delegates: {multicast}ms");

            Console.ReadKey();
        }
    }
}
