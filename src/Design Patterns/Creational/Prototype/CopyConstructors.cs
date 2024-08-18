namespace Prototype.CopyConstructors {
    public class CopyConstructors {
        public static void Run() {
            var john = new Person(new []{"John", "Doe"}, new Address {StreetName = "123 London Road", HouseNumber = 123});
            System.Console.WriteLine(john);

            var jane = new Person(john);
            jane.Names[0] = "Jane";
            System.Console.WriteLine(jane);
        }
    }

     public class Person {
        public string[] Names;
        private Address Address;
        
        // Here! Copy constructor
        public Person(Person other)
        {
            Names = other.Names;
            Address = new Address(other.Address); // Deep copy - we create a new object
        }

        public Person(string[] names, Address address)
        {
            Names = names;
            Address = address;
        }

        public override string ToString()
        {
            return $"Names: {string.Join(", ", Names)}, Address: {Address.StreetName} {Address.HouseNumber}";
        }

    }

    public class Address {
        public string StreetName;
        public int HouseNumber;

        // Here! Copy constructor
        public Address(Address other) {
            StreetName = other.StreetName;
            HouseNumber = other.HouseNumber;
        }

        public Address() {}
    }
}