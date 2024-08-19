using System.Text;
using System.Text.RegularExpressions;

namespace Composite.GeometricShapes {
    public class Demo {

        public class GraphicObject {
            public string Color;
            public virtual string Name { get; set; } = "Group";

            private Lazy<List<GraphicObject>> children = new Lazy<List<GraphicObject>>();

            public List<GraphicObject> Children => children.Value;

            public void Print(StringBuilder sb, int depth) {
                sb.Append(new string('*', depth))
                    .Append(string.IsNullOrWhiteSpace(Color) ? "" : $"{Color} ")
                    .AppendLine(Name);

                foreach (var child in Children)
                    child.Print(sb, depth + 1);
            }

            public override string ToString()
            {
                var sb = new StringBuilder();
                Print(sb, 0);
                return sb.ToString();
            }
        }

        public class Circle : GraphicObject {
            public override string Name => "Circle";
        }

        public class Square : GraphicObject {
            public override string Name => "Square";
        }
        public static void Run() {
            var drawing = new GraphicObject { Name = "My Drawing" };
            drawing.Children.Add(new Square { Color = "Red" });
            drawing.Children.Add(new Circle { Color = "Yellow" });

            var group = new GraphicObject();
            group.Children.Add(new Circle{Color = "Blue"});
            group.Children.Add(new Circle{Color = "Purple"});
            drawing.Children.Add(group);
            
            Console.WriteLine(drawing);
        }
    }
}