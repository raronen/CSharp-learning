// using Prototype.PrototypeInheritance.Before;
using Prototype.PrototypeInheritance.After;

namespace Prototype.PrototypeInheritance {
    public class PrototypeRun {
        public static void Run() {
            var john = new Employee(new []{"John", "Doe"}, new Address {StreetName = "123 London Road", HouseNumber = 123}, 50_000);
            System.Console.WriteLine(john);

            var jane = john.DeepCopy();
            jane.Names[0] = "Jane";
            jane.address.HouseNumber = 321;
            System.Console.WriteLine(jane);
        }
    }

    // There is a problem with this approach, because there is duplicate code. And in big chain of interface, there be lot of duplicate code.
    // So we'll resuse the base.CopyTo() method to avoid duplicate code
    namespace Before {

        // There is a problem with this approach, because there is duplicate code
        public interface IDeepCopyable<T> {
            T DeepCopy();
        }

        public class Person: IDeepCopyable<Person> {
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

            public Person DeepCopy()
            {
                return new Person(Names.Clone() as string[], address.DeepCopy()); // Duplicate code: Notice here: Names.Clone() as string[], address.DeepCopy()
            }
        }

        public class Employee: Person, IDeepCopyable<Employee> {
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

            Employee IDeepCopyable<Employee>.DeepCopy()
            {
                return new Employee(Names.Clone() as string[], this.address.DeepCopy(), Salary); // Duplicate code: Notice here: Names.Clone() as string[], address.DeepCopy()
            }
        }

        public class Address: IDeepCopyable<Address> {
            public string StreetName;
            public int HouseNumber;

            public Address() {}
            public Address(string streetName, int houseNumber) {
                StreetName = streetName;
                HouseNumber = houseNumber;
            }

            public Address DeepCopy()
            {
                return (Address) MemberwiseClone();
            }

            public override string ToString()
            {
                return $"Street Name: {StreetName}, House Number: {HouseNumber}";
            }
        }
    }

    namespace After {
        public interface IDeepCopyable<T>
                where T: new() // has to implement default constructor
        {
            void CopyTo(T target);
            
            // Default implementation
            public T DeepCopy() {
                T t = new T();
                CopyTo(t);
                return t;
            }
        }

        public class Person: IDeepCopyable<Person> {
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

            public void CopyTo(Person target)
            {
                target.Names = (string[]) Names.Clone();
                target.address = address.DeepCopy();
            }
        }

        public class Employee: Person, IDeepCopyable<Employee> {
            public int Salary;

            public Employee() {}

            public Employee(string[] names, Address address, int salary) : base(names, address)
            {
                Salary = salary;
            }

            public void CopyTo(Employee target)
            {
                base.CopyTo(target);
                target.Salary = Salary;
            }

            public override string ToString()
            {
                return base.ToString() + $", Salary: {Salary}";
            }
        }

        public class Address: IDeepCopyable<Address> {
            public string StreetName;
            public int HouseNumber;

            public Address() {}
            public Address(string streetName, int houseNumber) {
                StreetName = streetName;
                HouseNumber = houseNumber;
            }

            public void CopyTo(Address target)
            {
                target.StreetName = StreetName;
                target.HouseNumber = HouseNumber;
            }

            public override string ToString()
            {
                return $"Street Name: {StreetName}, House Number: {HouseNumber}";
            }
        }

        public static class ExtensionMethods {
            public static T DeepCopy<T>(this IDeepCopyable<T> item)
                where T: new()
            {
                return item.DeepCopy();

            }

            public static T DeepCopy<T>(this T person)
                where T: Person, new() {
                    return ((IDeepCopyable<T>) person).DeepCopy();
                }
        }
    }
}
