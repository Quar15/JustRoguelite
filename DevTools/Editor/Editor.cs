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
    public enum Mode { Create, Edit, Search }

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

        static Prompt prompt = new();
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
                (new KeyboardInput(ConsoleKey.S, isAlt: true), new HotKeyAction("Search", SearchModeHandler)),
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
                    // Then, check if current prompt is active
                    // and handle the input if it is.
                    if (prompt.HandleInput(input))
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
                prompt.Tick();

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

        // public static void SetPrompt(string text, int counter = 50)
        // {
        //     prompt
        // }

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
            prompt.Draw(screen);
            status.Draw(screen);
            screen.UpdateScreen();
            redraw = false;
            fullRedraw = false;
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
            if (mode == Mode.Search)
            {
                SearchPrompt searchPrompt = (SearchPrompt)prompt;
                var searchResult = CurrentFormData.FindIndex(x => x["Id"] == searchPrompt.Input);

                prompt = new Prompt();
                if (searchResult != -1)
                {
                    listItemIndex = searchResult;
                    SelectedForm.SetValues(CurrentFormData[listItemIndex]);
                    mode = Mode.Edit;
                    prompt.SetPrompt("Data found", 50);
                }
                else
                {
                    prompt.SetPrompt("Data not found", 50);
                    mode = Mode.Create;
                }
                FullRedraw();
                return true;
            }

            var newData = forms[selectedForm].GetValues();
            if (newData == null)
            {
                prompt.SetPrompt("Invalid data", 50);
                return true;
            }

            if (mode == Mode.Edit)
            {
                newData["Id"] = CurrentFormData[listItemIndex]["Id"];
                CurrentFormData[listItemIndex] = newData;
                listItemIndex = 0;
                mode = Mode.Create;
                prompt.SetPrompt("Data edited", 50);
            }
            else
            {
                Console.Write(CurrentFormData.Count);

                var nextId = CurrentFormData.Count > 0 ? CurrentFormData.Max(x => uint.Parse(x["Id"])) + 1 : 0;
                newData["Id"] = nextId.ToString();
                CurrentFormData.Add(newData);
                prompt.SetPrompt($"Data added (ID {nextId})", 50);
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
                prompt.SetPrompt("No data to edit", 50);
                return true;
            }

            mode = Mode.Edit;
            prompt.SetPrompt($"Edit mode (item {listItemIndex + 1}/{CurrentFormData.Count})", 50);
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
            prompt.SetPrompt("Create mode", 50);
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

            prompt.SetPrompt($"Edit mode (item {listItemIndex + 1}/{CurrentFormData.Count})", 50);
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
            prompt.SetPrompt("Data deleted", 50);
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

        static bool SearchModeHandler(KeyboardInput input)
        {
            SelectedForm.ClearFields();
            mode = Mode.Search;
            prompt = new SearchPrompt();
            prompt.SetPrompt("Search by id: ");
            return true;
        }

        static bool SaveHandler(KeyboardInput input)
        {
            SaveManager.SaveAllData(baseStatsData, charData, skillData, itemData);
            prompt.SetPrompt("Data saved", 50);
            return true;
        }

        // In case of crash, this method will be called to restore the terminal
        ~Editor()
        {
            screen.Exit();
        }
    }
}
