// Principle: You should be able to subtidute a base type for a subtype

namespace DesignPatterns {
    public class Rectangle {
        // public int Width { get; set; } // <- this is a Problem
        // public int Height { get; set; } // <- this is a Problem

        public virtual int Width { get; set; } // <- Fix! Virtual
        public virtual int Height { get; set; } // <- Fix! Virtual

        public Rectangle() {

        }

        public Rectangle(int width, int height) {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"{nameof(Width)}: {Width}, {nameof(Height)}: {Height}";
        }
    }

    // Now we want to build a Square
    public class Sqaure: Rectangle {
        // public new int Width {  // <- this is a Problem
        public override int Width {  // <- Fix! Override
            set { base.Width = base.Height = value; }
        }

        // public new int Height { // <- this is a Problem
        public override int Height { // <- Fix! Override
            set { base.Width = base.Height = value; }
        }
    }
}


/*
In main:

using DesignPatterns;

int Area(Rectangle r) {
    return r.Width * r.Height;
}

Rectangle rc = new Rectangle(2, 3);
Console.WriteLine($"{rc} has area {Area(rc)}");

Sqaure sq = new Sqaure();
sq.Width = 4;
Console.WriteLine($"{sq} has area {Area(sq)}"); // 16

// This is a violation of the Liskov Substitution principle
// What if we do?
Rectangle sq2 = new Sqaure();
sq2.Width = 4;
Console.WriteLine($"{sq2} has area {Area(sq2)}"); // 0!!!
*/