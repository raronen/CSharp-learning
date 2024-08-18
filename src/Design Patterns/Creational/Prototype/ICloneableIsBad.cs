namespace Prototype.ICloneableIsBad {

    // Problem: ICloneable is bad because you never know if its SHALLOW copy or DEEP copy
    public class ICloneableIsBad {
        public static void Run() {
            var john = new Person(new []{"John", "Doe"}, new Address {StreetName = "123 London Road", HouseNumber = 123});
            System.Console.WriteLine(john);

            // Now we just want to add another Person.
            var jane = john.Clone() as Person;
            jane.Names[0] = "Jane";
            // Doomed to failre, because jane is a reference to john, and still contains the SAME address
        }
    }
    public class Person: ICloneable {
        public string[] Names;
        private Address Address;
        public Person(string[] names, Address address)
        {
            Names = names;
            Address = address;
        }

        // You never know if its shallow copy or deep copy
        public object Clone()
        {
            // Which implementation? Shallow? Deep? How do we garuantuee it stays the same? And what if we add more fields?
            return null;
        }

        public override string ToString()
        {
            return $"Names: {string.Join(", ", Names)}, Address: {Address.StreetName} {Address.HouseNumber}";
        }
    }

    public class Address {
        public string StreetName;
        public int HouseNumber;
    }
}