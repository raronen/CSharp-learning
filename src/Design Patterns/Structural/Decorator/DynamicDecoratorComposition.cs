namespace Decorator.DynamicDecoratorComposition

// Dynamic Decorator - in the sense that you can make them in runtime and not compile time
// Composition - we are using composition to add functionality: ColoredShape, TransparentShape
// Cycle can happen if we are not careful
{
    public interface IShape {
        string AsString();
    }

    public class Circle: IShape {
        private float radius;

        public Circle(float radious) {
            this.radius = radious;
        }

        public void Resize(float factor) {
            radius *= factor;
        }

        public string AsString() => $"A circle with radius {radius}";
    }

    public class Square: IShape {
        private float side;

        public Square(float side) {
            this.side = side;
        }

        public string AsString() =>  $"A square with side {side}";
    }

    public class ColoredShape: IShape {
        private IShape shape;
        private string color;

        public ColoredShape(IShape shape, string color) {
            this.shape = shape;
            this.color = color;
        }

        public string AsString() => $"{shape.AsString()} has the color {color}";
    }

    public class TransparentShape: IShape {
        private IShape shape;
        private float transperncy;

        public TransparentShape(IShape shape, float transperncy) {
            this.shape = shape;
            this.transperncy = transperncy;
        }

        public string AsString() => $"{shape.AsString()} has {transperncy * 100.0f}% transparency";
    }

    public class Demo
    {
        public static void Run()
        {
            var square = new Square(1.23f);
            Console.WriteLine(square.AsString());

            var redSquare = new ColoredShape(square, "red");

            Console.WriteLine(redSquare.AsString());

            var redHalfTransparentSquare = new TransparentShape(redSquare, 0.5f);

            Console.WriteLine(redHalfTransparentSquare.AsString());
        }
    }
}