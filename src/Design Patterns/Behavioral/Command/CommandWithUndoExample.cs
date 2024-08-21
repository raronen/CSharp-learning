using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace CSharpLearnings.src.Design_Patterns.Structural.Command.CommandExample
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
    }

    public class BankAccountCommand: ICommand
    {
        private BankAccount account;
        public enum Action
        {
            Deposit, Withdraw
        };
        private Action action;
        private int amount;
        private bool successded = true;

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

        public void Call()
        {
            switch (action) {
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

        public void Undo()
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

    public class Demo {
        public static void Run() {
            var ba = new BankAccount();
            var commands = new List<BankAccountCommand> {
                new BankAccountCommand(ba, BankAccountCommand.Action.Deposit, 100),
                new BankAccountCommand(ba, BankAccountCommand.Action.Withdraw, 1000) // <- Operation failed, but when undo we assumed it's successeded. So we add flag is successeded.
            };

            WriteLine(ba);

            foreach (var c in commands) {
                c.Call();
            }

            WriteLine(ba);

            foreach (var command in Enumerable.Reverse(commands))
            {
                command.Undo();
                WriteLine(ba);
            }
        }
    }
}
