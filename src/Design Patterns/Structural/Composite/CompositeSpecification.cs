namespace Composite.Specification {

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

    // This is the Composite Specification!! 
    public abstract class CompositeSpecification<T>: ISpecification<T> {
        protected readonly ISpecification<T>[] items; 
        public CompositeSpecification(params ISpecification<T>[] items) {
            this.items = items;
        }
        public bool IsSatisfied(T t)
        {
            throw new NotImplementedException();
        }
    }

    // combinator
    public class AndSpecification<T> : CompositeSpecification<T>
    {
        public AndSpecification(params ISpecification<T>[] items) : base(items) { }
        public bool IsSatisfied(T t)
        {
            return items.All(i => i.IsSatisfied(t));
        }
    }

    public class OrSpecification<T> : CompositeSpecification<T>
    {
        public OrSpecification(params ISpecification<T>[] items) : base(items) { }
        public bool IsSatisfied(T t)
        {
            return items.Any(i => i.IsSatisfied(t));
        }
    }

    public class Demo {
        public static void Run() {

        }
    }
}