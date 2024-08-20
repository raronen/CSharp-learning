using CustomStringBuilder = Decorator.CustomStringBulder.CodeBuilder;


namespace Decorator.AdapterDecorator {

/*
// We want to *add* implicit conversion of string and, StringBuilder doesn't support += operator. So it's decorator
// We also want to convert StringBuilder to string implicitly. so it's also adapter pattern
StringBuilder s = "hello ";
s += "world";
Console.WriteLine(s);
*/

    public class MyStringBuilder : CustomStringBuilder {
        public static implicit operator MyStringBuilder(string s) {
            var msb = new MyStringBuilder();
            msb.Append(s);
            return msb;
        }

        public static MyStringBuilder operator +(MyStringBuilder msb, string s) {
            msb.Append(s);
            return msb;
        }
    }

    public class Demo {
        public static void Run() {
            // Implicit coversion of string to MyStringBuilder - Adapter pattern
            MyStringBuilder s = "hello ";
            // Overloaded + operator - Decorator pattern
            s += "world";
            Console.WriteLine(s);
        }
    }
}