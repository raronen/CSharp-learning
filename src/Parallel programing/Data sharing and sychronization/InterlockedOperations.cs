using CSharpLearnings.src.Parallel_programing.Data_sharing_and_sychronization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.Parallel_programing.InterlockedOperations
{
    public class BankAccount {
        private int balance;

        public int Balance { get => balance; set => balance = value; }

        public void Deposit(int amount)
        {
            // += is not Automic!
            // op1: temp <- get_Balance() + amount
            // op2: set_Balance(temp)
            Interlocked.Add(ref balance, amount);

            // More Interlocked stuff:
            // 1
            // 2
            //Interlocked.MemoryBarrier(); // <-- 
            // 3
            
            // CPU can execute 1,2,3 but can also do 3,2,1.. But not with MemoryBarrier!! It ensures 3 will be executed after 1 and 2.
            
            
            // Also, more operators.
            //Interlocked.Exchange(ref balance, amount); // and more operatiors...
        }

        public void Withdraw(int amount)
        {
            // Same: -= is not automic
            Interlocked.Add(ref balance, -amount);
        }
    }

    public class Demo
    {
        public static void Run()
        {
            var tasks = new List<Task>();
            var ba = new BankAccount();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        ba.Deposit(100);
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        ba.Withdraw(100);
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"Final balance is {ba.Balance}");
        }
    }
}
