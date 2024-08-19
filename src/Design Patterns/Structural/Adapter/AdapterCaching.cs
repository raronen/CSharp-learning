using MoreLinq;
using System.Collections;
using System.Collections.ObjectModel;

// Side affect of using dapater is temporary information - we can get arround it by using a cache
namespace Adapter.Catching {

    public class Point {
        public int X, Y;

        public Point(int x, int y) {
            X = x;
            Y = y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                return hash;
            }
        }
    }

    public class Line {
        public Point Start, End;

        public Line(Point start, Point end) {
            Start = start;
            End = end;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Start.GetHashCode();
                hash = hash * 23 + End.GetHashCode();
                return hash;
            }
        }
        
    }

    public class VectorObject: Collection<Line> {

    }

    public class VectorRectangle: VectorObject {
        public VectorRectangle(int x, int y, int width, int height) {
            Add(new Line(new Point(x, y), new Point( x + width, y)));
            Add(new Line(new Point(x + width, y), new Point( x + width, y + height)));
            Add(new Line(new Point(x, y + height), new Point( x + width, y + height)));
            Add(new Line(new Point(x, y), new Point( x, y + height)));
        }
    }

    public class LineToPointAdapter: IEnumerable<Point>
    {
        private static int count = 0;
        static Dictionary<int, List<Point>> cache = new Dictionary<int, List<Point>>() {};

        public LineToPointAdapter(Line line) {
            var hash = line.GetHashCode();

            if (cache.ContainsKey(hash))
            {
                return;
            }

            List<Point> points = new List<Point>();

            Console.WriteLine($"{++count}: Generating points for line [{line.Start.X},{line.Start.Y}]-[{line.End.X},{line.End.Y}]");

            int left = Math.Min(line.Start.X, line.End.X);
            int right = Math.Max(line.Start.X, line.End.X);
            int top = Math.Min(line.Start.Y, line.End.Y);
            int bottom = Math.Max(line.Start.Y, line.End.Y);
            int dx = right - left;
            int dy = line.End.Y - line.Start.Y;

            if (dx == 0)
            {
                for (int y = top; y <= bottom; ++y)
                {
                    points.Add(new Point(left, y));
                }
            } else if (dy == 0)
            {
                for (int x = left; x <= right; ++x)
                {
                    points.Add(new Point(x, top));
                }
            }

            cache.Add(hash, points);
        }

        public IEnumerator<Point> GetEnumerator()
        {
            return cache.SelectMany(x => x.Value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class Demo {

        private static readonly List<VectorObject> vectorObjects = new List<VectorObject> {
            new VectorRectangle(1, 1, 10, 10),
            new VectorRectangle(3, 3, 6, 6)
        };

        // We can only draw "." point
        public static void DrawPoint(Point p) {
            Console.Write(".");
        }

        public static void Draw() {
            foreach (var vo in vectorObjects)
            {
                foreach (var line in vo)
                {
                    var adapter = new LineToPointAdapter(line);
                    adapter.ForEach(DrawPoint);
                }
            }
        }
        public static void Run() {
            Draw();
            Draw();
        }
    }
}