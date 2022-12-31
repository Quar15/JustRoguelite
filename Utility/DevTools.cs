using JustRoguelite.Characters;
using JustRoguelite.Skills;
using JustRoguelite.Items;

namespace JustRoguelite.Utility
{
    static class DevTools
    {

        public static void WriteUserCmdPrompt() 
        {
            Console.Write(" > ");
        }

        public static void Create(CharactersList characters, List<SkillList> skills, Inventory inventory, string[] args) 
        {
            if (args.Length < 2) return;

            switch (args[1])
            {
                case "c":
                    Logger.Instance().Info("Create Character", "DevTools.Create(...)");
                    characters.Add(CreateCharacter());
                    break;

                case "s":
                    Logger.Instance().Info("Create Skill", "DevTools.Create(...)");
                    CreateSkill(); // @TODO: rethink skills lists
                    break;

                case "i":
                    Logger.Instance().Info("Create Item", "DevTools.Create(...)");
                    Item item = CreateItem();
                    PrettyPrint(item);
                    inventory.Add(item);
                    break;

                default:
                    break;
            }
        }

        public static CharacterBase CreateCharacter() 
        {
            CharacterBase character;

            character = new("Test", "Character Description", new());

            return character;
        }

        public static Skill CreateSkill() 
        {
            Skill skill;

            skill = new();

            return skill;
        }

        public static Item CreateItem()
        {
            Item item;

            Console.Write("> Item Name: ");
            string? name = Console.ReadLine();
            Console.Write("> Item Description: ");
            string? description = Console.ReadLine();

            item = new(name == null ? "Item" : name, description == null ? "" : description);

            return item;
        }

        public static void Find(CharactersList characters, List<SkillList> skills, Inventory inventory, string[] args) 
        {
            int id = -1;
            if(!int.TryParse(args[2], out id)) return;

            string? arg = null;
            if(args.Length >= 4) { arg = args[3]; }

            switch (args[1])
            {
                case "c": // [C]haracter
                    Logger.Instance().Info("Find Character", "DevTools.Find(...)");
                    if (characters == null) break;

                    CharacterBase? character = characters.GetItem(id);
                    if (character == null) break;

                    HandleFind(characters, character, arg);
                    break;

                case "s": // [S]kill
                    Logger.Instance().Info("Find Skill", "DevTools.Find(...)");
                    if (skills == null) break;

                    HandleFind(skills, id, arg);

                    break;

                case "i": // [I]tem
                    Logger.Instance().Info("Find Item", "DevTools.Find(...)");
                    if (inventory == null) break;

                    Items.Item? item = inventory.GetItem(id);
                    if (item == null)
                    {
                        Logger.Instance().Warning("Item NOT found", "DevTools.Find(...)");
                        break;
                    }

                    HandleFind(inventory, item, arg);
                    break;

                default:
                    break;
            }
        }

        private static void HandleFindInput(out string? cmd) 
        {
            string? user_input = null;
            while(user_input == null) 
            {
                Console.WriteLine("{ [P]rint | [E]dit | [D]elete }");
                WriteUserCmdPrompt();
                user_input = Console.ReadLine();
            }

            cmd = user_input;
        }

        public static void HandleFind(List<SkillList> skills, int id, string? arg = null) 
        {
            SkillList? skillList = null;
            Skill? skill = null;

            foreach (SkillList sl in skills)
            {
                skill = sl.GetItem(id);
                if (skill != null)
                {
                    skillList = sl;
                    break;
                }
            }

            if (skillList == null || skill == null) return;
            HandleFind(skillList, skill, arg);
        }

        public static void HandleFind(SkillList skillList, Skill skill, string? arg = null) 
        {
            Logger.Instance().Info("Handle Find Skill", "DevTools.HandleFind(...)");

            if(arg == null)
                HandleFindInput(out arg);

            switch (arg) 
            {
                case "p": // [P]rint
                    Logger.Instance().Info("Print Skill", "DevTools.HandleFind(...)");
                    PrettyPrint(skill);
                    break;

                case "e": // [E]dit
                    Logger.Instance().Info("Edit Skill", "DevTools.HandleFind(...)");
                    break;

                case "d": // [D]elete
                    Logger.Instance().Info("Delete Skill", "DevTools.HandleFind(...)");
                    skillList.Remove(skill);
                    break;

                default:
                    Console.WriteLine("Wrong option - available options { [P]rint | [E]dit | [D]elete }");
                    break;
            }
        }

        public static void HandleFind(CharactersList characters, CharacterBase character, string? arg = null)
        {
            Logger.Instance().Info("Handle Find Character", "DevTools.HandleFind(...)");

            if (arg == null)
                HandleFindInput(out arg);

            switch (arg)
            {
                case "p": // [P]rint
                    Logger.Instance().Info("Print Character", "DevTools.HandleFind(...)");
                    PrettyPrint(character);
                    break;

                case "e": // [E]dit
                    Logger.Instance().Info("Edit Character", "DevTools.HandleFind(...)");
                    break;

                case "d": // [D]elete
                    Logger.Instance().Info("Delete Character", "DevTools.HandleFind(...)");
                    characters.Remove(character);
                    break;

                default:
                    Console.WriteLine("Wrong option - available options { [P]rint | [E]dit | [D]elete }");
                    break;
            }
        }

        public static void HandleFind(Inventory inventory, Item item, string? arg = null)
        {
            Logger.Instance().Info("Handle Find Item", "DevTools.HandleFind(...)");

            if (arg == null)
                HandleFindInput(out arg);

            switch (arg)
            {
                case "p": // [P]rint
                    Logger.Instance().Info("Print Item", "DevTools.HandleFind(...)");
                    PrettyPrint(item);
                    break;

                case "e": // [E]dit
                    Logger.Instance().Info("Edit Item", "DevTools.HandleFind(...)");
                    break;

                case "d": // [D]elete
                    Logger.Instance().Info("Delete Item", "DevTools.HandleFind(...)");
                    inventory.Remove(item);
                    break;

                default:
                    Console.WriteLine("Wrong option - available options { [P]rint | [E]dit | [D]elete }");
                    break;
            }
        }

        public static void Loop(CharactersList characters, List<SkillList> skills, Inventory inventory)
        {
            bool looping = true;
            while (looping) 
            {
                Console.WriteLine("{ [N]ew | [F]ind | [Q]uit } { [C]haracter | [S]kill | [I]tem } [ID] [ [P]rint | [E]dit | [D]elete ]");
                WriteUserCmdPrompt();
                string? user_input = Console.ReadLine();
                if (user_input == null) continue;

                user_input = user_input.ToLower();
                string[] cmd = user_input.Split(" ");

                switch (cmd[0])
                {
                    case "n":
                        // new
                        Logger.Instance().Info("Create object", "DevTools.Loop()");
                        Create(characters, skills, inventory, cmd);
                        break;

                    case "f":
                        // find
                        if (cmd.Length < 3) 
                        {
                            Console.WriteLine("Find command needs 2 arguments (type of object, id)");
                            break;
                        }

                        Logger.Instance().Info("Find object", "DevTools.Loop()");
                        Find(characters, skills, inventory, cmd);
                        break;

                    case "q":
                        // quit
                        Logger.Instance().Info("Quit Dev Tools", "DevTools.Loop()");
                        looping = false;
                        break;

                    default:
                        Console.WriteLine("Wrong option - available options {  }");
                        break;
                }
            }
        }

        public static void PrettyPrint(CharacterBase character) 
        {
            character.DebugLog();
        }

        public static void PrettyPrint(IEnumerable<CharacterBase> characters) 
        {
            foreach (CharacterBase character in characters)
                PrettyPrint(character);
        }

        public static void PrettyPrint(Skill skill) 
        {
            skill.DebugLog();
        }

        public static void PrettyPrint(IEnumerable<Skill> skills)
        {
            foreach(Skill skill in skills)
                PrettyPrint(skill);
        }

        public static void PrettyPrint(Item item) 
        {
            item.DebugLog();
        }

        public static void PrettyPrint(Inventory inventory)
        {
            Item[] items = inventory.GetAll();
            foreach (Item item in items)
                PrettyPrint(item);
        }
    }
}
