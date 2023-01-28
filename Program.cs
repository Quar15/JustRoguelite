using System;
using JustRoguelite.Utility;
using JustRoguelite.Characters;
using JustRoguelite.Skills;
using JustRoguelite.Items;
using JustRoguelite.Devtools.Editor;


namespace JustRoguelite
{
    public static class Globals
    {
        public const LogType LOG_TYPE = LogType.CONSOLE;
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            Logger.Instance().Info("Program START", "Program.Main()");

            /* Test CharacterDataList save */
            CharacterData character1 = new(1, "Goblin", "Basic Goblin", new(), CharacterType.ENEMY);
            CharacterData character2 = new(2, "Goblin2", "Basic Goblin2", new(), CharacterType.ENEMY);

            CharacterDataList cdList = new CharacterDataList();
            cdList.Add(character1);
            cdList.Add(character2);

            // SaveManager.SaveCharacters(cdList);

            /* Test CharacterList save */
            CharacterEnemy e = new(character1);
            CharacterEnemy e2 = new(character2);

            CharactersList list = new CharactersList();
            list.Add(e);
            list.Add(e2);

            SaveManager.SaveCharacters(list);

            /* Test inventory save */
            Item item = new("Test1", "Test Description", 25, ItemType.EQUIPABLE);
            Item item2 = new("Test1", "Test Description", 25, ItemType.QUEST);

            Inventory inventory = new Inventory();
            inventory.Add(item);
            inventory.Add(item2);

            SaveManager.SaveItems(inventory);

            /* Test skillList save */
            Skill skill = new("Test1", "Test Description", DamageType.PHYSICAL);
            Skill skill2 = new("Test1", "Test Description", DamageType.MAGIC);

            SkillList skills = new SkillList(SkillType.ATTACK);
            inventory.Add(item);
            inventory.Add(item2);

            List<SkillList> sList = new List<SkillList>();
            sList.Add(skills);

            SaveManager.SaveSkills(sList);

            // Devtools 
            Editor.Setup(128, 30);
            Editor.Run();


            Logger.Instance().Info("Program END", "Program.Main()");
        }
    }
}