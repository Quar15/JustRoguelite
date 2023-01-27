namespace JustRoguelite.Devtools.Terminal
{
    // A struct representing an ANSI color scheme.
    //
    // Contains a foreground and background color.
    // Also contains methods for swapping the colors,
    // and for creating a default color scheme.
    // Can be implicitly converted to and from an int.
    public struct ColorScheme
    {
        public Color Foreground { get; }
        public Color Background { get; }

        public ColorScheme(Color fg, Color bg)
        {
            Foreground = fg;
            Background = bg;
        }

        public ColorScheme(Color fg) : this(fg, Color.Black) { }

        public ColorScheme Swapped()
        {
            return new ColorScheme(Background, Foreground);
        }

        public static ColorScheme Default => new ColorScheme(Color.White, Color.Black);

        public static implicit operator ColorScheme(int v) =>
            new ColorScheme((Color)((v & 0xFF00) >> 8), (Color)(v & 0x00FF));

        public static implicit operator int(ColorScheme a) => ((int)a.Foreground << 8) | (int)a.Background;

    }

    // ANSI escape codes for colors
    public enum Color : byte
    {
        Black = 30,
        Red = 31,
        Green = 32,
        Yellow = 33,
        Blue = 34,
        Magenta = 35,
        Cyan = 36,
        White = 37,
        BrightBlack = 90,
        BrightRed = 91,
        BrightGreen = 92,
        BrightYellow = 93,
        BrightBlue = 94,
        BrightMagenta = 95,
        BrightCyan = 96,
        BrightWhite = 97,
    }
}
