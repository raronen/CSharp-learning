using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.C__Performance.Intermediate.Gen0Optimizations.Gen0Optimizations
{
    public class Gen0Optimizations
    {
        public static void Run()
        {
            /*
        Optimizations:

            For Gen:0:
            The more objects in the generation 0, the more work the GC has to do, so:

            1) Optimizations - *limit * the number of objects you create.
            2) Allocate, use, and discard objects as *quickly * as possible.  

            Examples:                                                         */
            // Example 1:
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < 10000; i++)
            {
                // Instead:
                s.Append(i.ToString() + "KB"); // <--- String are immutable! String concatenation create extra string object on the heap!
                // Do:
                s.Append(i);
                s.Append("KB");
            }

            // Example 2:
            // Instead:
            ArrayList list = new ArrayList();  // <--- ArrayList is pushed to Gen:2, but it keeps creating Gen:0 items because the list itself is reference list!
            // Do:
            // List<int> list = new List<int>();
            for (int i = 0; i < 10000; i++)
            {
                list.Add(i); // If ArrayList, than i get boxed.
            }

            // Example 3:
            // Explanation: Object is small. because it takes lot of time between declaraion and actual use of obj, than it will probably get pushed to Gen:1 or Gen:2
            // Instead:
            // public static MyObject obj = new MyObject();
            // ..
            // <lots of other code>
            // ..
            // UseTheObject(obj)
            // Do:
            // public MyObject obj = new MyObject();
            // ..
            // <lots of other code>
            // ..
            // UseTheObject(obj)
            // obj = null.

        }
    }
}
