namespace Singleton.PerThreadSingleton {

    public class Demo {
        public static void Run() {
            var t1 = Task.Factory.StartNew(() => {
                Console.WriteLine("t1: " + PerThreadSingleton.Instance.Id);
            });
            var t2 = Task.Factory.StartNew(() => {
                Console.WriteLine("t2: " + PerThreadSingleton.Instance.Id);
                Console.WriteLine("t2: " + PerThreadSingleton.Instance.Id);
            });

            Task.WaitAll([t1, t2]);
        }
    }

    public sealed class PerThreadSingleton {
        private static ThreadLocal<PerThreadSingleton> threadInstance = new ThreadLocal<PerThreadSingleton>(() => new PerThreadSingleton()); // ThreadLocal<T> is a class that provides thread-local storage of data.

        public int Id;
        private PerThreadSingleton() {
            Id = Thread.CurrentThread.ManagedThreadId;
        }

        public static PerThreadSingleton Instance => threadInstance.Value;
    }
}