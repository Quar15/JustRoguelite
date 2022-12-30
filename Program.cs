using System;
using JustRoguelite.Utility;
using JustRoguelite.Characters;
using JustRoguelite.Skills;
using JustRoguelite.Items;


namespace JustRoguelite
{
    public static class Globals 
    {
        public const bool DEBUG_LOGS = true;
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            Logger.Instance().Info("Program START", "Program.Main()");

            CharactersList charactersList;
            List<SkillList> fullSkillsLists;
            Inventory allItemsInventory;
            SaveManager.LoadData(out charactersList, out fullSkillsLists, out allItemsInventory);

            // Main Menu handling
            DevTools.Loop(charactersList, fullSkillsLists, allItemsInventory);


            Logger.Instance().Info("Program END", "Program.Main()");
        }
    }
}