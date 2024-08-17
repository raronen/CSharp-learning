using System.Text;

// A builder is a separate component for building an object
// Can either give a builder a constructor or return it via a static function
// To make builder fluent, return this
// Different facets of an object can be built with different builders working in tandem via a base class

namespace DesignPatterns {

    public class HTMLElement {
        public string Name, Text;
        public List<HTMLElement> Elements = new List<HTMLElement>();
        private const int indentSize = 2;

        public HTMLElement() {}

        public HTMLElement(string name, string text) {

            Elements.ForEach(e => e.Name = name);
            Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
            Text = text ?? throw new ArgumentNullException(paramName: nameof(text));
        }

        private string ToStringImpl(int indent) {
            var sb = new StringBuilder();
            var i = new string(' ', indentSize * indent);
            sb.Append($"{i}<${Name}>\n");

            if (!string.IsNullOrWhiteSpace(Text))
            {
                sb.Append(new string(' ', indentSize * (indent + 1)));
                sb.AppendLine(Text);
            }

            foreach (var elm in Elements)
            {
                sb.Append(elm.ToStringImpl(indent + 1));
            }
            sb.Append($"{i}</{Name}>\n");

            return sb.ToString();
        }

        public override string ToString()
        {
            return ToStringImpl(0);
        }
    }

    public class HTMLBuilder {
        HTMLElement root = new HTMLElement();
        private string rootName;

        public HTMLBuilder(string rootName) {
            this.rootName = rootName;
            root.Name = rootName;
        }

        public void AddChild(string childName, string childText) {
            var e = new HTMLElement(childName, childText);
            root.Elements.Add(e);
        }

        public override string ToString()
        {
            return root.ToString();
        }

        public void Clear() {
            root = new HTMLElement{Name = rootName};
        }
    }

    class Builder {

        // Good Way
        public static void LifeWithBuilderPattern() {
            var builder = new HTMLBuilder("ul");
            builder.AddChild("li", "hello");
            builder.AddChild("li", "world");

            Console.WriteLine(builder.ToString());
        }

        // Bad Way
        // Life without builder pattern - can't build an html properly, and it's not reusable, and it's not readable, and it's not maintainable
        public static void LifeWithoutBuilderPattern() {
            var sb = new StringBuilder();

            var words = new[] {"hello", "world"};
            sb.Clear();
            sb.Append("<ul>");
            foreach (var word in words)
            {
                sb.AppendFormat("<li>{0}</li>", word);
            }
            sb.Append("</ul>");
            Console.WriteLine(sb);

        }
    }
}