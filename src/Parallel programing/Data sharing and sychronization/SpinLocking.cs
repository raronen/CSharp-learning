using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.Parallel_programing.SpinLocking
{
    public class BankAccount
    {
        public int Balanace { get; set; }

        public void Deposit(int amount)
        {
            Balanace += amount;
        }

        public void Withdraw(int amount)
        {
            Balanace -= amount;
        }
    }

    public class Demo
    {
        public static void FirstExample()
        {
            var tasks = new List<Task>();
            var ba = new BankAccount();

            SpinLock sl = new SpinLock();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        bool lockTaken = false;
                        try
                        {
                            sl.Enter(ref lockTaken); // lock
                            ba.Deposit(100);
                        }
                        finally
                        {
                            if (lockTaken) sl.Exit(); // release
                        }
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        bool lockTaken = false;
                        try
                        {
                            sl.Enter(ref lockTaken); // lock
                            ba.Withdraw(100);
                        }
                        finally
                        {
                            if (lockTaken) sl.Exit(); // release
                        }
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"Final balance is {ba.Balanace}");
        }

        static SpinLock sl = new SpinLock(true); // will throw exception, and not cause DEAD lock. otherwise the same thread will lock TWICE and cause deadlock.
        
        public static void LockRecursive(int x)
        {
            bool lockTaken = false;

            try
            {
                sl.TryEnter(ref lockTaken); // We'll call this a second time, and SpinLock doesn't support Lock recurision -> cause exception. Solution: Mutex.
            }
            catch (LockRecursionException ex)
            {
                Console.WriteLine("Exception" + ex);
            }
            finally
            {
                if (lockTaken)
                {
                    Console.WriteLine($"Took a lock, x = {x}");
                    LockRecursive(x - 1);
                     sl.Exit();
                }
                else
                {
                    Console.WriteLine($"Failed to take a lock x = {x}");
                }
            }
        }
        public static void SecondExample()
        {
            LockRecursive(5);
        }
        public static void Run()
        {
            SecondExample();
        }
    }
}
