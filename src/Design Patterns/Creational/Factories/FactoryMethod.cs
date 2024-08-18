namespace Factories.FactoryMethod {
    public class Point {
        private double x, y;
        private Point(double x, double y) { // private constructor
            this.x = x;
            this.y = y;
        }

        // Factory methods
        public static Point NewCartesianPoint(double x, double y) {
            return new Point(x, y);
        }

        public static Point NewPolarPoint(double rho, double theta) {
            return new Point(rho * System.Math.Cos(theta), rho * System.Math.Sin(theta));
        }
    }

    public class FactoryMethod {
        public static void Run() {
            var point = Point.NewPolarPoint(1.0, System.Math.PI / 2);
            var point2 = Point.NewCartesianPoint(2, 3);
        }
    }
}