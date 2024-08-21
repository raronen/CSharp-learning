using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.Parallel_programing.Async_Await.ValueTaskExample
{
    public class Demo
    {
        public static void Run()
        {
            /*
             * ValueTask is a stuct (!). It exists because Task is a class. 
             * And app may create lots of Tasks, causing GC to run, 
             * which might create memory bottleneck
             * 
             * ValueTask<TResult> can wrap TResult or Task<TResult>
             * */
        }
    }
}
