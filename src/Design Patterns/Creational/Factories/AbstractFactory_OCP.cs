// We do it using reflection.

namespace Factories.AbstractFactory_OCP {

    public class AbstractFactory_OCP {
        public static void Run() {
            var machine = new HotDrinkMachine();
            var drink = machine.MakeDrink();
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
            Console.WriteLine($"Grind some beans, boil water, pour {amount} ml, add cream and sugar, enjoy!");
            return new Coffe();
        }
    }

    public class HotDrinkMachine {

        // Breaks Open-Closed Principle - What if we want to add a new drink?
        // public enum AvailableDrink {
        //     Coffe, Tea
        // }

        // Here abstract comes into play

        // public HotDrinkMachine() {
        //     foreach (var drink in Enum.GetValues(typeof(AvailableDrink))) {
        //         var factory = (IHotDrinkFactory) Activator.CreateInstance(
        //             Type.GetType("Factories.AbstractFactory." + Enum.GetName(typeof(AvailableDrink), drink) + "Factory")
        //         );

        //         factories.Add((AvailableDrink) drink, factory);
        //     }
        // }

        // public IHotDrink MakeDrink(AvailableDrink drink, int amount) {
        //     return factories[drink].Prepare(amount);
        // }

        private List<Tuple<string, IHotDrinkFactory>> factories = new List<Tuple<string, IHotDrinkFactory>>();
        public HotDrinkMachine() {
            foreach (var t in typeof(HotDrinkMachine).Assembly.GetTypes())
            {
                if (typeof(IHotDrinkFactory).IsAssignableFrom(t) && !t.IsInterface) {
                    factories.Add(Tuple.Create(
                        t.Name.Replace("Factory", string.Empty),
                        (IHotDrinkFactory) Activator.CreateInstance(t) as IHotDrinkFactory
                    ));
                }
            }
        }

        public IHotDrink MakeDrink() {
            Console.WriteLine("Available drinks:");
            for (int i = 0; i < factories.Count; i++)
            {
                var tuple = factories[i];
                Console.WriteLine($"{i}: {tuple.Item1}");
            };

            while (true) {
                string s;
                if ((s = Console.ReadLine()) != null 
                    && int.TryParse(s, out var i)
                    && i >= 0
                    && i < factories.Count) {

                    Console.WriteLine("Specify amount:");
                    s = Console.ReadLine();

                    if (s != null & int.TryParse(s, out var amount) && amount > 0) {
                        return factories[i].Item2.Prepare(amount);
                    }

                }
            }
        }
    }

}