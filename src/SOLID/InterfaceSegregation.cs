// Principle: Interfaces should be segregated so nobody implementing our interface requires to implement functionality which doesn't exists./
// should be small and focused on a single responsibility

namespace DesignPatterns {
    public class Document {

    }

    public interface IMachine {
        void Print(Document d);
        void Fax(Document d);
        void Scan(Document d);
    }

    public class MultiFunctionPrinter : IMachine
    {
        public void Fax(Document d)
        {
            // ..
        }

        public void Print(Document d)
        {
            // ..
        }

        public void Scan(Document d)
        {
            // ..
        }
    }

    public class OldFashionPrinter: IMachine {
        public void Fax(Document d)
        {
            // WHAT DO WE DO? this is an Old fashon printer and doesn't have this functionality!
            // BREAKS the Interface Segregation principle
        }

        public void Print(Document d)
        {
            // ..
        }

        public void Scan(Document d)
        {
            // WHAT DO WE DO? this is an Old fashon printer and doesn't have this functionality!
            // BREAKS the Interface Segregation principle
        }
    }

    // Fix:
    public interface IPrinter {
        void Print(Document d);
    }

    public interface IScanner {
        void Scan(Document d);
    }

    public class Photocopier : IPrinter, IScanner {
        public void Print(Document d)
        {
            // ..
        }

        public void Scan(Document d)
        {
            // ..
        }
    }

    public interface IMultiFunctionDevice : IPrinter, IScanner {}

    public class MultiFunctionMachine : IMultiFunctionDevice
    {
        private IPrinter printer;
        private IScanner scanner;
        public void Print(Document d)
        {
            if (printer != null)
            {
                printer.Print(d);
            }
        }

        public void Scan(Document d)
        {
            if (scanner != null)
            {
                scanner.Scan(d);
            }
        }
    }
}