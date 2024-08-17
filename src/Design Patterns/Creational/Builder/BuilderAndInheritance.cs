namespace DesignPatterns.BuilderAndInheritance
{

    public class BuilderAndInheritance
    {
        public static void Run(string[] args)
        {
            // Before (Problematic):
            /*
            var pb = new PersonJobBuilder();
            // This will not work - as we are not returning Person2 object from PersonJobBuilder2
            var person = pb.Called("Dmitri").WorksAsA("Quant").Build(); 
            */

            // Solution - Recursive Generics
            Person.New.Called("Dmitri").WorksAsA("Quant").Build();
        }
    }
    public class Person
    {
        public string Name;

        public string Position;

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Position)}: {Position}";
        }

        // Part of solution
        public class Builder : PersonJobBuilder<Builder> {

        }

        public static Builder New => new Builder();
    }

    // Before (Problematic):
    public class PersonInfoBuilder
    {
        protected Person person = new Person();

        public PersonInfoBuilder Called(string name)
        {
            person.Name = name;
            return this;
        }
    }

    // Inheritance - for (Open Close principal!) - but - you'll see in Run method - it will not work
    public class PersonJobBuilder : PersonInfoBuilder
    {
        public PersonJobBuilder WorksAsA(string position)
        {
            person.Position = position;
            return this;
        }
    }

    // Solution - Recursive Generics

    public abstract class PersonBuilder {
        protected Person person = new Person();

        public Person Build() {
            return person;
        }
    }

    // This is the base class
    // Class Foo: Bar<Foo> - Foo is the derived class
    public class PersonInfoBuilderV2<SELF> : PersonBuilder
        where SELF : PersonInfoBuilderV2<SELF>
    {
        public SELF Called(string name) {
            person.Name = name;
            return (SELF) this;
        }
    }

    public class PersonJobBuilder<SELF> : PersonInfoBuilderV2<PersonJobBuilder<SELF>>
      where SELF : PersonJobBuilder<SELF>
    {
        public SELF WorksAsA(string position)
        {
            person.Position = position;
            return (SELF) this;
        }
    }
}