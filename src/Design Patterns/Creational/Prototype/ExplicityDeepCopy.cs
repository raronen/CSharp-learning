namespace Prototype.ExplicitlyDeepCopy {
    // Still problematic - Cause it's tediouce to implement DeepCopy for every class, imagine you have many class in composition
    public class ExplicitlyDeepCopy {
        public static void Run() {
            var john = new Person(new []{"John", "Doe"}, new Address {StreetName = "123 London Road", HouseNumber = 123});
            System.Console.WriteLine(john);

            var jane = john.DeepCopy();
            jane.Names[0] = "Jane";
            System.Console.WriteLine(jane);
        }
    }

    // Explicitly deep copy
    public interface IPrototype<T> {
        T DeepCopy();
    }

    public class Person: IPrototype<Person> {
        public string[] Names;
        private Address Address;
        
        public Person DeepCopy()
        {
            return new Person(Names, Address.DeepCopy());
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

    public class Address: IPrototype<Address> {
        public string StreetName;
        public int HouseNumber;

        public Address DeepCopy() {
            return new Address {StreetName = StreetName, HouseNumber = HouseNumber};
        }

        public Address() {}
    }
}