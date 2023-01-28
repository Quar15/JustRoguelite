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
        const string BASE_STATS_PATH = DATA_PATH + "BaseStats.json";

        private static void CreateDataPath()
        {
            if (!Directory.Exists(DATA_PATH))
            {
                Directory.CreateDirectory(DATA_PATH);
            }
        }

        internal static List<Dictionary<string, string>> LoadData(string path)
        {
            try
            {
                string json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json)!;
            }
            catch (DirectoryNotFoundException)
            {
                CreateDataPath();
                return new();
            }
            catch (FileNotFoundException)
            {
                return new();
            }
        }

        public static void SaveData(string path, List<Dictionary<string, string>> data)
        {
            CreateDataPath();

            string json = JsonSerializer.Serialize(data);
            File.WriteAllText(path, json);
        }

        public static List<CharacterStats> LoadBaseStats()
        {
            return LoadData(BASE_STATS_PATH)
                .Select(b => CharacterStats.FromDict(b))
                .ToList();
        }

        public static void SaveBaseStats(List<CharacterStats> baseStats)
        {
            CreateDataPath();
            SaveData(BASE_STATS_PATH, baseStats.Select(b => b.AsDict()).ToList());
        }

        internal static CharacterDataList LoadCharacters(List<CharacterStats> baseStats)
        {
            return new(LoadData(CHARACTERS_PATH)
                .Select(c => CharacterData.FromDict(c, baseStats))
                .ToList());
        }

        public static void SaveCharacters(CharacterDataList characters)
        {
            CreateDataPath();
            SaveData(CHARACTERS_PATH, characters.GetAll().Select(c => c.AsDict()).ToList());
        }

        internal static SkillDataList LoadSkills()
        {
            return new(LoadData(SKILLS_PATH)
                .Select(s => SkillData.FromDict(s))
                .ToList());
        }

        public static void SaveSkills(SkillDataList skills)
        {
            CreateDataPath();
            SaveData(SKILLS_PATH, skills.GetAll().Select(s => s.AsDict()).ToList());
        }

        internal static ItemDataList LoadItems()
        {
            return new(LoadData(ITEMS_PATH)
                .Select(i => ItemData.FromDict(i))
                .ToList());
        }

        public static void SaveItems(ItemDataList items)
        {
            CreateDataPath();
            SaveData(ITEMS_PATH, items.GetAll().Select(i => i.AsDict()).ToList());
        }

        public static void LoadAllData(out List<Dictionary<string, string>> baseStats, out List<Dictionary<string, string>> characters, out List<Dictionary<string, string>> skills, out List<Dictionary<string, string>> items)
        {
            Logger.Instance().Info("Loading data", "SaveManager.LoadAllData()");

            baseStats = LoadData(BASE_STATS_PATH);
            characters = LoadData(CHARACTERS_PATH);
            skills = LoadData(SKILLS_PATH);
            items = LoadData(ITEMS_PATH);
        }

        public static void LoadAll(out List<CharacterStats> baseStats, out CharacterDataList characters, out SkillDataList skills, out ItemDataList items)
        {
            Logger.Instance().Info("Loading data", "SaveManager.LoadAll()");

            baseStats = LoadBaseStats();
            characters = LoadCharacters(baseStats);
            skills = LoadSkills();
            items = LoadItems();
        }

        public static void SaveAllData(List<Dictionary<string, string>> baseStats, List<Dictionary<string, string>> characters, List<Dictionary<string, string>> skills, List<Dictionary<string, string>> items)
        {
            Logger.Instance().Info("Saving data", "SaveManager.SaveAllData()");

            SaveData(BASE_STATS_PATH, baseStats);
            SaveData(CHARACTERS_PATH, characters);
            SaveData(SKILLS_PATH, skills);
            SaveData(ITEMS_PATH, items);
        }

        public static void SaveAll(List<CharacterStats> baseStats, CharacterDataList characters, SkillDataList skills, ItemDataList items)
        {
            Logger.Instance().Info("Saving data", "SaveManager.SaveAll()");

            SaveBaseStats(baseStats);
            SaveCharacters(characters);
            SaveSkills(skills);
            SaveItems(items);
        }
    }
}
