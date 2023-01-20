using JustRoguelite.Characters;
using JustRoguelite.Skills;
using JustRoguelite.Items;

using System.Text.Json;
// using System.IO;

namespace JustRoguelite.Utility
{
    internal class SaveManager
    {
        const string DATA_PATH = "Data/";
        const string CHARACTERS_PATH = DATA_PATH + "Characters.json";
        const string SKILLS_PATH = DATA_PATH + "Skills.json";
        const string ITEMS_PATH = DATA_PATH + "Items.json";

        private static bool _dataPathExists = Directory.Exists(DATA_PATH);

        private static void CreateDataPath()
        {
            if (!_dataPathExists)
            {
                Directory.CreateDirectory(DATA_PATH);
                _dataPathExists = true;
            }
        }

        internal static CharactersList LoadCharacters()
        {
            try
            {
                string json = File.ReadAllText(CHARACTERS_PATH);
                List<Dictionary<string, string>> charList = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json)!;

                return new CharactersList(charList.Select(c =>
                {
                    CharacterType type = (CharacterType)Enum.Parse(typeof(CharacterType), c["Type"]);
                    return type switch
                    {
                        CharacterType.PLAYER => new CharacterPlayer(c),
                        CharacterType.ENEMY => new CharacterEnemy(c),
                        CharacterType.NEUTRAL => new CharacterNeutral(c),
                        _ => new CharacterBase(c),
                    };
                }).ToList());
            }
            catch (DirectoryNotFoundException)
            {
                CreateDataPath();
                return new CharactersList();
            }
            catch (FileNotFoundException)
            {
                return new CharactersList();
            }
        }

        public static void SaveCharacters(CharactersList characters)
        {
            CreateDataPath();

            var charList = characters.GetAll().ToList().Select(c => c.ToDict()).ToList();

            string json = JsonSerializer.Serialize(characters);
            File.WriteAllText(CHARACTERS_PATH, json);
        }

        internal static List<SkillList> LoadSkills()
        {
            try
            {
                string json = File.ReadAllText(SKILLS_PATH);
                List<(SkillType, Skill[])> skillList = JsonSerializer.Deserialize<List<(SkillType, Skill[])>>(json)!;
                return skillList.Select(s => new SkillList(s.Item1, s.Item2.ToList())).ToList();
            }
            catch (DirectoryNotFoundException)
            {
                CreateDataPath();
                return new List<SkillList>();
            }
            catch (FileNotFoundException)
            {
                return new List<SkillList>();
            }
        }

        public static void SaveSkills(List<SkillList> skills)
        {
            CreateDataPath();

            var skillList = skills.Select(s => (s.SkillListType, s.GetAll())).ToList();

            string json = JsonSerializer.Serialize(skillList);
            File.WriteAllText(SKILLS_PATH, json);
        }

        internal static Inventory LoadItems()
        {
            try
            {
                string json = File.ReadAllText(ITEMS_PATH);

                List<(ItemType, Item)> itemList = JsonSerializer.Deserialize<List<(ItemType, Item)>>(json)!;

                return new Inventory(itemList.Select(i => { i.Item2.SetItemType(i.Item1); return i.Item2; }).ToList());
            }
            catch (DirectoryNotFoundException)
            {
                CreateDataPath();
                return new Inventory();
            }
            catch (FileNotFoundException)
            {
                return new Inventory();
            }
        }

        public static void SaveItems(Inventory items)
        {
            CreateDataPath();

            var itemList = items.GetAll().Select(i => (i.GetItemType(), i)).ToList();

            string json = JsonSerializer.Serialize(itemList);
            File.WriteAllText(ITEMS_PATH, json);
        }

        public static void LoadAll(out CharactersList characters, out List<SkillList> skills, out Inventory items)
        {
            Logger.Instance().Info("Loading data", "SaveManager.LoadAll()");

            characters = LoadCharacters();
            skills = LoadSkills();
            items = LoadItems();
        }

        public static void SaveAll(CharactersList characters, List<SkillList> skills, Inventory items)
        {
            Logger.Instance().Info("Saving data", "SaveManager.SaveAll()");

            SaveCharacters(characters);
            SaveSkills(skills);
            SaveItems(items);
        }
    }
}
