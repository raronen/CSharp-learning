// NET 8 - Now we have default interface members, which allows us to have multiple inheritance in C#.
namespace Decorator.MultipleInheritanceWithDefaultInterfaceMembers {

    public class Demo {
        public static void Run() {
            Dragon d = new Dragon{Age = 5};
            // Option 1:
            ((ILizard)d).Crawl();
            // Options 2:
            if (d is ILizard lizard) {
                lizard.Crawl();
            }
        }
    }

    public interface ICreature {
        int Age { get; set; }

    }

    public interface IBird: ICreature {
        void Fly() {
            if (Age >= 10)
                Console.WriteLine("Flying");
        }
    }

    public class Organism {}

    // Extensions method to add Crawl is better because we keep on OCP.
    public interface ILizard: ICreature {
        void Crawl() {
            if (Age < 10)
                Console.WriteLine("Crawling");
        }
    }

    public class Dragon: Organism, IBird, ILizard {
        public int Age { get; set; }
    }
}