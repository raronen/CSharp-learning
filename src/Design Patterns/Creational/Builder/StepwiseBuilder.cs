namespace DesignPatterns.Stepwise {

    // Enforcing order of execution of methods.
    // First: Type
    // Second: WheelSize
    // Third: Build
    public class StepwiseBuilder {
        public static void Run(string[] args) {
            Car car = CarBuilder.Create().OfType(CarType.CrossOver).WithWheelSize(18).Build();
        }
    }
    public class Car {
        public CarType Type;
        public int WheelSize;
    }

    public enum CarType {
        Sedan,
        CrossOver,
    }

    public interface ISpecifictCarType {
        ISpecifyWheelSize OfType(CarType type);
    }

    public interface ISpecifyWheelSize {
        IBuildCar WithWheelSize(int size);
    }

    public interface IBuildCar {
        public Car Build();
    }

    public class CarBuilder {

        // We can also put the CarBuilder inside Car, but than it will be tightly coupled
        // Impl class is private, so it can't be accessed from outside - and also no one knows it exists
        private class Impl : ISpecifictCarType, ISpecifyWheelSize, IBuildCar
        {
            private Car car = new Car();
            public ISpecifyWheelSize OfType(CarType type)
            {
                car.Type = type;
                return this;
            }
            public IBuildCar WithWheelSize(int size) {
                switch (car.Type) {
                    case CarType.CrossOver when size < 17 || size > 20:
                    case CarType.Sedan when size < 15 || size > 17:
                        throw new ArgumentException($"Invalid wheel size {size} for {car.Type}");
                }

                car.WheelSize = size;
                return this;
            }
            public Car Build()
            {
                return car;
            }
        }

        public static ISpecifictCarType Create() => new Impl();
    }
}