using System.Threading.Tasks.Dataflow;

namespace DesignPatterns.Faceted {

    public class FacetedBuilder {
        public static void Run() {
            var pb = new PersonBuilder();

            Person person = pb
                .Works.At("Microsoft").AsA("Developer").Earing(100000)
                .Lives.At("123 London Road").In("London")
                .Build();
            
            Console.WriteLine(person);
        }
    }

    public class Person {
        // adress
        public string StreetAddress, Postcode, City;

        // employment
        public string CompanyName, Position;
        public int AnnualIncome;

        public override string ToString()
        {
            return $"Address: {StreetAddress}, {Postcode}, {City}\n" +
                   $"Employment: {CompanyName}, {Position}, {AnnualIncome}";
        }
    }

    public class PersonBuilder // Facade for other builders. It doesn't build the person itself, but it keeps the person that is beeing built. And it allows you access to those sub builders
    {
        // reference!
        protected Person person = new Person();

        public PersonJobBuilder Works => new PersonJobBuilder(person);
        public PersonAdressBuilder Lives => new PersonAdressBuilder(person);

        public Person Build() {
            return person;
        }
    }

    // This is a sub builder - 1st Facet
    public class PersonJobBuilder: PersonBuilder {
        public PersonJobBuilder(Person person)
        {
            this.person = person;
        }

        public PersonJobBuilder At(string companyName) {
            person.CompanyName = companyName;
            return this;
        }

        public PersonJobBuilder AsA(string position) {
            person.Position = position;
            return this;
        }

        public PersonJobBuilder Earing(int amount) {
            person.AnnualIncome = amount;
            return this;
        }
    }

    // This is a sub builder - 2nd Facet
    public class PersonAdressBuilder: PersonBuilder {
        public PersonAdressBuilder(Person person)
        {
            this.person = person;
        }

        public PersonAdressBuilder At(string streetAddress) {
            person.StreetAddress = streetAddress;
            return this;
        }

        public PersonAdressBuilder WithPostcode(string postcode) {
            person.Postcode = postcode;
            return this;
        }

        public PersonAdressBuilder In(string city) {
            person.City = city;
            return this;
        }
    }
}