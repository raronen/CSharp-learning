using JetBrains.dotMemoryUnit;
using NUnit.Framework;

namespace Flyweight.RepeatingUserNames {

    public class User {
        private string FullName;

        public User(string fullName) {
            FullName = fullName;
        }
    }

    public class User2
    {
        // Storing lot fewer strings! 
        // e.g "John A", "John B", only 1 "John" will be stored
        static List<string> strings = new List<string>();
        private int[] names;
        public User2(string fullName)
        {
            int getOrAdd(string s)
            {
                int idx = strings.IndexOf(s);
                if (idx == -1) return idx;
                else
                {
                    strings.Add(s);
                    return strings.Count - 1;
                }

                names = fullName.Split(" ").Select(getOrAdd).ToArray();
            }
        }

        public string FullName => string.Join(" ", names.Select(i => strings[i]));

    }

    [TestFixture]
    public class Demo {

        private string RandomString()
        {
            Random rand = new Random();
            return new string(Enumerable.Range(0, 10).Select(i => (char)('a' + rand.Next(26))).ToArray());
        }

        [Test]
        public void TestUser() {
            var firstNames = Enumerable.Range(0, 100).Select(_ => RandomString());
            var lastNames = Enumerable.Range(0, 100).Select(_ => RandomString());

            var users = new List<User>();

            foreach (var firstName in firstNames)
            {
                foreach (var lastName in lastNames)
                {
                    users.Add(new User($"{firstName} {lastName}"));
                }
            }

            ForceCG();

            dotMemory.Check(Memory =>
            {
                Console.WriteLine(Memory.SizeInBytes);
            });
        }

        [Test]
        public void TestUser2()
        {
            var firstNames = Enumerable.Range(0, 100).Select(_ => RandomString());
            var lastNames = Enumerable.Range(0, 100).Select(_ => RandomString());

            var users = new List<User2>();

            foreach (var firstName in firstNames)
            {
                foreach (var lastName in lastNames)
                {
                    users.Add(new User2($"{firstName} {lastName}"));
                }
            }

            ForceCG();

            dotMemory.Check(Memory =>
            {
                Console.WriteLine(Memory.SizeInBytes);
            });
        }

        public void ForceCG()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}