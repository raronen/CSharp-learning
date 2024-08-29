using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace CSharpLearnings.src.Parallel_programing.ChannelExample
{

    class Worker
    {
        Channel<int> channel = Channel.CreateUnbounded<int>();
        CancellationTokenSource cts = new CancellationTokenSource();


        public void Produce()
        {
            Task.Run(async () =>
            {
                var i = 0;
                while (true)
                {
                    await channel.Writer.WriteAsync(i);
                    Console.WriteLine($"Wrote {++i} to channel.");
                }
            });
        }

        public async Task Consume()
        {
            await foreach (var item in channel.Reader.ReadAllAsync(cts.Token))
            {
                Console.WriteLine(item);
            }
        }

        // Maybe consumer wants to have its own cancellation token? We can expose it via WithCancellation
        public IAsyncEnumerable<int> GetAsyncEnumerable() => channel.Reader.ReadAllAsync(cts.Token);
    }

    public class ChannelExample
    {
        public static async Task Demo()
        {
            var worker = new Worker();
            worker.Produce();

            var cts = new CancellationTokenSource(10000);
            await Consume(worker.GetAsyncEnumerable(), cts.Token);
        }

        async static Task Consume(IAsyncEnumerable<int> enumerable, CancellationToken cancellationToken)
        {
            await foreach (var item in enumerable.WithCancellation(cancellationToken))
            {
                Console.WriteLine($"Consuming value {item}.");
            }
        }
    }

}
