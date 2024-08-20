using System.Text;
using System.Xml;

namespace Flyweight.TextFormatting {

    public class Demo
    {
        public static void Run()
        {
            // Save another array just to understand if every c is capitialize or not.
            var ft = new FormattedText("This is a brave new world");
            ft.Capitalize(10, 15);
            Console.WriteLine(ft);

            // Save Range object and computes if c if upper on runtime
            var bft = new BetterFormattedText("This is a brave new world");
            bft.GetRange(10, 15).Capitalize = true;

            Console.WriteLine(bft);

        }
    }

    public class FormattedText {
        private string plainText;
        private bool[] caps; // Bad implementation - Lot of memory!
        public FormattedText(string plainText) {
            this.plainText = plainText;
            this.caps = new bool[plainText.Length];
        }

        public void Capitalize(int start, int end) {
            for (int i = start; i <= end; i++) {
                caps[i] = true;
            }
        }

        public override string ToString() {
            var sb = new StringBuilder();
            for (int i = 0; i < plainText.Length; i++) {
                var c = plainText[i];
                sb.Append(caps[i] ? char.ToUpper(c) : c);
            }
            return sb.ToString();
        }
    }

    public class BetterFormattedText
    {
        private string plainText;
        // Saving cache of formating
        private List<TextRange> formatting = new List<TextRange>();

        public BetterFormattedText(string plainText)
        {
            this.plainText = plainText;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < plainText.Length; i++)
            {
                var c = plainText[i];

                foreach (var range in formatting)
                {
                    if (range.Covers(i) && range.Capitalize)
                    {
                        c = char.ToUpper(c);
                    }
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        public TextRange GetRange(int start, int end)
        {
            {
                var range = new TextRange { Start = start, End = end };
                formatting.Add(range);
                return range;
            }
        }

        public class TextRange
        {
            public int Start, End;
            public bool Capitalize, Bold, Italic;
            public bool Covers(int position)
            {
                return position >= Start && position <= End;
            }

        }
    }
}