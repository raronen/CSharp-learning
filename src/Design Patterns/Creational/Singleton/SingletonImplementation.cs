using MoreLinq;

namespace Singeleton.Implementation {

    public class SingletonImplementation {
        public static void Run() {
            var db = SingletonDatabase.Instance;

            Console.WriteLine(db.GetPopulation("Tokyo"));
        }
    }
    public interface IDatabase {
        int GetPopulation(string name);
    }


    public class SingletonDatabase : IDatabase {
        private Dictionary<string, int> capitals;

        private SingletonDatabase() {
            Console.WriteLine("Initializing database");

            capitals = File.ReadAllLines("../../../src/Design Patterns/Creational/Singleton/cities.txt")
                .Batch(2)
                .ToDictionary(
                    list => list.ElementAt(0).Trim(),
                    list => int.Parse(list.ElementAt(1))
                );
        }

        private static Lazy<SingletonDatabase> instance = new Lazy<SingletonDatabase>(() => new SingletonDatabase());

        public static SingletonDatabase Instance => instance.Value;

        public int GetPopulation(string name)
        {
            return capitals[name];
        }
    }
}