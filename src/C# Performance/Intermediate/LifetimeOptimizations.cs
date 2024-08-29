using CommandLine.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime;
using System.Text.Encodings;
using System.Threading.Tasks;

namespace CSharpLearnings.src.C__Performance.Intermediate.LifetimeOptimizations
{
    public class Pair
    {
        public Pair(int a, int b) { }
    }

    public class LifetimeOptimizations
    {
        public static void Run()
        {
            /*
        Optimizations:

            For Lifetime optimizations:
            The Garbagfe Collector assumes 90% of all small objects are short-lived, and all large objects are long-lived, so:

            1) Avoid large short-lived objects
            2) Avoid small long-lived objects

            Examples:                                                         */

            // Example 1:
            // Re-use shorted-lived large objects - "Object pooling":
            ArrayList list = new ArrayList(85190); // More than 85K - will get allocated to LOH.
            // Instead:
            UseTheList(list);
            list = new ArrayList(85190); // <--- Creates new list
            UseTheList(list);
            // Do: 
            UseTheList(list);
            list.Clear();                // Instead, clear it to use the same object.
            UseTheList(list);

            // Example 2:
            // Reallocate long-lived small objects after use:
            // Instead:
            ArrayList list2 = new ArrayList(85190); // Goes to LOH
            for (int i = 0; i < 10000; i ++)
            {
                list.Add(new Pair(i, i + 1)); // ArrayList is reference list, and will never dereference *all* the small Pair items, 10000 in count. So they all will move to Gen:2.
            }
            // Do:
            // Because int[] values are stored inside the array - there will only be 2 large arrays in LOH, and no items in Gen:2 like before.
            int[] list_1 = new int[85190];
            int[] list_2 = new int[85190];
            for (int i = 0;i < 10000; i ++)
            {
                list_1[i] = i;
                list_2[i] = i + 1;
            }

            // Example 3
            // For smal-lived large objects
            // Fine-tune the size of the objects:
            // Split objects, or reduce footprint:
            // Example for footprint:
            // Instead:
            int[] buffer = new int[32798]; // int is 4 bytes in size, so 4 * 32798 = 131192 bytes > 85K --> Stored in LOH.
            // Do: 
            byte[] buffer2 = new byte[32768]; // byte is 1 byte.. so this will actually be stored in SOH
            for (int i = 0; i < buffer.Length; i ++)
            {
                buffer[i] = GetBytes(i);
            }

            // Example 4
            // For - long-lived small objects
            // merge objects, resize lists.
            // public static ArrayList list = new ArrayList();
            // ..
            // <lost of other code>
            // ..
            // UseTheList(list)
            // Explanation - it is clear that the intention is that the list will be long-lived, it is defined as static,
            // so we better move it to LOH by:
            // public static ArrayList list = new ArrayList(851490);

        }

        public static void UseTheList(ArrayList list) { }
        public static int GetBytes(int i) { return 0; }
    }
}
