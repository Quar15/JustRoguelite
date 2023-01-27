using System.Text;

using JustRoguelite.Devtools.Terminal;

namespace JustRoguelite.Devtools.Editor
{
    public abstract class InputField
    {
        public string Name { get; }
        public StringBuilder Value { get; set; } = new();

        public Func<char, string, bool> Validator { get; set; } = (char c, string _) => Char.IsAscii(c);

        public static ColorScheme defaultColors = new(Color.White, Color.Black);
        public static ColorScheme selectedColors = new(Color.BrightWhite, Color.BrightBlack);

        public InputField(string name)
        {
            Name = name;
        }

        public InputField(string name, string value) : this(name)
        {
            Value.Append(value);
        }

        public bool HandleInput(KeyboardInput input)
        {
            if (input.Key == ConsoleKey.Backspace)
            {
                if (Value.Length > 0)
                {
                    Value.Remove(Value.Length - 1, 1);
                    Editor.FullRedraw();
                    return true;
                }
            }
            else if (input.Key == ConsoleKey.Spacebar)
            {
                Value.Append(' ');
                Editor.Redraw();
                return true;
            }
            else if (Validator((char)input.Key, Value.ToString()))
            {
                Value.Append((char)input.Key);
                Editor.Redraw();
                return true;
            }
            return false;
        }

        public virtual void Draw(Screen screen, int x, int y, bool selected)
        {
            var cs = selected ? selectedColors : defaultColors;
            screen.Move(x, y);
            screen.AddString(Name, cs);
            screen.AddChar(':', cs);
            screen.Move(x, y + 1);
            screen.AddString(Value.ToString(), defaultColors);
        }
    }

    public class TextInputField : InputField
    {
        public TextInputField(string name) : base(name) { }
    }

    public class NumericInputField : InputField
    {
        public NumericInputField(string name) : base(name)
        {
            Validator = (char c, string _) =>
                Char.IsDigit(c) ||
                (c == '-' && Value.Length == 0) ||
                (c == '.' && !Value.ToString().Contains('.') && Value.Length > 0);
        }
    }
}
