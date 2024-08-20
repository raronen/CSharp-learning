using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.Parallel_programing.Async_Await.AsyncFactoryMethodExample
{
    // How to do async in constructor? - using async factory method
    public class Foo
    {
        private Foo()
        {

        }

        private async Task<Foo> InitAsync()
        {
            await Task.Delay(1000);
            return this;
        }

        public static Task<Foo> CreateAsync()
        {
            var result = new Foo();
            return result.InitAsync();
        }
    }

    public class Demo
    {
        public static async void Run()
        {
            Foo x = await Foo.CreateAsync();
        }
    }
}
