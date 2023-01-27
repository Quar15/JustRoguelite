using System;
using System.Text;
using System.Collections.Generic;

using JustRoguelite.Devtools.Terminal;

namespace JustRoguelite.Devtools.Editor
{
    public struct HotKeyAction
    {
        public string name;
        public Func<KeyboardInput, bool> handler;

        public HotKeyAction(string name, Func<KeyboardInput, bool> handler)
        {
            this.name = name;
            this.handler = handler;
        }
    }

    public class Statusbar
    {
        Dictionary<KeyboardInput, HotKeyAction> hotkeys = new();

        static ColorScheme createColors = new(Color.Black, Color.BrightGreen);
        static ColorScheme editColors = new(Color.Black, Color.BrightRed);

        // public Mode mode = Mode.Create;

        StringBuilder sb = new();

        public void SetHandler(KeyboardInput input, HotKeyAction action)
        {
            hotkeys.Add(input, action);
        }

        public void SetHandlers(params (KeyboardInput, HotKeyAction)[] actions)
        {
            foreach (var (input, action) in actions)
            {
                hotkeys.Add(input, action);
            }
        }

        public void ClearHandlers()
        {
            hotkeys.Clear();
        }

        public bool HandleQuickAction(KeyboardInput input)
        {
            if (hotkeys.TryGetValue(input, out HotKeyAction action))
            {
                return action.handler(input);
            }
            return false;
        }

        public void Draw(Screen screen)
        {
            sb.Clear();
            var cs = Editor.Mode == Mode.Create ? createColors : editColors;
            foreach (var (input, action) in hotkeys)
            {
                var keymap = $"{action.name}: {input}  ";
                if (sb.Length + keymap.Length >= screen.Cols)
                {
                    screen.Move(0, screen.Rows - 2);
                    screen.AddString(sb.ToString().PadRight(screen.Cols), cs);
                    sb.Clear();

                }
                sb.Append(keymap);
            }

            int right = screen.Cols - Editor.Mode.ToString().Length - 2;
            if (sb.Length < screen.Cols - right)
            {
                sb.Append(' ', screen.Cols - sb.Length - right);
            }

            sb.Append($"[{Editor.Mode}]");

            screen.Move(0, screen.Rows - 1);
            screen.AddString(sb.ToString(), cs);
        }
    }
}
