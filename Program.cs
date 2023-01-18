using System;
using JustRoguelite.Utility;
using JustRoguelite.Characters;
using JustRoguelite.Skills;
using JustRoguelite.Items;


namespace JustRoguelite
{
    public static class Globals
    {
        public const LogType LOG_TYPE = LogType.CONSOLE;
    }


    internal class Program
    {
        static void HandleMainMenuLoop()
        {

        }

        static void Main(string[] args)
        {
            Logger.Instance().Info("Program START", "Program.Main()");

            CharactersList charactersList;
            List<SkillList> fullSkillsLists;
            Inventory allItemsInventory;
            SaveManager.LoadAll(out charactersList, out fullSkillsLists, out allItemsInventory);

            HandleMainMenuLoop();

            DevTools.Loop(charactersList, fullSkillsLists, allItemsInventory);


            Logger.Instance().Info("Program END", "Program.Main()");
        }
    }
}