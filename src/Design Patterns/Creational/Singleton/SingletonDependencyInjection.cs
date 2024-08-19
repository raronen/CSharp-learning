using MoreLinq;
using NUnit.Framework;

// TESTING ON A LIVE DATABASE - What happens if the database is down? or data is changed?
// The issue is that SingletonRecordFinder has a HARDCODED dependency on SingletonDatabase
// Solution -> Dependency Injection - ConfigurableRecordFinder class below
namespace Singeleton.DependencyInjection {
    public class SingletonImplementation {
        public static void Run() {
            var db = SingletonDatabase.Instance;

            Console.WriteLine(db.GetPopulation("Tokyo"));
        }
    }
    public interface IDatabase {
        int GetPopulation(string name);
    }

    [TestFixture]
    public class SingletonTests {
        [Test]
        public void IsSingletonTest() {
            var db = SingletonDatabase.Instance;
            var db2 = SingletonDatabase.Instance;
            Assert.That(db, Is.SameAs(db2));
            Assert.That(SingletonDatabase.Count, Is.EqualTo(1));
        }

        [Test]
        public void SingletonTotalPopulationTest() {
            var rf = new ConfigurableRecordFinder(new DummayDatabase()); // <-- Dependency Injection
            var names = new []{ "alpha", "beta", "gamma"};

            int tp = rf.GetTotalPopulation(names);
            // TESTING ON A LIVE DATABASE - What happens if the database is down? or data is changed?
            // The issue is that SingletonRecordFinder has a HARDCODED dependency on SingletonDatabase
            Assert.That(tp, Is.EqualTo(1 + 2 + 3));
        }

    }


    public class SingletonDatabase : IDatabase {
        private Dictionary<string, int> capitals;
        private static int instanceCount; // 0 by default
        public static int Count => instanceCount;

        private SingletonDatabase() {
            Console.WriteLine("Initializing database");
            instanceCount++;

            capitals = File.ReadAllLines(new FileInfo(typeof(IDatabase).Assembly.Location).DirectoryName + "../../../src/Design Patterns/Creational/Singleton/cities.txt")
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


    // New implementation
    public class ConfigurableRecordFinder {
        private IDatabase database;
        public ConfigurableRecordFinder(IDatabase database) {
            this.database = database ?? throw new ArgumentNullException(paramName: nameof(database));
        }

        public int GetTotalPopulation(IEnumerable<string> names) {
            int result = 0;
            foreach(var name in names) {
                result += database.GetPopulation(name);
            }

            return result;
        }
    }

    public class DummayDatabase: IDatabase {
        public int GetPopulation(string name) {
            return new Dictionary<string, int> {
                ["alpha"] = 1,
                ["beta"] = 2,
                ["gamma"] = 3
            }[name];
        }
    }
}