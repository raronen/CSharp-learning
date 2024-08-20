namespace Decorator.StaticDecoratorComposition
{
    public class Demo
    {
        public static void Run() {
            var square = new TransparentShape<ColoredShape<Square>>(0.4f); // Can't pass color! only parameter for TransparenShape
            // square.color - can't access ColoredShape property

            Console.WriteLine(square.AsString());
        }
    }

    public abstract class Shape {
        public abstract string AsString();
    }

    public class Circle: Shape {
        private float radius;

        public float Radious {  get; set; }

        public Circle(float radious) {
            this.radius = radious;
        }

        public void Resize(float factor) {
            radius *= factor;
        }

        public override string AsString() => $"A circle with radius {radius}";
    }

    public class Square: Shape {
        private float side;

        public Square(float side) {
            this.side = side;
        }

        public Square(): this(0f) { }

        public override string AsString() =>  $"A square with side {side}";
    }

    public class ColoredShape: Shape {
        private Shape shape;
        private string color;

        public ColoredShape(Shape shape, string color) {
            this.shape = shape;
            this.color = color;
        }

        public override string AsString() => $"{shape.AsString()} has the color {color}";
    }

    public class TransparentShape: Shape {
        private Shape shape;
        private float transperncy;

        public TransparentShape(Shape shape, float transperncy) {
            this.shape = shape;
            this.transperncy = transperncy;
        }

        public TransparentShape(Shape shape) : this(shape, 0f) { }

        public override string AsString() => $"{shape.AsString()} has {transperncy * 100.0f}% transparency";
    }

    public class ColoredShape<T> : Shape 
        where T : Shape, new() {
        private string color;
        private T shape = new T();

        public ColoredShape(string color) {
            this.color = color;
        }

        public ColoredShape() : this("black") { }
        
        public override string AsString() {
            return $"{shape.AsString()} has the color {color}";
        }
    }

    public class TransparentShape<T> : Shape
        where T : Shape, new()
    {
        private float t;
        private T shape = new T();

        public TransparentShape(float t)
        {
            this.t = t;
        }

        public TransparentShape() : this(0f) { }

        public override string AsString()
        {
            return $"{shape.AsString()} has {t * 100f}% transparency";
        }
    }
}