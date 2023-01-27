using System;
using System.Collections.Generic;

using JustRoguelite.Utility;
using JustRoguelite.Devtools.Terminal;

namespace JustRoguelite.Devtools.Editor
{
    public enum Mode { Create, Edit, }

    public class Editor
    {
        static Screen screen;
        static Keyboard consoleKeyboard;

        static bool redraw = true;
        static bool fullRedraw = true;
        static bool exit = false;

        static Statusbar status;
        static List<DataForm> forms = new();
        static int selectedForm = 0;
        static internal DataForm SelectedForm => forms[selectedForm];

        static List<Dictionary<string, string>> charData = new();
        static List<Dictionary<string, string>> itemData = new();
        static List<Dictionary<string, string>> skillData = new();

        static List<Dictionary<string, string>> CurrentFormData => selectedForm switch
        {
            0 => charData,
            1 => itemData,
            2 => skillData,
            _ => throw new Exception("Invalid form index"),
        };

        static Mode mode = Mode.Create;
        static internal Mode Mode => mode;

        static int listItemIndex = 0;

        static string prompt = "";
        static int promptCounter = 0;

        static bool started = false;

        static public void Setup(int width, int height)
        {
            screen = new Screen();
            consoleKeyboard = new Keyboard();

            screen.SetScreenSize(width, height);

            status = new Statusbar();
            status.SetHandlers(
                (new KeyboardInput(ConsoleKey.Escape), new HotKeyAction("Quit", (_) => { exit = true; return true; })),
                (new KeyboardInput(ConsoleKey.Enter), new HotKeyAction("Accept", AcceptHandler)),
                (new KeyboardInput(ConsoleKey.A, isAlt: true), new HotKeyAction("Add", CreateModeHandler)),
                (new KeyboardInput(ConsoleKey.E, isAlt: true), new HotKeyAction("Edit", EditModeHandler)),
                (new KeyboardInput(ConsoleKey.RightArrow), new HotKeyAction("Next", ChangeItemHandler)),
                (new KeyboardInput(ConsoleKey.LeftArrow), new HotKeyAction("Prev", ChangeItemHandler)),
                (new KeyboardInput(ConsoleKey.Delete), new HotKeyAction("Delete", DeleteHandler)),
                (new KeyboardInput(ConsoleKey.S, isAlt: true, isShift: true), new HotKeyAction("Save", SaveHandler)),
                (new KeyboardInput(ConsoleKey.C, isShift: true), new HotKeyAction("Character", SelectFormHandler(0))),
                (new KeyboardInput(ConsoleKey.I, isShift: true), new HotKeyAction("Item", SelectFormHandler(1))),
                (new KeyboardInput(ConsoleKey.S, isShift: true), new HotKeyAction("Skill", SelectFormHandler(2)))
            );

            forms = new List<DataForm>() { new CharacterDataForm(), new ItemDataForm(), new SkillDataForm() };

            SaveManager.LoadAllData(out charData, out itemData, out skillData);

            started = true;
        }


        public static void Run()
        {
            if (!started)
            {
                throw new Exception("Editor not started");
            }

            screen.Init();
            exit = false;

            while (!exit)
            {
                while (consoleKeyboard.KeypressQueue.TryDequeue(out KeyboardInput input))
                {
                    if (status.HandleQuickAction(input))
                    {
                        continue;
                    }
                    SelectedForm.HandleInput(input);
                }

                if (fullRedraw) { screen.Clear(); }
                if (redraw || fullRedraw) { Draw(); }

                promptCounter--;
                if (promptCounter <= 0)
                {
                    prompt = "";
                    FullRedraw();
                }

                System.Threading.Thread.Sleep(50);
            }

            SaveManager.SaveAllData(charData, skillData, itemData);
            screen.Exit();
        }

        public static void SetPrompt(string text, int counter = 50)
        {
            prompt = text;
            promptCounter = counter;
            if (promptCounter > 0)
            {
                FullRedraw();
            }
            else
            {
                Redraw();
            }
        }

        public static void Redraw()
        {
            redraw = true;
        }

        public static void FullRedraw()
        {
            fullRedraw = true;
        }

        public static void Draw()
        {
            SelectedForm.Draw(screen);
            DrawPrompt();
            status.Draw(screen);
            screen.UpdateScreen();
            redraw = false;
            fullRedraw = false;
        }

        static void DrawPrompt()
        {
            screen.Move(0, screen.Rows - 3);
            screen.AddString(prompt);
        }

        static Func<KeyboardInput, bool> SelectFormHandler(int formIndex)
        {
            return (_) =>
            {
                selectedForm = formIndex;
                FullRedraw();
                return true;
            };
        }

        static bool AcceptHandler(KeyboardInput input)
        {
            var newData = forms[selectedForm].GetValues();
            if (newData == null || newData.Any(f => f.Value.Length == 0))
            {
                SetPrompt("Invalid data");
                return true;
            }

            if (mode == Mode.Edit)
            {
                CurrentFormData[listItemIndex] = newData;
                listItemIndex = 0;
                mode = Mode.Create;
                SetPrompt("Data updated");
            }
            else
            {
                CurrentFormData.Add(newData);
                SetPrompt("Data added");
            }
            SelectedForm.ClearFields();
            return true;
        }

        static bool EditModeHandler(KeyboardInput input)
        {
            if (mode == Mode.Edit)
            {
                return false;
            }
            listItemIndex = 0;
            if (CurrentFormData.Count == 0)
            {
                SetPrompt("No data to edit");
                return true;
            }

            mode = Mode.Edit;
            SetPrompt($"Edit mode (item {listItemIndex + 1}/{CurrentFormData.Count})");
            SelectedForm.ClearFields();
            SelectedForm.SetValues(CurrentFormData[listItemIndex]);
            return true;
        }

        static bool CreateModeHandler(KeyboardInput input)
        {
            if (mode == Mode.Create)
            {
                return false;
            }
            mode = Mode.Create;
            SelectedForm.ClearFields();
            return true;
        }

        static bool ChangeItemHandler(KeyboardInput input)
        {
            if (mode != Mode.Edit)
            {
                return false;
            }

            if (input.Key == ConsoleKey.RightArrow)
            {
                listItemIndex = (listItemIndex + 1) % CurrentFormData.Count;
            }
            else
            {
                listItemIndex = (listItemIndex - 1 + CurrentFormData.Count) % CurrentFormData.Count;
            }

            SetPrompt($"Edit mode (item {listItemIndex + 1}/{CurrentFormData.Count})");
            SelectedForm.ClearFields();
            SelectedForm.SetValues(CurrentFormData[listItemIndex]);
            return true;
        }

        static bool DeleteHandler(KeyboardInput input)
        {
            if (mode != Mode.Edit)
            {
                return false;
            }
            CurrentFormData.RemoveAt(listItemIndex);
            SetPrompt("Data deleted");
            listItemIndex = Math.Max(0, listItemIndex - 1);
            if (CurrentFormData.Count == 0)
            {
                mode = Mode.Create;
                SelectedForm.ClearFields();
                return true;
            }
            SelectedForm.ClearFields();
            SelectedForm.SetValues(CurrentFormData[listItemIndex]);
            return true;
        }

        static bool SaveHandler(KeyboardInput input)
        {
            SaveManager.SaveAllData(charData, skillData, itemData);
            SetPrompt("Data saved");
            return true;
        }
    }
}
