using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.Parallel_programing.Data_sharing_and_sychronization
{
    public class BankAccount
    {
        public object padlock = new object();
        public int Balanace { get; set; }

        public void Deposit(int amount)
        {
            // += is not Automic!
            // op1: temp <- get_Balance() + amount
            // op2: set_Balance(temp)
            lock (padlock) // lock uses Monitor.Enter
            {
                Balanace += amount;
            }
        }

        public void Withdraw(int amount)
        {
            // Same: -= is not automic
            lock (padlock)
            {
                Balanace -= amount;
            }
        }
    }

    public class Demo { 
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

            Console.WriteLine($"Final balance is {ba.Balanace}");
        }
    }
}
