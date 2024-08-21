using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.Parallel_programing.Async_Await.AsyncLazyInit
{
    public class Stuff
    {
        private static int value;

        private readonly Lazy<Task<int>> AutoIncValue =
            new Lazy<Task<int>>(async () => 
            {
                await Task.Delay(1000).ConfigureAwait(false); // <-- not sure if it'll run on ui thread or not..

                return value++;
            });


        private readonly Lazy<Task<int>> AutoIncValue2 =
            new Lazy<Task<int>>(() => Task.Run(async() => // <-- always will be use on the thread pool thread
            {
                await Task.Delay(1000);

                return value++;
            }));

        // Nito.AsynceX
       /*
        private AsyncLazy<int> AutoIncValue3 =
            new AsyncLazy<int>(async () =>
            {
                await Task.Delay(1000);
                return value++;
            });
       */
        public async void UseValue()
        {
            int value = await AutoIncValue.Value;
        }
    }
    public class Demo
    {
        public static void Run()
        {

        }
    }
}
