using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Reading from couple of thread, but only 1 thread can write.
namespace CSharpLearnings.src.Parallel_programing.ReaderWriterLock
{
    public class Demo
    {
        static ReaderWriterLockSlim padLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion); // Support multiple locks on same thread

        static Random random = new Random();
        public static void Run()
        {
            int x = 0;

            var tasks = new List<Task>();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    // Regular example
                    padLock.EnterReadLock();
                    //padLock.EnterReadLock(); // LockRecursionPolicy.SupportsRecursion enables this, not recommended for most cases


                    Console.WriteLine($"Entered read lock, x = {x}");
                    Thread.Sleep( 5000 );

                    padLock.ExitReadLock();
                    //padLock.ExitReadLock(); // LockRecursionPolicy.SupportsRecursion enables this, not recommended for most cases

                    Console.WriteLine($"Exited read lock, x = {x}.");

                    // Example for upgrade lock: Started with read but want write 
                    padLock.EnterUpgradeableReadLock();

                    if (i%2 == 0)
                    {
                        padLock.EnterWriteLock();
                        x = 123;
                        padLock.ExitWriteLock();
                    }

                    Console.WriteLine($"Entered read lock, x = {x}");
                    Thread.Sleep(5000);
                    padLock.ExitUpgradeableReadLock();
                }));
            }

            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException ae)
            {
                ae.Handle(e =>
                {
                    Console.WriteLine(e);
                    return true;
                });
            }

            while (true)
            {
                Console.ReadKey();
                padLock.EnterWriteLock();
                Console.WriteLine("Write lock aquired");
                int newValue = random.Next(10);
                x = newValue;
                Console.WriteLine($"Set x = {x}");
                padLock.ExitWriteLock();
                Console.WriteLine("Write lock released");
            }

        }
    }
}
