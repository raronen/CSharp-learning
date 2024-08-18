// Extracting Factory to its own class
// But notice -> Caveat is that constructor is PUBLIC. 
// Solution: Inner Factory Class
namespace Factories.Factory {

    public class Factory {
        public static void Run() {
            var point = Point.Factory.NewPolarPoint(1.0, System.Math.PI / 2);
            var point2 = Point.Factory.NewCartesianPoint(2, 3);

            // Like Task btw:
            // Task.Factory.StartNew..
        }
    }
    public class Point {
        private double x, y;
        private Point(double x, double y) { 
            this.x = x;
            this.y = y;
        }

        // Move to Inner class:
        public static class Factory {
            public static Point NewCartesianPoint(double x, double y) {
                return new Point(x, y);
            }

            public static Point NewPolarPoint(double rho, double theta) {
                return new Point(rho * System.Math.Cos(theta), rho * System.Math.Sin(theta));
            }
        }

        // BTW: 
        // "=>" make it a property
        public static Point Origin => new Point(0, 0);

        // Field, Initialize it only once
        public static Point Origin2 = new Point(0, 0); // better
    }
}