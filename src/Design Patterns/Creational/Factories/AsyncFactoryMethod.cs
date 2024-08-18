namespace Factories.AsyncFactoryMethod {
    public class AsyncFactoryMethod {
        public static async void Run() {
            // Problem:
            var foo = new Foo(); // 1. Needs to be initialized
            foo.InitAsync().GetAwaiter().GetResult(); // 2. Need to remember to call InitAsync

            // After:
            var foov2 = FooV2.CreateAsync();
        }
    }

    // Problem:
    public class Foo {
        public Foo() { // constructor can't be async
        }

        public  async Task<Foo> InitAsync() {
            await Task.Delay(1000);
            return this;
        }
    }

    // Solution:
    public class FooV2 {
        private FooV2() { // private constructor

        } 

        private async Task<FooV2> InitAsync() {
            await Task.Delay(1000);
            return this;
        }

        public static Task<FooV2> CreateAsync() {
            var result = new FooV2();
            return result.InitAsync();
        }
    }
}