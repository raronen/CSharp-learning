// Why do we need Abstract Factory?
// If we don't want to return the type, but an abstract of it, for encapsulation for example

// Example: Here all user can do is to consume the drink.

namespace Factories.AbstractFactory {

    public class AbstractFactory {
        public static void Run() {
            var machine = new HotDrinkMachine();
            var drink = machine.MakeDrink(HotDrinkMachine.AvailableDrink.Coffe, 100);
            drink.Consume();
        }
    }

    public interface IHotDrink {
        void Consume();
    }

    internal class Tea: IHotDrink {
        public void Consume() {
            Console.WriteLine("This tea is nice but I'd prefer it with milk.");
        }
    }

    internal class Coffe: IHotDrink {
        public void Consume() {
            Console.WriteLine("This coffee is sensational!");
        }
    }

    public interface IHotDrinkFactory {
        IHotDrink Prepare(int amount);
    }

    internal class TeaFactory: IHotDrinkFactory {
        public IHotDrink Prepare(int amount) {
            Console.WriteLine($"Put in tea bag, boil water, pour {amount} ml, add lemon, enjoy!");
            return new Tea();
        }
    }

    internal class CoffeFactory: IHotDrinkFactory {
        public IHotDrink Prepare(int amount) {
            Console.WriteLine("Grind some beans, boil water, pour {amount} ml, add cream and sugar, enjoy!");
            return new Coffe();
        }
    }

    public class HotDrinkMachine {

        // Breaks Open-Closed Principle - What if we want to add a new drink?
        public enum AvailableDrink {
            Coffe, Tea
        }

        // Here abstract comes into play
        private Dictionary<AvailableDrink, IHotDrinkFactory> factories = new Dictionary<AvailableDrink, IHotDrinkFactory>();

        public HotDrinkMachine() {
            foreach (var drink in Enum.GetValues(typeof(AvailableDrink))) {
                var factory = (IHotDrinkFactory) Activator.CreateInstance(
                    Type.GetType("Factories.AbstractFactory." + Enum.GetName(typeof(AvailableDrink), drink) + "Factory")
                );

                factories.Add((AvailableDrink) drink, factory);
            }
        }

        public IHotDrink MakeDrink(AvailableDrink drink, int amount) {
            return factories[drink].Prepare(amount);
        }
    }
}