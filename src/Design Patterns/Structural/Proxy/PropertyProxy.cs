namespace Proxy.Property {

    public class Property<T> where T: new() {
        private T value;

        public T Value {
            get {
                return value;
            }
            set {
                if (Equals(this.value, value)) return;
                Console.WriteLine($"Assigning value to {value}");
                this.value = value;
            }
        }

        public Property(): this(default(T)) {}

        public Property(T value) {
            this.value = value;
        }   
    }
    public class Demo {
        public static void Run() {
        }
    }
}