// The Single Responsibility Principle (SRP) states that a class should have only one reason to change, meaning that a class should have only one job.

using System.Diagnostics;

namespace DesignPatterns {
    public class Journal {
        private readonly List<string> entries = new List<string>();

        private static int count = 0;

        public int AddEntry(string text) {
            entries.Add($"{++count}: {text}");
            return count; // memento
        }

        public void RemoveEntery(int index) {
            entries.RemoveAt(index);
        }

        public override string ToString() {
            return string.Join(Environment.NewLine, entries);
        }


        // start - BREAK Single Responsibility principle - Suddenly also manages persistence! 
        public void Save(string fileName) {
            File.WriteAllText(fileName, ToString());
        }

        public static Journal Load(string fileName) {
            return new Journal();
        }

        public void Load(Uri uri) {
            // 
        }
        // end
    }

    // start - FIXES single responsibility principle - now we have Seperation of concerns.
    // Jorunal - concern keeping a bunch of entries
    // Persistence - concern of saving and loading (e.g file, or database)
    public class Persistence {
        public void SaveToFile(Journal journal, string fileName, bool overwrite = false) {
            if (overwrite || !File.Exists(fileName)) {
                File.WriteAllText(fileName, journal.ToString());
            }
        }
    }
    // end
}

/*
using System.Diagnostics;
using DesignPatterns;

var j = new Journal();
j.AddEntry("I cried today.");
j.AddEntry("I ate a bug.");
Console.WriteLine(j);

var p = new Persistence();
var fileName = @"./journal.txt";
p.SaveToFile(j, fileName, true);
var psi = new ProcessStartInfo(fileName)
{
    UseShellExecute = true
};

*/