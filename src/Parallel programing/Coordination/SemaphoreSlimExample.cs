using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.Parallel_programing.Coordination.SemaphoreSlimExample
{
    public class Demo
    {
        public static void Run()
        {
            var semaphore = new SemaphoreSlim(2, 10); // Initial count - 2 requires cuncurrently, second number is max

            for (int i = 0; i < 20; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    Console.WriteLine($"Entering task ${Task.CurrentId}");
                    semaphore.Wait(); // ReleaseCount--
                    Console.WriteLine($"Processing task ${Task.CurrentId}");
                });
            }

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Console.WriteLine($"Semaphore count: {semaphore.CurrentCount}");
                    Thread.Sleep(2000);
                }
            });

            while (semaphore.CurrentCount <= 2)
            {
                Console.WriteLine($"Semaphore count: {semaphore.CurrentCount}");
                Console.ReadKey();
                semaphore.Release(2);
            }
        }
    }
}
