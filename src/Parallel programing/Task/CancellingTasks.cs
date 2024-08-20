using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.Parallel_programing.CancellingTask
{
    public class Demo { 
        public static void Run()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            token.Register(() =>
            {
                Console.WriteLine("Cancel has been requested"); // <--- 1st option to call callback after cancellation.
            });

            Task.Factory.StartNew(() =>
            {
                token.WaitHandle.WaitOne(); // <--- 2st option to call callback after cancellation.
                Console.WriteLine("WaitHandle released - Cancel has been requested"); 
            });

            var t = new Task(() =>
            {
                int i = 0;
                while (true)
                {
                    if (token.IsCancellationRequested)
                    {
                        //break; <-- 1st option to cancel task (task will be completed, cause no exception)
                        //throw new OperationCanceledException(); // <-- 2nd option

                    } else
                    {
                        Console.WriteLine(i++ + "\t");
                    }

                    token.ThrowIfCancellationRequested(); // <-- 3rd option - Can also do:
                }
            }, token);

            t.Start();

            Console.ReadKey();
            cts.Cancel();

            Console.WriteLine("Main program done.");
            Console.ReadKey();
        }

        // Linked token source
        public static void Run2()
        {
            var planned = new CancellationTokenSource();
            var preventative = new CancellationTokenSource();
            var emergency = new CancellationTokenSource();

            var paranoid = CancellationTokenSource.CreateLinkedTokenSource(
                planned.Token, preventative.Token, emergency.Token
                );

            Task.Factory.StartNew(() => {
                int i = 0;
                while (true) {
                    paranoid.Token.ThrowIfCancellationRequested();
                    Console.WriteLine($"{i++}\t");
                    Thread.Sleep( 1000 );
                 }
            }, paranoid.Token);

            Console.ReadKey();

            emergency.Cancel();

            Console.WriteLine("Main program done.");
            Console.ReadKey();
        }
    }

}
