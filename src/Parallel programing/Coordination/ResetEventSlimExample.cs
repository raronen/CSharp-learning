using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.Parallel_programing.Coordination.ResetEventSlimExample
{
    public class Demo
    {
        public static void Run()
        {
            // Slim is improvement of the current implementation. We need to use Slim.
            // First example - ManualResetEventSlim
            var evt = new ManualResetEventSlim(); // In Manual -> After evt,set, it allways be set.

            Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Boiling water");
                evt.Set(); // Unblock the wait.
            });

            var makeTea = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Wait for water..");
                evt.Wait();
                Console.WriteLine("Here is your tea");
            });

            makeTea.Wait();


            // Second example - AutoResetEvent

            var evt2 = new AutoResetEvent(false); // Initializing first state.

            Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Boiling water");
                evt2.Set(); // Unblock the wait.
            });

            var makeTea2 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Wait for water..");
                evt2.WaitOne(); // Set it back to false after this line (From here the name - One).
                Console.WriteLine("Here is your tea");
                var ok = evt2.WaitOne(1000); // We'll be stuck, unless we specify timeout.
                if (ok)
                {
                    Console.WriteLine("Enjoy your tea");
                } else
                {
                    // We'll reach here because ev2.Set is called only once
                    Console.WriteLine("No Tea for you"); 
                }
            });

            makeTea.Wait();
        }
    }
}
