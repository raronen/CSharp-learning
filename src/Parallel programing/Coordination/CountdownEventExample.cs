using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.Parallel_programing.Coordination.CountdownEventExample
{
    // Difference between Barrier and CountdownEvent is that Signal and Await are
    // SEPERATE methods in CountdownEvent
    // When Countdown reach 0, any waits will be Released

    public class Demo
    {
        private static int taskCount = 5;
        static CountdownEvent cte = new CountdownEvent(taskCount);
        static Random random = new Random();
        public static void Run()
        {
            for (int i = 0; i < taskCount; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    Console.WriteLine($"Entering task {Task.CurrentId}");
                    Thread.Sleep(random.Next(3000));
                    cte.Signal();
                    Console.WriteLine($"Exiting task {Task.CurrentId}");
                });
            }

            var finalTask = Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"Waiting for other tasks to compelete in {Task.CurrentId}");
                cte.Wait(); // Blocking - until count down reaches zero.
                Console.WriteLine("All tasks completed");
            });

            finalTask.Wait();
        }
    }
}
