using System;
using System.Text;
using System.Collections.Generic;

using JustRoguelite.Devtools.Terminal;

namespace JustRoguelite.Devtools.Editor
{
    // Basic structure containing a display name for an action,
    // and a handler function for said action.
    public struct HotKeyAction
    {
        public string name;

        // The handler function is passed the KeyboardInput that
        // triggered the action, and returns true if the action
        // was handled, and false if it was not (allowing for differenciating
        // whether to move on, or further propagate the input).
        public Func<KeyboardInput, bool> handler;

        public HotKeyAction(string name, Func<KeyboardInput, bool> handler)
        {
            this.name = name;
            this.handler = handler;
        }
    }

    // The status bar is a small bar at the bottom of the screen
    // that displays the current mode, and a list of quick actions
    // that can be performed.
    public class Statusbar
    {
        // The main mapping of KeyboardInputs to Possible actions
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
                if (sb.Length + keymap.Length - 2 > screen.Cols)
                {
                    screen.Move(0, screen.Rows - 2);
                    screen.AddString(sb.ToString().PadRight(screen.Cols), cs);
                    sb.Clear();

                }
                sb.Append(keymap);
            }

            int right = screen.Cols - Editor.Mode.ToString().Length - 2;
            if (sb.Length < right)
            {
                sb.Append(' ', right - sb.Length);
            }

            sb.Append($"[{Editor.Mode}]");

            screen.Move(0, screen.Rows - 1);
            screen.AddString(sb.ToString(), cs);
        }
    }
}
