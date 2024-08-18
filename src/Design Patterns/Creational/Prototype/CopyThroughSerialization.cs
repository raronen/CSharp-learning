using System.Data;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Prototype.CopyThroughSerialization {

    // Caveat of using BinaryFormatter is to mark all classes as [Serializable], this why we use DeepCopyXML with XmlSerializer!
    // But - for XMLSerializer, you have to have empty constructor.
    // There are different requierments for different serializers.
    // Requierments: 
    // BinaryFormatter - [Serializable]
    // XMLSerializer - Empty constructor
    public class CopyThroughSerialization {
        public static void Run() {
            var john = new Employee(new []{"John", "Doe"}, new Address {StreetName = "123 London Road", HouseNumber = 123}, 5000);
            System.Console.WriteLine(john);

            var jane = john.DeepCopyXML();
            jane.Names[0] = "Jane";
            jane.address.HouseNumber = 321;
            System.Console.WriteLine(jane);
        }
    }

    public static class ExtensionMethods {
        // Caveat of using BinaryFormatter is to mark all classes as [Serializable]
        public static T DeepCopy<T>(this T self) {
            var stream = new MemoryStream();
            #pragma warning disable SYSLIB0011 // Type or member is obsolete
            // Caveat of using BinaryFormatter is to mark all classes as [Serializable]
            var formatter = new BinaryFormatter();
            #pragma warning restore SYSLIB0011 // Type or member is obsolete

            formatter.Serialize(stream, self);
            stream.Seek(0, SeekOrigin.Begin);
            object copy = formatter.Deserialize(stream);
            stream.Close();

            return (T) copy;
        }

        public static T DeepCopyXML<T>(this T self) {
            var stream = new MemoryStream();
            var formatter = new XmlSerializer(typeof(T));
            formatter.Serialize(stream, self);

            stream.Position = 0;
            var copy = formatter.Deserialize(stream);

            return (T) copy;
        }
    }

    public class Person {
        public string[] Names;
        public Address address;

        public Person(string[] names, Address address)
        {
            Names = names;
            this.address = address;
        }

        public Person() {}

        public override string ToString()
        {
            return $"Names: {string.Join(", ", Names)}, Address: {address}";
        }
    }

    public class Employee: Person {
        public int Salary;

        public Employee() {}

        public Employee(string[] names, Address address, int salary) : base(names, address)
        {
            Salary = salary;
        }

        public override string ToString()
        {
            return base.ToString() + $", Salary: {Salary}";
        }
    }

    public class Address {
        public string StreetName;
        public int HouseNumber;

        public Address() {}
        public Address(string streetName, int houseNumber) {
            StreetName = streetName;
            HouseNumber = houseNumber;
        }

        public override string ToString()
        {
            return $"Street Name: {StreetName}, House Number: {HouseNumber}";
        }
    }
}