using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.C__Performance.Basic.BoxingOverhead
{
    /*
     * 1. In Example below, MeasureB is ~5.5 slower than Measure A!
     * 2. In mission critical code, Avoid using those classes because they use object or object array for internal storage.
     *    Almost all System.Collections: ArrayList, CollectionBase, HashTable, etc..
     *    Also System.Data: DataSet. DataReader is faster.. 
     *    Instead! use System.Collection.Generic:
     *    HashSet<T>, List<T>, Queue<T>, etc..
     *    or Array[KnownLength].
     *  
     * 
     */
    public class BoxingOverhead
    {
        private const int arraySize = 10000000;
        public static long MeasureA()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int a = 1;
            for (int i = 0;i < arraySize;i++)
            {
                a = a + i;
                /*
                 *  * Disassembly just for this line:
                21:                 a = a + i;
                00007FF9DFE6754F  mov         ecx,dword ptr [rbp+44h]  
                00007FF9DFE67552  add         ecx,dword ptr [rbp+40h]  
                00007FF9DFE67555  mov         dword ptr [rbp+44h],ecx 
                */

            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        public static long MeasureB()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            object a = 1; // object --> stored in heap
            for (int i = 0; i < arraySize; i++)
            {
                a = (int)a + i;
/*
 * Disassembly just for this line:
33:                 a = (int)a + i;
00007FF9DFE6A233 mov         rcx,7FF9DFDD1188h
00007FF9DFE6A23D call        CORINFO_HELP_NEWSFAST(07FFA3F9ED140h)
00007FF9DFE6A242 mov         qword ptr[rbp + 28h], rax
00007FF9DFE6A246 mov         rdx,qword ptr[rbp + 50h]
00007FF9DFE6A24A mov         rcx,7FF9DFDD1188h
00007FF9DFE6A254 call        qword ptr[CLRStub[MethodDescPrestub]@00007FF9DFE54408(07FF9DFE54408h)]
00007FF9DFE6A25A mov         ecx,dword ptr[rax]
00007FF9DFE6A25C add         ecx,dword ptr[rbp + 4Ch]
00007FF9DFE6A25F mov         rax,qword ptr[rbp + 28h]
00007FF9DFE6A263 mov         dword ptr[rax + 8], ecx
00007FF9DFE6A266 mov         rcx,qword ptr[rbp + 28h]
00007FF9DFE6A26A mov         qword ptr[rbp + 50h], rcx
                    */
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
            long intDuration = MeasureA();
            long objDuration = MeasureB();

            // Display results
            Console.WriteLine($"Integer performance {intDuration} microseconds");
            Console.WriteLine($"Object performance {objDuration} microseconds");
            Console.WriteLine();
            Console.WriteLine("Method B is {0} times slower", 1.0 * objDuration / intDuration);
        }
    }
}
