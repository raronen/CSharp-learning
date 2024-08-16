using System.Runtime.CompilerServices;

namespace CSharpNewFeatures {
    class C9_10_11 {
        public async void run() {
            // https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-version-history#c-version-9
            // 1. C# 9.0 - Record Types
            // 2. C# 9.0 - Top-level statements - Only 1 file in the project can have top-level statements.
            // 3. C# 9.0 - Init-only properties - you can now initialize properties in the constructor.
            // 4. C# 9.0 - With-expressions - you can now create a new object with the same values as an existing object, but with some properties changed.
            // 5. C# 9.0 - Covariant return types - you can now override a method with a more derived return type.
            // 6. C# 9.0 - New target-typed new-expressions - you can now use the new keyword without specifying the type.
            // 7. C# 9.0 - Lambda discard parameters - you can now use the _ character as a discard parameter in a lambda expression.
            // 8. C# 9.0 - Attributes on local functions - you can now add attributes to local functions.
            // 9. C# 9.0 - Module initializers - you can now add a module initializer to a .NET 5 project.
            // 10. C# 9.0 - Function pointers - you can now use function pointers in C#.
            // 11. C# 9.0 - Skip locals init - you can now skip locals init in a method.
            // 12. C# 9.0 - Caller argument expression - you can now use the caller argument expression attribute.
            // 14. C# 9.0 - Target-typed conditional expressions - you can now use target-typed conditional expressions.
            // 15. C# 9.0 - Static anonymous functions - you can now use static anonymous functions.

            // C# 10.0 - Global using directives - you can now add global using directives to a project: "global using System;"
            // C# 10.0 - File-scoped namespace declaration - you can now declare a namespace at the file level: "namespace MyNamespace;"
            // C# 10.0 - Constant interpolated strings - you can now use constant interpolated strings: "const string message = $"{name}";"
            // C# 10.0 - Improvements to the 'using' statement - you can now use the 'using' statement with multiple resources: "using var resource1 = new Resource1(); using var resource2 = new Resource2();"
            // C# 10.0 - Improvements to the 'interpolated string handler' - you can now use the 'interpolated string handler' to customize the behavior of interpolated strings.

            



            // 4. Usage of With-expressions
            Person3 person = new Person3("John", "Doe");
            Person3 updatedPerson = person.WithFirstName("Jane").WithLastName("Smith");
        }
    }

    // 1. C# 9.0 - Record Types - record types are reference types that provide built-in functionality for encapsulating data.
    // Immutable by default, but you can make them mutable.
    public record Information { }
    public record Person(string FirstName, string LastName);

    // 3. C# 9.0 - Init-only properties - you can now initialize properties in the constructor.
    public class Person2 {
        public string FirstName { get; init; } // <-- init-only property

        public Person2(string firstName) {
            FirstName = firstName;
        }
    }

    // 4. C# 9.0 - With-expressions - you can now create a new object with the same values as an existing object, but with some properties changed.
    public record class Person3
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }

        public Person3(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public Person3 WithFirstName(string newFirstName)
        {
            return this with { FirstName = newFirstName };
        }

        public Person3 WithLastName(string newLastName)
        {
            return this with { LastName = newLastName };
        }
    }

    // 5. C# 9.0 - Covariant return types - you can now override a method with a more derived return type.
    public class Animal
    {
        public virtual Animal GetChild()
        {
            return new Animal();
        }
    }

    public class Dog : Animal
    {
        public override Dog GetChild()
        {
            return new Dog();
        }
    }

    // 6. C# 9.0 - New target-typed new-expressions - you can now use the new keyword without specifying the type.
    public class Example
    {
        public void CreateObject()
        {
            var o = new object();
            o = new(); // <-- new target-typed new-expression
        }
    }
    
    // C# 9.0 - Lambda discard parameters
    public class LambdaDiscardParametersExample
    {
        public void Example()
        {
            Func<int, int, int> add = (int x, int y) => x + y;
            Func<int, int, int> add2 = (_, _) => 0; // <-- discard parameters
        }
    }

    // C# 9.0 - Attributes on local functions - you can now add attributes to local functions.
    public class AttributesOnLocalFunctionsExample
    {
        public void Example()
        {
            [Obsolete]
            void LogMessage()
            {
                Console.WriteLine("Message");
            }

            LogMessage();
        }
    }

    // 12. C# 9.0 - Caller argument expression - you can now use the caller argument expression attribute.
    public class CallerArgumentExpressionExample
    {
        public void Example([CallerArgumentExpression("message")] string message = "")
        {
            Console.WriteLine(message);
        }
    }
}