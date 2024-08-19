using Autofac;

namespace Bridge {

    public interface IRenderer {
        void RenderCircle(float radius);
    }

    public class VectorRenderer: IRenderer {
        public void RenderCircle(float radius) {
            Console.WriteLine($"Drawing a circle of radius {radius}");
        }
    }

    public class RasterRenderer: IRenderer {
        public void RenderCircle(float radius) {
            Console.WriteLine($"Drawing pixels for a circle of radius {radius}");
        }
    }

    public abstract class Shape {
        protected IRenderer renderer;

        public Shape(IRenderer renderer) {
            this.renderer = renderer;
        }

        public abstract void Draw();
        public abstract void Resize(float factor);
    }

    public class Circle : Shape
    {
        private float radious;
        public Circle(IRenderer renderer, float radious) : base(renderer)
        {
            this.radious = radious;
        }

        public override void Draw()
        {
            renderer.RenderCircle(radious);
        }

        public override void Resize(float factor)
        {
            radious *= factor;
        }
    }

    public class Demo {
        public static void Run() {
            // IRenderer renderer = new VectorRenderer();
            IRenderer renderer = new RasterRenderer();
            var circle = new Circle(renderer, 5);

            circle.Draw();
            circle.Resize(2);
            circle.Draw();





            // Using Dependency Injection:
            var cb = new ContainerBuilder();
            cb.RegisterType<VectorRenderer>().As<IRenderer>() // Everytime someone asks for IRenderer, give them a VectorRenderer
                .SingleInstance(); // Singleton
            cb.Register((c, p) => 
                new Circle(c.Resolve<IRenderer>(), p.Positional<float>(0))); // Positional means give it in Resolve, and place it as 1st parameter

            using(var c = cb.Build()) {
                var _circle = c.Resolve<Circle>( new PositionalParameter(0, 5.0f));

                _circle.Draw();
            }
        }
    }
}