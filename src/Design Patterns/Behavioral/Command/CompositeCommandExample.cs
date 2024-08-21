using BenchmarkDotNet.Portability;
using Microsoft.Diagnostics.Tracing.Parsers.JScript;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace CSharpLearnings.src.Design_Patterns.Structural.Command.CompositeCommandExample
{
    public class BankAccount
    {
        public int balance;
        public int overdraftLimit = -500;

        // Make internal so others won't be able to access internal implementation - SOLID - D
        public void Deposit(int amount)
        {
            balance += amount;
            WriteLine($"Deposited ${amount}, balanc is now {balance}");
        }

        public bool Withdraw(int amount)
        {
            if (balance - amount >= overdraftLimit)
            {
                balance -= amount;
                WriteLine($"Withdraw ${amount}, balance is now {balance}");
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"Bank account: {balance}";
        }
    }

    public interface ICommand
    {
        void Call();
        void Undo();
        bool Success { get; set; }
    }

    public class BankAccountCommand : ICommand
    {
        private BankAccount account;
        public enum Action
        {
            Deposit, Withdraw
        };
        private Action action;
        private int amount;
        private bool successded = true;

        public virtual bool Success { get => successded; set => successded = value; }

        public BankAccountCommand(BankAccount account, Action action, int amount)
        {
            this.account = account;
            this.action = action;
            this.amount = amount;
        }

        public override string ToString()
        {
            return $"Bank Account: Balance = {account.balance}, Overdraft Limit = {account.overdraftLimit}";
        }

        public virtual void Call()
        {
            switch (action)
            {
                case Action.Deposit:
                    account.Deposit(amount);
                    break;
                case Action.Withdraw:
                    successded = account.Withdraw(amount);
                    break;
                default:
                    break;
            }
        }

        public virtual void Undo()
        {
            if (!successded) return;
            switch (action)
            {
                case Action.Withdraw:
                    account.Deposit(amount);
                    break;
                case Action.Deposit:
                    account.Withdraw(amount);
                    break;
                default:
                    break;
            }
        }
    }

    public class MoneyTransferCommand: CompositeBankAccountCommand {
        public MoneyTransferCommand(BankAccount from, BankAccount to, int amount) {
            AddRange(new[]
            {
                new BankAccountCommand(from, BankAccountCommand.Action.Withdraw, amount),
                new BankAccountCommand(to, BankAccountCommand.Action.Deposit, amount),
            });
        }

        public override void Call()
        {
            BankAccountCommand last = null;
            foreach( var cmd in this)
            {
                if (last == null || last.Success)
                {
                    cmd.Call();
                    last = cmd;
                } else
                {
                    cmd.Undo();
                    break;
                }
            }
        }
    }


    public class CompositeBankAccountCommand
        : List<BankAccountCommand>, ICommand
    {
        // Helps with inheritance
        public CompositeBankAccountCommand() { }

        public CompositeBankAccountCommand(
            IEnumerable<BankAccountCommand> commands ): base( commands ) { }
        public virtual bool Success
        {
            get { return this.All(cmd => cmd.Success); }
            set
            {
                foreach (var cmd in this) { cmd.Success = value; }
            }
        }

        public virtual void Call()
        {
            ForEach(cmd => cmd.Call());
        }

        public virtual void Undo()
        {
            foreach( var cmd in ((IEnumerable<BankAccountCommand>) this).Reverse())
            {
                if (cmd.Success) cmd.Undo();
            }
        }
    }

    public class Demo
    {
        public static void Run()
        {
            var from = new BankAccount();
            var to = new BankAccount();
            var deposit = new BankAccountCommand(from, BankAccountCommand.Action.Deposit, 1000);

            var mtc = new MoneyTransferCommand(from, to, 1000);


            mtc.Call();
            Console.WriteLine(from);
            Console.WriteLine(to);

            mtc.Undo();

            Console.WriteLine(from);
            Console.WriteLine(to);

        }
    }
}
