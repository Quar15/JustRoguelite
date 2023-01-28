using System.Text;

using JustRoguelite.Devtools.Terminal;

namespace JustRoguelite.Devtools.Editor
{
    // Base class for an input field.
    //
    // Contains the name, value, and validator for the field.
    // Also contains methods for drawing and handling input.
    public abstract class InputField
    {
        public string Name { get; }
        public StringBuilder Value { get; set; } = new();

        // Filter is called on a new input character,
        // returning true if the character is a valid input
        // for the field.
        public Func<char, string, bool> Filter { get; set; } = (char c, string _) => Char.IsAscii(c);

        public Func<string, bool> Validator { get; set; } = (string s) => s.Length > 0;

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

        public virtual bool HandleInput(KeyboardInput input)
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
            else if (Filter((char)input.Key, Value.ToString()))
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

    // Input field that accepts text input.
    public class TextInputField : InputField
    {
        public TextInputField(string name) : base(name) { }

        public override bool HandleInput(KeyboardInput input)
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
            else if (Filter((char)input.Key, Value.ToString()))
            {
                Value.Append((char)input.Key);
                Editor.Redraw();
                return true;
            }
            return false;
        }
    }

    // Input field that accepts numeric input.
    public class NumericInputField : InputField
    {
        bool isFloat = false;
        bool isSigned = false;
        public NumericInputField(string name, bool isFloat = false, bool isSigned = false) : base(name)
        {
            this.isFloat = isFloat;
            this.isSigned = isSigned;
            Filter = (char c, string _) => (Char.IsDigit(c));
        }

        public override bool HandleInput(KeyboardInput input)
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
            else if (isFloat)
            {
                if (input.Key == ConsoleKey.Separator)
                {
                    if (Value.Length > 0 && !Value.ToString().Contains('.'))
                    {
                        Value.Append('.');
                        Editor.Redraw();
                        return true;
                    }
                }
            }
            else if (isSigned)
            {
                if (input.Key == ConsoleKey.Subtract)
                {
                    if (Value.Length == 0)
                    {
                        Value.Append('-');
                        Editor.Redraw();
                        return true;
                    }
                }
            }
            else if (Filter((char)input.Key, Value.ToString()))
            {
                Value.Append((char)input.Key);
                Editor.Redraw();
                return true;
            }
            return false;
        }
    }

    public class MultiInputField : InputField
    {
        public MultiInputField(string name) : base(name)
        {
            Filter = (char c, string _) => Char.IsDigit(c);
        }

        public override bool HandleInput(KeyboardInput input)
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
                if (Value.Length > 0 && !Value.ToString().EndsWith(' '))
                {
                    Value.Append(' ');
                    Editor.Redraw();
                    return true;
                }
            }
            else if (Filter((char)input.Key, Value.ToString()))
            {
                Value.Append((char)input.Key);
                Editor.Redraw();
                return true;
            }
            return false;
        }
    }

    public class EnumInputField : InputField
    {
        Type enumType;
        public EnumInputField(string name, Type enumType) : base(name)
        {
            this.enumType = enumType;
            Filter = (char c, string _) => Char.IsLetter(c);
            Validator = (string s) => Enum.TryParse(enumType, s, true, out _);
        }

        public override void Draw(Screen screen, int x, int y, bool selected)
        {
            var cs = selected ? selectedColors : defaultColors;
            screen.Move(x, y);
            screen.AddString(Name, cs);

            // Draw the enum string values
            screen.AddString(" ( ", cs);
            foreach (var value in Enum.GetValues(enumType))
            {
                screen.AddString(value.ToString(), cs);
                screen.AddChar(' ', cs);
            }
            screen.AddChar(')', cs);

            screen.AddChar(':', cs);
            screen.Move(x, y + 1);
            screen.AddString(Value.ToString(), defaultColors);
        }
    }
}
