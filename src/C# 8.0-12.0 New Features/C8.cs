using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSharpNewFeatures {
    public class C8: IExampleInterface {

        public async void run() {
            // 1. Read-only struct - If you call a method on a struct - the compiler will make a defensive copy of the struct. With read-only, the compiler won't do it because it doesn't modify state.
            // 2. Default interface methods
            // 3. Pattern matching enhancements - new switch paterns, tuple patterns, positional patterns, property patterns, and more.
            // 4. Using declarations - create a var that is only available in the scope of the using block, and after the block, the var is disposed.
            // 5. Static local functions - local functions can now be static.
            // 6. Disposable ref structs - ref structs can now implement IDisposable.
            // 7. Nullable reference types - you can now enable nullable reference types in your project.
            // 8. Asynchronous streams - you can now return IAsyncEnumerable<T> from a method.
            // 9. Indices and ranges - you can now use indices and ranges to access elements in a collection.
            // 10. Null-coalescing assignment - you can now use ??= to assign a value to a variable if it is null.
            // 11. Unmanaged constructed types - you can now use unmanaged generic types.
            // 12. Stackalloc in nested expressions - you can now use stackalloc in nested expressions.
            // 13.  Enhancement of interpolated verbatim strings

            // 2.
            IExampleInterface.HelloWorld();

            // 8.
            // IAsyncEnumerable<int> x = AsyncStreamsExample.GenerateSequeneceOfIntegers();

            // await foreach (int item in x) {
            //     Console.WriteLine(item);
            // }

            IndicesAndRangesExample.Example();

        }
    }

    // 1. Read-only struct
    public struct Stats {
        public readonly int Attack;
        public readonly int Defense;

        public readonly override string ToString()
        {
            return $"Attack: {Attack}, Defense: {Defense}";
        }
    }

    // 2. Default interface methods
    interface IExampleInterface {
        public static void HelloWorld() {
            Console.WriteLine("Hello, World!");
        }
    }

    // 3. Pattern matching enhancements
    public class RGBColor {
        public RGBColor(int r, int g, int b) { }
    }

    enum Color { Black, White };

    public class PatternExample {
        // Old way
        static RGBColor GetRGBColor(Color color) {
            switch(color) {
                case Color.Black:
                    return new RGBColor(0, 0, 0);
                case Color.White:
                    return new RGBColor(255, 255, 255);
                default:
                    throw new ArgumentException();
            }
        }

        // New way
        static RGBColor GetRGBColorNew(Color color) => 
            color switch {
                Color.Black => new RGBColor(0, 0, 0),
                Color.White => new RGBColor(255, 255, 255),
                _ => throw new ArgumentException()
            };
    }

    // 4. Using declarations
    public class UsingExample {
        public void Example() {
            using var rsa = RSA.Create();
            // rsa is disposed after funciton ends

            if (true){
                using var file = new FileStream("file.txt", FileMode.Open);
                // file is disposed after if block
            }

            /* Comipled code looks like this:
            using (RSA.Create())
            {
                bool flag = true;
                using (new FileStream("file.txt", FileMode.Open))
                {
                }
            }
            */
        }
    }

    // 5. Static local functions
    public class StaticLocalFunctionExample {
        public void PrintMessage() {
            
            string s = "";

            static void LogMessage() { // <-- so inner methods won't use outer local variables
                Console.WriteLine(/*s*/); // <-- error
            }
        }
    }

    // 6. Disposable ref structs - ref structs can now implement IDisposable.
    ref struct CD {
        public void Dispose() { } // <-- will call in end of using block
    }

    public class DisposableRefStructExample {
        public void Example() {
            using (var book = new CD()) {
                Console.WriteLine("Play Music!");
            }
        }
    }

    // 7. Nullable reference types - you can now enable nullable reference types in your project.
    public class NullableReferenceTypesExample {
        public void Example() {
            // Already supported:
            bool? b = null; // <-- nullable value types

            // New:
            string? s = null; // <-- nullable reference types
        }
    }

    // 8. Asynchronous streams - you can now return IAsyncEnumerable<T> from a method.
    public class AsyncStreamsExample {
        public static async IAsyncEnumerable<int> GenerateSequeneceOfIntegers() {
            for (int i = 0; i < 10; i++) {
                await Task.Delay(500);
                yield return i;
            }
        }
    }

    // 9. Indices and ranges - you can now use indices and ranges to access elements in a collection.
    public class IndicesAndRangesExample {
        public static void Example() {
            List<string> letters = new List<string> { "A", "B", "C", "D", "E" };

            Console.WriteLine(letters[^1]); // <-- E
            Console.WriteLine(letters[0..2].Count); // <-- 2    For - AB
        }
    }

    // 10. Null-coalescing assignment - you can now use ??= to assign a value to a variable if it is null.
    public class NullCoalescingAssignmentExample {
        public void Example() {
            object o = null;

            o ??= new object(); // <-- if o is null, create new object
        }
    }

    // 11. Unmanaged constructed types
    public class UnmanagedConstructedTypesExample {
        public struct MyStruct<T> where T: unmanaged {
            public T field;
        }
        public void Example() {
            // Unmanaged types are types that are not managed by the garbage collector.
            // Unmanaged types include pointers, handles, and other types that contain references to memory locations outside the garbage-collected heap.
        }
    }

    // 12. Stackalloc in nested expressions - you can now use stackalloc in nested expressions.
    public class StackallocInNestedExpressionsExample {
        public void Example() {
            // What is stackalloc? - stackalloc is a C# operator that allocates a block of memory on the stack.
            // Long answer: A stackalloc expression allocates a block of memory on the stack. 
            // A stack-allocated memory block created during the method execution is automatically discarded when that method returns. 
            // You can't explicitly free the memory allocated with stackalloc. A stack allocated memory block isn't subject to garbage 
            // collection and doesn't have to be pinned with a fixed statement.
            Span<int> numbers = stackalloc[] {1, 2, 3, 4 };
        }
    }

    // 13.  Enhancement of interpolated verbatim strings
    public class InterpolatedVerbatimStringsExample {
        public void Example() {
            string name = "John";
            string message = $@"Hello, {name}!"; // $ and @ can be in any order
            string message2 = @$"Hello, {name}!"; // $ and @ can be in any order
        }
    }
}