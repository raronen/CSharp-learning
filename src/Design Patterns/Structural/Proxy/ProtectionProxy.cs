namespace Proxy.Protection {
    public interface ICar {
        void Drive();
    }

    public class Car : ICar {
        public void Drive() {
            Console.WriteLine("Car is being driven");
        }
    }

    public class Driver {
        public int Age { get; set; }
    }

    // You don't add other members to the proxy class, you just add the functionality you want to control.
    public class CarProxy: ICar {
        private Driver driver;
        private Car car = new Car();

        public CarProxy(Driver driver) {
            this.driver = driver;
        }

        public void Drive() {
            if (driver.Age > 16) {
                car.Drive();
            } else {
                Console.WriteLine("Driver too young");
            }
        }
    }

    public class Demo {
        public static void Run() {
            ICar car = new CarProxy(new Driver { Age = 18 });
            car.Drive();
        }
    }
}