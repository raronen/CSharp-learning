using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.Parallel_programing.Async_Await.AsyncInitPattern
{
    public interface IAsyncInit
    {
        Task InitTask { get; }
    }

    public class MyClass : IAsyncInit
    {
        public MyClass()
        {
            InitTask = InitAsync();
        }

        public Task InitTask { get; }

        private async Task InitAsync()
        {
            await Task.Delay(1000);
        }
    }

    public class MyOtherClass : IAsyncInit
    {
        private readonly MyClass myClass;
        public MyOtherClass(MyClass myClass)
        {
            this.myClass = myClass;
            InitTask = InitAsync();
        }

        public Task InitTask { get; }

        private async Task InitAsync()
        {
            if (myClass is IAsyncInit ai)
            {
                ai.InitTask.Start();
            }
            await Task.Delay(1000);
        }
    }

    public class Demo
    {
        public static async void Run()
        {
            var myClass = new MyClass();

            var oc = new MyOtherClass(myClass);

            await oc.InitTask;
        }
    }
}
