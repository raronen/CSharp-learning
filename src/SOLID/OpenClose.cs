// Principle - Open for extension, but closed for modification.

namespace DesignPatterns {
    public enum Color {
        Red, Green, Blue
    }

    public enum Size {
        Small, Medium, Large, Yuge
    }

    public class Product {
        public string Name;
        public Color Color;
        public Size Size;

        public Product(string name, Color color, Size size)
        {
            if (name == null) {
                throw new ArgumentNullException(paramName: nameof(name));
            }
            Name = name;
            Color = color;
            Size = size;
        }
    }

    public class ProductFilter {
        public IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size) {
            foreach(var p in products) {
                if (p.Size == size) {
                    yield return p;
                }
            }
        }

        // start - BREAK Open Close principle - we have to modify this class every time we want to add a new filter
        // In this case - Color
        public IEnumerable<Product> FilterByColor(IEnumerable<Product> products, Color color) {
            foreach(var p in products) {
                if (p.Color == color) {
                    yield return p;
                }
            }
        }
        // end

    }

    // Now, demand to filter by BOTH filters.
    // So it's not only copy and rename function, but also change the logic.
    // Breaks: Open for extension, but closed for modification.
    // Solution in this case: Inheritance.

    public interface ISpecification<T> {
        bool IsSatisfied(T t);
    }

    public interface IFilter<T> {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }

    // Lets support multiple filters
    public class ColorSpecification: ISpecification<Product> {
        private Color color;
        public ColorSpecification(Color color) {
            this.color = color;
        }

        public bool IsSatisfied(Product p) {
            return p.Color == color;
        }
    }

    public class BetterFilter: IFilter<Product> {
        public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec) {
            foreach(var i in items) {
                if (spec.IsSatisfied(i)) {
                    yield return i;
                }
            }
        }
    }

    // Now, since it's open for extension, we can create new Specification:
    public class SizeSpecification: ISpecification<Product> {
        private Size size;
        public SizeSpecification(Size size) {
            this.size = size;
        }

        public bool IsSatisfied(Product p) {
            return p.Size == size;
        }
    }

    public class AndSpecification<T> : ISpecification<T>
    {
        public ISpecification<T> first, second;
        public AndSpecification(ISpecification<T> first, ISpecification<T> second)
        {
            this.first = first ?? throw new ArgumentNullException(paramName: nameof(first));
            this.second = second ?? throw new ArgumentNullException(paramName: nameof(second));
        }
        public bool IsSatisfied(T t)
        {
            return first.IsSatisfied(t) && second.IsSatisfied(t);
        }
    }
}

/*
In main:
using System.Diagnostics;
using System.Runtime.CompilerServices;
using DesignPatterns;

var apple = new Product("Apple", Color.Green, Size.Small);
var tree = new Product("Tree", Color.Green, Size.Large);
var house = new Product("House", Color.Blue, Size.Large);

Product[] products = {apple, tree, house};

var pf = new ProductFilter();
Console.WriteLine("Green products (old):");
foreach (var p in pf.FilterByColor(products, Color.Green))
{
    Console.WriteLine($" - {p.Name} is green");
}

var bf = new BetterFilter();
Console.WriteLine("Green products (new):");
foreach(var p in bf.Filter(products, new ColorSpecification(Color.Green))) {
    Console.WriteLine($" - {p.Name} is green");
}

Console.WriteLine("Green products (new):");
foreach(var p in bf.Filter(products, 
    new AndSpecification<Product>(
        new ColorSpecification(Color.Green),
        new SizeSpecification(Size.Large))
    )) {
    Console.WriteLine($" - {p.Name} is big and blue");
}

*/