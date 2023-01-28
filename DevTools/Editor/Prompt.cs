using System;
using System.Text;

using JustRoguelite.Devtools.Terminal;

namespace JustRoguelite.Devtools.Editor
{
    public class Prompt
    {
        string prompt = "";
        uint promptCounter = 0;

        public Prompt(string prompt = "")
        {
            this.prompt = prompt;
        }

        public void SetPrompt(string prompt, uint promptCounter = 50)
        {
            this.prompt = prompt;
            this.promptCounter = promptCounter;
        }

        public virtual void Tick()
        {
            if (promptCounter > 0)
            {
                promptCounter--;
            }
            if (promptCounter == 0)
            {
                Editor.FullRedraw();
            }
        }

        public virtual bool HandleInput(KeyboardInput input)
        {
            return false;
        }

        public virtual void Draw(Screen screen)
        {
            if (promptCounter > 0)
            {
                screen.Move(0, screen.Rows - 3);
                screen.AddString(prompt, ColorScheme.Default);
            }
        }
    }

    public class SearchPrompt : Prompt
    {
        StringBuilder promptInput = new();

        public string Input => promptInput.ToString();

        public override void Tick()
        {
            return;
        }

        public override bool HandleInput(KeyboardInput input)
        {
            if (input.Key == ConsoleKey.Backspace)
            {
                if (promptInput.Length > 0)
                {
                    promptInput.Remove(promptInput.Length - 1, 1);
                    Editor.FullRedraw();
                    return true;
                }
            }
            else if (Char.IsNumber((char)input.Key))
            {
                promptInput.Append((char)input.Key);
                Editor.Redraw();
                return true;
            }
            return false;
        }

        public override void Draw(Screen screen)
        {
            screen.Move(0, screen.Rows - 3);
            screen.AddString("Search by ID: ", ColorScheme.Default);
            screen.AddString(promptInput.ToString(), ColorScheme.Default);
        }
    }
}