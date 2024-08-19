using Autofac;
using MoreLinq;
using NUnit.Framework;

namespace Singleton.Autofac {
    public interface IDatabase {
        int GetPopulation(string name);
    }

    [TestFixture]
    public class SingletonTests {
        [Test]
        public void DIPuplationTest() {
            var cb = new ContainerBuilder(); // <-- Autofac
                cb.RegisterType<OrdanaryDatabase>()
                // cb.RegisterType<DummayDatabase>() // Or the Dummy!!
                .As<IDatabase>()
                .SingleInstance(); // <-- Singleton
            cb.RegisterType<ConfigurableRecordFinder>();
            using(var c = cb.Build()) {
                var crf = c.Resolve<ConfigurableRecordFinder>();
                crf.GetTotalPopulation(new []{"alpha", "beta"});
            }
        }

    }


    public class OrdanaryDatabase : IDatabase {
        private Dictionary<string, int> capitals;

        private OrdanaryDatabase() {
            Console.WriteLine("Initializing database");

            capitals = File.ReadAllLines(new FileInfo(typeof(IDatabase).Assembly.Location).DirectoryName + "../../../src/Design Patterns/Creational/Singleton/cities.txt")
                .Batch(2)
                .ToDictionary(
                    list => list.ElementAt(0).Trim(),
                    list => int.Parse(list.ElementAt(1))
                );
        }

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