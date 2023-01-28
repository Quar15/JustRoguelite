using JustRoguelite.Devtools.Terminal;

namespace JustRoguelite.Devtools.Editor
{
    // Base class for all forms in the editor.
    //
    // Contains the list of input fields, and methods for
    // drawing and interacting with them.
    public abstract class DataForm
    {
        protected virtual string Header { get; set; } = "";
        protected List<InputField> fields = new();
        protected int selected = 0;

        public void AddField(InputField field)
        {
            fields.Add(field);
        }

        public void AddFields(params InputField[] fields)
        {
            this.fields.AddRange(fields);
        }

        public void ClearFields()
        {
            foreach (var field in fields)
            {
                field.Value.Clear();
                Editor.FullRedraw();
            }
        }

        public virtual void SetValues(Dictionary<string, string> values)
        {
            foreach (var field in fields)
            {
                if (values.ContainsKey(field.Name))
                {
                    field.Value.Append(values[field.Name]);
                }
            }
        }

        public virtual Dictionary<string, string>? GetValues()
        {
            Dictionary<string, string> values = new();
            foreach (var field in fields)
            {
                if (!field.Validator(field.Value.ToString()))
                {
                    return null;
                }
                values.Add(field.Name, field.Value.ToString());
            }
            return values;
        }

        public virtual void Draw(Screen screen)
        {
            int y = 0;

            if (Header != "")
            {
                screen.Move(0, y);
                screen.AddString(Header, new ColorScheme(Color.BrightGreen, Color.Black));
                y += 2;
            }

            foreach (var field in fields)
            {
                field.Draw(screen, 0, y, selected == fields.IndexOf(field));
                y += 2;
            }
        }

        public virtual bool HandleInput(KeyboardInput input)
        {
            switch (input.Key)
            {
                case ConsoleKey.UpArrow:
                    selected = (selected - 1 + fields.Count) % fields.Count;
                    Editor.Redraw();
                    return true;
                case ConsoleKey.DownArrow:
                    selected = (selected + 1) % fields.Count;
                    Editor.Redraw();
                    return true;
                case ConsoleKey.Tab:
                    selected = Math.Max(0, Math.Min(fields.Count - 1, selected + (input.IsShift ? -1 : 1)));
                    Editor.Redraw();
                    return true;
                default:
                    return fields[selected].HandleInput(input);
            }
        }
    }
}
