namespace Decorator.MultipleInheritance {
    // Multiple inheritance is ugly, look at Weight property which we need to keep managing for both base classes: Bird and Lizard.
    public class Demo {
        public static void Run() {
            var d = new Dragon();
            d.Weight = 123;
            d.Fly();
            d.Crawl();

            Console.Write(d.Weight); // Which Weight will this print?
        }
    }

    public interface IBird {
        int Weight { get; set; } // Weight!
        void Fly();
    }
    public class Bird: IBird {
        public int Weight { get; set; }
        public void Fly() {
            Console.WriteLine($"Soaring in the sky with weight {Weight}");
        }
        
    }

    public interface ILizard {
        int Weight { get; set; } // Another weight!
        void Crawl();
    }

    public class Lizard: ILizard {
        public int Weight { get; set; }
        public void Crawl() {
            Console.WriteLine($"Crawling in the dirt with weight {Weight}");
        }
    }

    public class Dragon : IBird, ILizard {
        private Bird bird = new Bird();
        private Lizard lizard = new Lizard();

        public void Crawl() {
            lizard.Crawl();
        }

        public void Fly() {
            bird.Fly();
        }

        public int Weight { // So we adjust the weight of BOTH bird and lizard
            get => bird.Weight;
            set {
                bird.Weight = value;
                lizard.Weight = value;
            }
        }
    }
}