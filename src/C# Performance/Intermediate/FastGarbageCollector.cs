using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CSharpLearnings.src.C__Performance.Intermediate.FastGarbageCollector1
{
    public class FastGarbageCollector
    {
        /*
    .NET GC:
        1) Is a generation -> so it won't have to mark lots of object each times it runs. (only Gen0, and sometimes Gen1, and rarely Gen2)
        2) It also tends to the big issue of potential copying a very large object 4 times - from Gen0 to Gen2. By introducing 2 heaps!
        
        Implicit assumptions:
        - Objects are either short-lived or long-lived.
        - Short-lived objects will be allocated and discarded within a single collection cycle.
        - Objects that survive two collection are long-lived
        - 90% of all small objects are short-lived
        - All large objects (85K+) are long-lived.

        Do not go againts these assumptions


        new object() -->
        
        size < 85K
        --> Small Object Heap (SOH)
        Gen: 0    | Gen:1       | Gen: 2
        frequent  |  infrequent |   very infrequent
        collection

        size > 85K
        ---> Large Object Heap (LOH) - very infrequent collection, no compaction.

        */

        /*
    What we learned?
        - *Limit* the number of objects you create.
        - Alolocate, use, and discard small objects as *fast* as possible.
        - *Re-use Large object.

        - Use only *small short-lived*, and *large long-lived* objects.

        - Increase lifetime or decrease size of large short-lived objects.
        - Decrease lifetime or increase size of small long-lived objects.

         */
    }
}
