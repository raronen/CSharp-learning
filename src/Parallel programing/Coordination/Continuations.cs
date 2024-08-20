using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.Parallel_programing.Coordination.Continuations
{
    // Getting multiple tasks to execute in a praticular order
    public class Demo
    {
        public static void Run()
        {
            // Simple example
            var task = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Boilling Water");
            });

            var task2 = task.ContinueWith(t =>
            {
                Console.WriteLine($"Completed task {t.Id}, pour water into cap.");
            });

            task2.Wait();



            // Second example
            var t = Task.Factory.StartNew(() => "Task 1");
            var t2 = Task.Factory.StartNew(() => "Task 2");

            // 1 -> many relation
            var t3 = Task.Factory.ContinueWhenAll(new[] {t, t2}, // Also have ContinueWhenAny
                tasks =>
                {
                    Console.WriteLine("Tasks completed:");
                    foreach (var t in tasks)
                    {
                        Console.WriteLine($" - {t.Result}");
                    }
                }

                ); 
            
            t3.Wait();
        }
    }
}
