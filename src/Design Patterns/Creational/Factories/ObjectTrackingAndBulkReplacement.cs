using System.Text;

namespace Factories.ObjectTracking {

    public class ObjectTracking {
        public static void Run() {
            var factory = new TrackingThemeFactory();
            var theme1 = factory.CreateTheme(true);
            var theme2 = factory.CreateTheme(false);
            Console.WriteLine(factory.Info);

            var replaceableFactory = new ReplaceableThemeFactory();
            var magicTheme = replaceableFactory.CreateTheme(true);
            Console.WriteLine(magicTheme.Value.BgrColor);

            replaceableFactory.ReplaceTheme(false);
            Console.WriteLine(magicTheme.Value.BgrColor);
        }
    }
    public interface ITheme {
        string TextColor { get; }
        string BgrColor { get; }
    }   

    class LightTheme: ITheme {
        public string TextColor => "Black";
        public string BgrColor => "White";
    }

    class DarkTheme: ITheme {
        public string TextColor => "White";
        public string BgrColor => "Black";
    }

    public class TrackingThemeFactory {
        private readonly List<WeakReference<ITheme>> themes = new();

        public ITheme CreateTheme(bool dark) {
            ITheme theme = dark ? new DarkTheme() : new LightTheme();
            themes.Add(new WeakReference<ITheme>(theme));

            return theme;
        }

        public string Info 
        {
            get 
            {
                var sb = new StringBuilder();

                foreach (var reference in themes)
                {
                    if (reference.TryGetTarget(out var theme)) {
                        bool dark = theme is DarkTheme;

                        sb.Append(dark ? "Dark" : "Light").AppendLine( "themne");
                    }
                }

                return sb.ToString();
            }
        }
    }

    public class ReplaceableThemeFactory {
        private readonly List<WeakReference<Ref<ITheme>>> themes = new();

        private ITheme createThemeImpl(bool dark) {
            return dark ? new DarkTheme() : new LightTheme();
        }

        public Ref<ITheme> CreateTheme(bool dark) {
            var r = new Ref<ITheme>(createThemeImpl(dark));
            themes.Add(new(r));
            return r;
        }

        public void ReplaceTheme(bool dark) {
            foreach (var wr in themes)
            {
                if (wr.TryGetTarget(out var reference)) {
                    reference.Value = createThemeImpl(dark);
                }
            }
        }        
    }

    public class Ref<T> where T: class {
        public T Value { get; set; }

        public Ref(T value) {
            Value = value;
        }
    }
}