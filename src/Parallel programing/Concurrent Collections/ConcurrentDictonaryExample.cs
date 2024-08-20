using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.Parallel_programing.Concurrent_Collections.ConcurrentDictonaryExample
{
    public class Demo
    {
        private static ConcurrentDictionary<string, string> capitals = new ConcurrentDictionary<string, string>();

        public static void AddParis()
        {
            // There is no simple Add method - key can be already be in the dictonary.
            bool success = capitals.TryAdd("France", "Paris");
            string who = Task.CurrentId.HasValue ? ("Task" + Task.CurrentId.Value) : "Main thread";
            Console.WriteLine($"{who} {success}");
        }
        public static void Run()
        {
            Task.Factory.StartNew(AddParis).Wait();
            AddParis();

            capitals["Russia"] = "Leningrad";
            capitals["Russia"] = "Moscow";     // Override.

            // Can also do:
            capitals["Russia"] = "Leningrad";
            capitals.AddOrUpdate("Russia", /*if not exists, put: */"Moscow",
                /* if already exists, you can do something with the old param, or alter the insertive value */
                (k, old) => old + " ---> Moscow");

            Console.WriteLine(capitals["Russia"]); // "Leningrad ---> Moscow"

            //capitals["Sweden"] = "Uppsala";
            var capOfSweden = capitals.GetOrAdd("Sweden", "Stockholm");

            Console.WriteLine(capitals["Sweden"]); // "Stockholm"

            const string toRemove = "Russia";
            string removed;
            var didRemoved = capitals.TryRemove(toRemove, out removed);
            if (didRemoved)
            {
                Console.WriteLine($"Removed {removed}");
            }

            // EXPENSIVE OPERATION
            Console.WriteLine(capitals.Count);

            foreach ( var c in capitals ) { 
                Console.WriteLine($"{c.Key}: {c.Value}");
            }
        }
    }
}
