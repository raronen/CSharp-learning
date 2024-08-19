using System.Text;

namespace Singleton.Ambient {

    // Ambient Context - it's present everywhere. And it's a singleton. Or perhaps a thread-local singleton.
    public sealed class BuildingContext: IDisposable {
        public int WallHeight = 3000;
        private static Stack<BuildingContext> stack = new Stack<BuildingContext>();

        static BuildingContext() {
            stack.Push(new BuildingContext(0));
        }

        public BuildingContext(int wallHeight) {
            WallHeight = wallHeight;
            stack.Push(this);
        }

        public static BuildingContext Current => stack.Peek();

        public void Dispose()
        {
            if (stack.Count > 1) {
                stack.Pop();
            }
        }
    }
    public class Demo {
        public static void Run() {
            var building = new Building();

            // BuildingContext.WallHeight = 3000; // height is Ambient Context - it's present everywhere.
            using (new BuildingContext(3000)) {
                    // gnd 3000
                    building.Walls.Add(new Wall(new Point(0, 0), new Point(5000, 0)));
                    building.Walls.Add(new Wall(new Point(0, 0), new Point(0, 4000)));

                using (new BuildingContext(3500)) {
                    // 1st 3500
                    building.Walls.Add(new Wall(new Point(0, 0), new Point(6000, 0)));
                    building.Walls.Add(new Wall(new Point(0, 0), new Point(0, 4000)));

                    using (new BuildingContext(3000)) {
                        building.Walls.Add(new Wall(new Point(0, 0), new Point(5000, 4000)));
                    }
                }
            }

            Console.WriteLine(building);
        }
    }
    public class Building {
        public List<Wall> Walls = new List<Wall>();

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var wall in Walls)
                sb.AppendLine(wall.ToString());
            return sb.ToString();
        }
    }

    public struct Point {
        private int x, y;
        public Point(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"{nameof(x)}: {x}, {nameof(y)}: {y}";
        }
    }
    public class Wall {
        public Point Start, End;
        public int Height;
        public Wall(Point start, Point end) {
            Start = start;
            End = end;
            Height = BuildingContext.Current.WallHeight;
        }

        public override string ToString()
        {
            return $"{nameof(Start)}: {Start}, {nameof(End)}: {End}, {nameof(Height)}: {Height}";
        }
    }
}