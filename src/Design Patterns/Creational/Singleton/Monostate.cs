namespace Singleton.Monostate {
    public class CEO {
        // Monostate - Properties are static but the object is not, and also getters not
        private static string name;
        private static int age;

        public string Name {
            get => name;
            set => name = value;
        }

        public int Age {
            get => age;
            set => age = value;
        }

        public override string ToString()
        {
            return $"Name: {Name}, Age: {Age}";
        }
    }

    public class Monostate {
        public static void Run() {
            var ceo1 = new CEO();
            ceo1.Name = "Adam Smith";
            ceo1.Age = 55;

            var ceo2 = new CEO();
            Console.WriteLine(ceo2.Name);
            Console.WriteLine(ceo2.Age);
        }
    }
}