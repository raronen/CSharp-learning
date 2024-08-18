namespace Factories.PointExample {

    // Displaying the problem:
    public class Point {
        private double x, y;
        public Point(double x, double y) {
            this.x = x;
            this.y = y;
        }

        // Can't do this with a constructor
        /*
        public Point(double rho, double theta) {
            x = rho * System.Math.Cos(theta);
            y = rho * System.Math.Sin(theta);
        }
        */

        // So you do:
         public Point(double a, double b, CoordinateSystem system = CoordinateSystem.Polar) {
            switch (system) {
                case CoordinateSystem.Cartesian:
                    x = a;
                    y = b;
                    break;
                case CoordinateSystem.Polar:
                    x = a * System.Math.Cos(b);
                    y = a * System.Math.Sin(b);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(system), system, null);
            }
        }
    }

    public enum CoordinateSystem {
        Cartesian,
        Polar
    }

    public class PointExample {
        public static void Run() {
           
        }
    }
}