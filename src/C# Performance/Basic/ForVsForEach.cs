using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.C__Performance.Basic.ForVsForEach
{
    /*
     *  
     *  ArrayList takes more time because of boxing/unboxing.
        ArrayList for:       40ms <--- Optimizing forEach to for is only relavent for ArrayList
        ArrayList forEach:   69ms 

        GenericList for:     22ms
        GenericList forEach: 23ms

        int[] for:           12ms
        int[] forEach:       11ms
    */
    public class ForVsForEach
    {
        private const int numElements = 10000000;

        private static ArrayList arrayList = new ArrayList(numElements);
        private static List<int> genericList = new List<int>(numElements);
        private static int[] array = new int[numElements];

        public static void PrepareList()
        {
            Random random = new Random();
            for (int i = 0; i < numElements; i++)
            {
                int number = random.Next(256);
                arrayList.Add(number);
                genericList.Add(number);
                array[i] = number;
            }
        }
        public static long MeasureA1()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < numElements; i++)
            {
                int result = (int) arrayList[i];
            }
            /*  21 lines
                46:             for (int i = 0; i < numElements; i++)
                00007FF98B97B993  xor         ecx,ecx  
                00007FF98B97B995  mov         dword ptr [rbp+54h],ecx  
                00007FF98B97B998  nop  
                00007FF98B97B999  jmp         CSharpLearnings.src.C__Performance.Basic.ForVsForEach.ForVsForEach.MeasureA1()+0B8h (07FF98B97B9E8h)  
                47:             {
                00007FF98B97B99B  nop  
                48:                 int result = (int) arrayList[i];
                00007FF98B97B99C  mov         rcx,27EF1C020F8h  
                00007FF98B97B9A6  mov         rcx,qword ptr [rcx]  
                00007FF98B97B9A9  mov         qword ptr [rbp+20h],rcx  
                00007FF98B97B9AD  mov         rcx,qword ptr [rbp+20h]  
                00007FF98B97B9B1  mov         edx,dword ptr [rbp+54h]  
                00007FF98B97B9B4  mov         rax,qword ptr [rbp+20h]  
                00007FF98B97B9B8  mov         rax,qword ptr [rax]  
                00007FF98B97B9BB  mov         rax,qword ptr [rax+48h]  
                00007FF98B97B9BF  call        qword ptr [rax+18h]  
                00007FF98B97B9C2  mov         qword ptr [rbp+30h],rax  
                00007FF98B97B9C6  mov         rdx,qword ptr [rbp+30h]  
                00007FF98B97B9CA  mov         rcx,7FF98B8E1188h  
                00007FF98B97B9D4  call        qword ptr [CLRStub[MethodDescPrestub]@00007FF98B964408 (07FF98B964408h)]  
                00007FF98B97B9DA  mov         ecx,dword ptr [rax]  
                00007FF98B97B9DC  mov         dword ptr [rbp+50h],ecx 
            */
            
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        public static long MeasureA2()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            foreach (int i in arrayList)
            {
                int result = i;
            }
            /*
             * 28 lines
            84:             foreach (int i in arrayList)
            00007FF98B984AF5  nop  
            00007FF98B984AF6  mov         rcx,27EF1C020F8h  
            00007FF98B984B00  mov         rcx,qword ptr [rcx]  
            00007FF98B984B03  mov         qword ptr [rbp+28h],rcx  
            00007FF98B984B07  mov         rcx,qword ptr [rbp+28h]  
            00007FF98B984B0B  mov         rax,qword ptr [rbp+28h]  
            00007FF98B984B0F  mov         rax,qword ptr [rax]  
            00007FF98B984B12  mov         rax,qword ptr [rax+58h]  
            00007FF98B984B16  call        qword ptr [rax]  
            00007FF98B984B18  mov         qword ptr [rbp+48h],rax  
            00007FF98B984B1C  mov         rcx,qword ptr [rbp+48h]  
            00007FF98B984B20  mov         qword ptr [rbp+70h],rcx  
            00007FF98B984B24  nop  
            00007FF98B984B25  jmp         CSharpLearnings.src.C__Performance.Basic.ForVsForEach.ForVsForEach.MeasureA2()+0DDh (07FF98B984B5Dh)  
            00007FF98B984B27  mov         rcx,qword ptr [rbp+70h]  
            00007FF98B984B2B  mov         r11,offset Pointer to: CLRStub[VSD_LookupStub]@00007FF98B9708D0 (07FF98B8201D8h)  
            00007FF98B984B35  call        qword ptr [r11]  
            00007FF98B984B38  mov         qword ptr [rbp+38h],rax  
            00007FF98B984B3C  mov         rdx,qword ptr [rbp+38h]  
            00007FF98B984B40  mov         rcx,7FF98B8E1188h  
            00007FF98B984B4A  call        qword ptr [CLRStub[MethodDescPrestub]@00007FF98B964408 (07FF98B964408h)]  
            00007FF98B984B50  mov         edx,dword ptr [rax]  
            00007FF98B984B52  mov         dword ptr [rbp+6Ch],edx  
            85:             {
            00007FF98B984B55  nop  
            86:                 int result = i;
            00007FF98B984B56  mov         edx,dword ptr [rbp+6Ch]  
            00007FF98B984B59  mov         dword ptr [rbp+68h],edx  
            */
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        public static long MeasureB1()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < numElements; i++)
            {
                int result = genericList[i];
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        public static long MeasureB2()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            foreach (int i in genericList)
            {
                int result = i;
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        public static long MeasureC1()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < numElements; i++)
            {
                int result = array[i];
            }
            /*
             *  18 lines
                152:             for (int i = 0; i < numElements; i++)
                00007FF98B98EC9F  xor         ecx,ecx  
                00007FF98B98ECA1  mov         dword ptr [rbp+44h],ecx  
                00007FF98B98ECA4  nop  
                00007FF98B98ECA5  jmp         CSharpLearnings.src.C__Performance.Basic.ForVsForEach.ForVsForEach.MeasureC1()+097h (07FF98B98ECD7h)  
                   153:             {
                00007FF98B98ECA7  nop  
                   154:                 int result = array[i];
                00007FF98B98ECA8  mov         rcx,27EF1C02108h  
                00007FF98B98ECB2  mov         rcx,qword ptr [rcx]  
                00007FF98B98ECB5  mov         eax,dword ptr [rbp+44h]  
                00007FF98B98ECB8  cmp         eax,dword ptr [rcx+8]  
                00007FF98B98ECBB  jb          CSharpLearnings.src.C__Performance.Basic.ForVsForEach.ForVsForEach.MeasureC1()+082h (07FF98B98ECC2h)  
                00007FF98B98ECBD  call        00007FF9EB60F8F0  
                00007FF98B98ECC2  mov         edx,eax  
                00007FF98B98ECC4  lea         rcx,[rcx+rdx*4+10h]  
                00007FF98B98ECC9  mov         ecx,dword ptr [rcx]  
                00007FF98B98ECCB  mov         dword ptr [rbp+40h],ecx  
            */
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        public static long MeasureC2()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            foreach (int i in array)
            {
                int result = i;
                /*
                    21 lines
                    185:             foreach (int i in array)
                    00007FF98B99504E  nop  
                    00007FF98B99504F  mov         rcx,27EF1C02108h  
                    00007FF98B995059  mov         rcx,qword ptr [rcx]  
                    00007FF98B99505C  mov         qword ptr [rbp+50h],rcx  
                    00007FF98B995060  xor         ecx,ecx  
                    00007FF98B995062  mov         dword ptr [rbp+4Ch],ecx  
                    00007FF98B995065  nop  
                    00007FF98B995066  jmp         CSharpLearnings.src.C__Performance.Basic.ForVsForEach.ForVsForEach.MeasureC2()+0A5h (07FF98B995095h)  
                    00007FF98B995068  mov         rcx,qword ptr [rbp+50h]  
                    00007FF98B99506C  mov         eax,dword ptr [rbp+4Ch]  
                    00007FF98B99506F  cmp         eax,dword ptr [rcx+8]  
                    00007FF98B995072  jb          CSharpLearnings.src.C__Performance.Basic.ForVsForEach.ForVsForEach.MeasureC2()+089h (07FF98B995079h)  
                    00007FF98B995074  call        00007FF9EB60F8F0  
                    00007FF98B995079  mov         edx,eax  
                    00007FF98B99507B  lea         rcx,[rcx+rdx*4+10h]  
                    00007FF98B995080  mov         ecx,dword ptr [rcx]  
                    00007FF98B995082  mov         dword ptr [rbp+48h],ecx  
                       186:             {
                    00007FF98B995085  nop  
                       187:                 int result = i;
                    00007FF98B995086  mov         ecx,dword ptr [rbp+48h]  
                    00007FF98B995089  mov         dword ptr [rbp+44h],ecx
                */
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        public static void Run()
        {
            PrepareList();
            long a1 = MeasureA1();
            long a2 = MeasureA2();
            long b1 = MeasureB1();
            long b2 = MeasureB2();
            long c1 = MeasureC1();
            long c2 = MeasureC2();

            Console.WriteLine($"Regular for loop for ArrayList: {a1}ms");
            Console.WriteLine($"forEach loop for ArrayList: {a2}ms");
            Console.WriteLine();
            Console.WriteLine($"Regular for loop for GenericList: {b1}ms");
            Console.WriteLine($"forEach loop for GenericList: {b2}ms");
            Console.WriteLine();
            Console.WriteLine($"Regular for loop for int[]: {c1}ms");
            Console.WriteLine($"forEach loop for int[]: {c2}ms");

            Console.ReadKey();
        }
    }
}
