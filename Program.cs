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
        public const LogType LOG_TYPE = LogType.FILE;
        public const uint MAX_LOGS_N = 100;
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

            SaveManager.SaveCharacters(cdList);


            /* Test ItemDataList save */
            ItemData item = new(1, "Test1", "Test Description", 25, ItemType.EQUIPABLE);
            ItemData item2 = new(2, "Test1", "Test Description", 25, ItemType.QUEST);

            ItemDataList items = new ItemDataList();
            items.Add(item);
            items.Add(item2);

            SaveManager.SaveItems(items);

            /* Test SkillDataList save */
            SkillData skill = new(1, "Test1", "Test Description", new() { 1 }, DamageType.PHYSICAL, SkillType.ATTACK);
            SkillData skill2 = new(2, "Test1", "Test Description", new() { 1, 3, 1 }, DamageType.MAGIC, SkillType.SPECIAL);

            SkillDataList skills = new SkillDataList();
            skills.Add(skill);
            skills.Add(skill2);

            SaveManager.SaveSkills(skills);

            // Devtools 
            Editor.Setup(128, 30);
            Editor.Run();


            Logger.Instance().Info("Program END", "Program.Main()");
            Logger.Instance().SaveLogs();
        }
    }
}