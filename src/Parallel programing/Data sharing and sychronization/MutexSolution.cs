using CSharpNewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.Parallel_programing.MutexSolution
{
    public class BankAccount
    {
        public object padlock = new object();
        public int Balance { get; set; }

        public void Deposit(int amount)
        {
            // += is not Automic!
            // op1: temp <- get_Balance() + amount
            // op2: set_Balance(temp)
            lock (padlock) // lock uses Monitor.Enter
            {
                Balance += amount;
            }
        }

        public void Withdraw(int amount)
        {
            // Same: -= is not automic
            lock (padlock)
            {
                Balance -= amount;
            }
        }

        public void Transfer(BankAccount where, int amount)
        {
            Balance -= amount;
            where.Deposit(amount);
        }
    }


    public class Demo
    {
        public static void Example1()
        {
            var tasks = new List<Task>();
            var ba = new BankAccount();

            Mutex mutex = new Mutex();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        bool haveLock = mutex.WaitOne();
                        try
                        {
                            ba.Deposit(100);
                        }
                        finally
                        {
                            if (haveLock) mutex.ReleaseMutex();
                        }
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        bool haveLock = mutex.WaitOne();
                        try
                        {
                            ba.Withdraw(100);
                        }
                        finally
                        {
                            if (haveLock) mutex.ReleaseMutex();
                        }
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"Final balance is {ba.Balance}");
        }

        public static void Example2()
        {
            var tasks = new List<Task>();
            var ba = new BankAccount();
            var ba2 = new BankAccount();

            Mutex mutex = new Mutex();
            Mutex mutex2 = new Mutex();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        bool haveLock = mutex.WaitOne();
                        try
                        {
                            ba.Deposit(1);
                        }
                        finally
                        {
                            if (haveLock) mutex.ReleaseMutex();
                        }
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        bool haveLock = mutex2.WaitOne();
                        try
                        {
                            ba2.Deposit(1);
                        }
                        finally
                        {
                            if (haveLock) mutex2.ReleaseMutex();
                        }
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for(int j = 0;j < 1000; j++)
                    {
                        // Have to wait for both mutex because both bank accoutns are changing
                        bool haveLock = WaitHandle.WaitAll(new[] {mutex, mutex2});
                        try
                        {
                            ba.Transfer(ba2, 1);
                        }
                        finally
                        {
                            if (haveLock)
                            {
                                mutex.ReleaseMutex();
                                mutex2.ReleaseMutex();
                            }
                        }
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"Final balance for ba is {ba.Balance}");
            Console.WriteLine($"Final balance for ba2 is {ba2.Balance}");
        }

        // Mutex can be shared across several different programs!!!
        // e.g to prevent several copies of a program from beeing run.
        public static void Example3()
        {
            const string appName = "MyApp";
            Mutex mutex;

            try
            {
                mutex = Mutex.OpenExisting(appName);
                Console.WriteLine($"Sorry, {appName} is already running");
            } catch (WaitHandleCannotBeOpenedException)
            {
                Console.WriteLine("We can run the program just fine");
                mutex = new Mutex(false, appName);
            }

            Console.ReadKey();

            mutex.ReleaseMutex();
        }
        public static void Run()
        {
            Example3();
        }
    }
}
