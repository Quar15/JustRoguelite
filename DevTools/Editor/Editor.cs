using System;
using System.Collections.Generic;

using JustRoguelite.Utility;
using JustRoguelite.Devtools.Terminal;

namespace JustRoguelite.Devtools.Editor
{
    // Currently, the editor supports two modes:
    // - `Create` - for creating new data
    // - `Edit` - for editing existing data
    //
    // Deletion is supported in `Edit` mode
    // (the currently selected item is deleted when the `Delete` key is pressed)
    public enum Mode { Create, Edit, }

    // Main `Editor` class
    // 
    // This class is responsible for the main loop of the editor.
    // It also contains the lists of data for game structures,
    // and the list of forms for editing them.
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
        static List<Dictionary<string, string>> baseStatsData = new();

        static List<Dictionary<string, string>> CurrentFormData => selectedForm switch
        {
            0 => charData,
            1 => itemData,
            2 => skillData,
            3 => baseStatsData,
            _ => throw new Exception("Invalid form index"),
        };

        static Mode mode = Mode.Create;
        static internal Mode Mode => mode;

        static int listItemIndex = 0;

        static string prompt = "";
        static int promptCounter = 0;

        static bool started = false;

        // Should be called before `Run()`.
        // This method initializes the editor:
        // - creates the screen and sets its dimensions
        // - starts the keyboard input thread
        // - creates the statusbar and sets its hotkeys
        // - creates the list of forms for editing data
        // - loads the data from the files
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
                (new KeyboardInput(ConsoleKey.S, isShift: true), new HotKeyAction("Skill", SelectFormHandler(2))),
                (new KeyboardInput(ConsoleKey.B, isShift: true), new HotKeyAction("Base Stats", SelectFormHandler(3)))
            );

            forms = new List<DataForm>() { new CharacterDataForm(), new ItemDataForm(), new SkillDataForm(), new BaseStatsDataForm() };

            SaveManager.LoadAllData(out baseStatsData, out charData, out skillData, out itemData);

            started = true;
        }

        // Main loop of the editor.
        // This method is responsible for:
        // - handling keyboard input
        // - drawing the screen
        // - saving the data to the files on exit
        public static void Run()
        {
            if (!started)
            {
                throw new Exception("Editor not started");
            }

            // Initializes the underlying terminal driver
            screen.Init();
            exit = false;

            while (!exit)
            {
                // Handle keyboard input
                while (consoleKeyboard.KeypressQueue.TryDequeue(out KeyboardInput input))
                {
                    // First, if one of the inputs maps to a hotkey
                    // in the statusbar, handle it.
                    if (status.HandleQuickAction(input))
                    {
                        continue;
                    }
                    // Otherwise, pass the input to the current form.
                    SelectedForm.HandleInput(input);
                }

                // If a full redraw is needed, clear the screen
                if (fullRedraw) { screen.Clear(); }
                // and update it
                if (redraw || fullRedraw) { Draw(); }

                // If a prompt is set, decrease its counter
                promptCounter = Math.Max(0, promptCounter - 1);
                // and if it's time to remove it, do so
                if (promptCounter <= 0)
                {
                    prompt = "";
                    FullRedraw();
                }

                // Wait a bit before the next iteration
                // (this is to prevent the CPU from being overloaded)
                // no input is lost, because the keyboard input is handled
                // in a separate thread
                System.Threading.Thread.Sleep(50);
            }

            // Since the editor is exiting, save the data
            SaveManager.SaveAllData(baseStatsData, charData, skillData, itemData);
            // and restore the terminal to its original state
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

        // Status bar handlers

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
            if (newData == null)
            {
                SetPrompt("Invalid data");
                return true;
            }

            if (mode == Mode.Edit)
            {
                newData["Id"] = CurrentFormData[listItemIndex]["Id"];
                CurrentFormData[listItemIndex] = newData;
                listItemIndex = 0;
                mode = Mode.Create;
                SetPrompt("Data updated");
            }
            else
            {
                Console.Write(CurrentFormData.Count);

                var nextId = CurrentFormData.Count > 0 ? CurrentFormData.Max(x => uint.Parse(x["Id"])) + 1 : 0;
                newData["Id"] = nextId.ToString();
                CurrentFormData.Add(newData);
                SetPrompt($"Data added (ID {nextId})");
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
            SetPrompt("Create mode");
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
            SaveManager.SaveAllData(baseStatsData, charData, skillData, itemData);
            SetPrompt("Data saved");
            return true;
        }

        // In case of crash, this method will be called to restore the terminal
        ~Editor()
        {
            screen.Exit();
        }
    }
}
