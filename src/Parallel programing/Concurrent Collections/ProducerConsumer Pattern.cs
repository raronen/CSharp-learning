using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace CSharpLearnings.src.Parallel_programing.Concurrent_Collections.ProducerConsumer_Pattern
{
    // Pattern: have collection, keep adding items from many thread. And consuming items from other threads.
    // BlockingCollection is a wrapper collection
    public class Demo
    {
        static BlockingCollection<int> messages = new BlockingCollection<int>(
            new ConcurrentBag<int>(),
            10 // how many elements we can max have - afterward - the collection will block. And stall who ever trying to add.
        );

        static CancellationTokenSource _cts = new CancellationTokenSource();
        static Random random = new Random();

        static void ProduceAndConsume()
        {
            var producer = Task.Factory.StartNew(RunProducer);
            var consumer = Task.Factory.StartNew(RunConsumer);

            try
            {
                Task.WaitAll(new[] { producer, consumer }, _cts.Token);
            }
            catch (AggregateException ex)
            {
                ex.Handle(e => true);
            }
        }
        public static void Run()
        {
           Task.Factory.StartNew(ProduceAndConsume, _cts.Token);

            Console.ReadKey();

            _cts.Cancel();
        }

        private static void RunConsumer()
        {
            foreach (var item in messages.GetConsumingEnumerable())
            {
                _cts.Token.ThrowIfCancellationRequested();
                Console.WriteLine($"-{item}\t");
                Thread.Sleep(random.Next(100));
            }
        }

        private static void RunProducer()
        {
            while (true)
            {
                _cts.Token.ThrowIfCancellationRequested();
                int i = random.Next(100);
                messages.Add(i); // Block here if there are more than 10 in messages.
                Console.WriteLine($"+{i}\t");
                Thread.Sleep(random.Next(1000));
            }
        }
    }
}
