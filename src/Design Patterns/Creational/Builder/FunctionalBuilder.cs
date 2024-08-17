namespace DesignPatterns.Funcitonal {

    public class FunctionalBuilder {
        public static void Run(string[] args) {
            var person = new PersonBuilder()
                .Called("Dmitri")
                // Extension method
                .WorksAs("Developer")
                .Build();
        }
    }
    public class Person {
        public string Name, Position;
    }

    public sealed class PersonBuilder {
        private readonly List<Func<Person, Person>> actions = new List<Func<Person, Person>>();

        public PersonBuilder Called(string name) => 
            Do(p => p.Name = name);

        public PersonBuilder Do(Action<Person> action) => AddAction(action);

        public Person Build() => actions.Aggregate(new Person(), (p, f) => f(p)); //Aggregate is a LINQ method. It's like reduce in javascript

        private PersonBuilder AddAction(Action<Person> action) {
            actions.Add(p => { action(p); return p; });

            return this;
        }
    }

    // We want to stick to Open Close Principal, instead of inheritiance, or change the class itself, which won't work, we'll use *extensions method*.
    public static class PersonBuilderExtensions {
        public static PersonBuilder WorksAs
            (this PersonBuilder builder, string position) => 
            builder.Do(p => p.Position = position);
    }
}