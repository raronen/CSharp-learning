using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.Parallel_programing.Coordination.ChildTaskExample
{
    public class Demo
    {
        public static void Run()
        {
            var parent = new Task(() =>
            {
                // detached - by default, parent not waiting for it. Need to use "AttachedToParent"
                var child = new Task(() =>
                {
                    Console.WriteLine("Child task starting");
                    Thread.Sleep(3000);
                    Console.WriteLine("Child task Finishing");
                    throw new Exception(); // <-- will go to failHandler
                }, TaskCreationOptions.AttachedToParent);

                var completionHandler = child.ContinueWith(t =>
                {
                    Console.WriteLine($"Horray, task {t.Id}'s state is {t.Status}");
                }, TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.OnlyOnRanToCompletion);

                var failHandler = child.ContinueWith(t =>
                {
                    Console.WriteLine($"Opps, task {t.Id}'s state is {t.Status}");
                }, TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.OnlyOnFaulted);

                child.Start();
            }, TaskCreationOptions.AttachedToParent);

            parent.Start();

            try
            {
                parent.Wait();
            }
            catch (AggregateException ex)
            {
                ex.Handle(e => true);
            }
        }
    }
}
