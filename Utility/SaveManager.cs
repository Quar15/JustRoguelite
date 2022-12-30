using JustRoguelite.Characters;
using JustRoguelite.Skills;
using JustRoguelite.Items;

namespace JustRoguelite.Utility
{
    internal class SaveManager
    {
        public static void LoadData(out CharactersList characters, out List<SkillList> skills, out Inventory inventory)
        {
            Logger.Instance().Info("Loading Data", "DevTools.LoadData(...)");
            // Skills
            skills = new();
            SkillList skillList = new(SkillType.ATTACK);
            skills.Add(skillList);
            Skill skill1 = new("Basic Attack", "Lorem Ipsum", DamageType.PHYSICAL, new(5));
            skillList.Add(skill1);

            SkillList skillList2 = new(SkillType.SPELL, new());
            skills.Add(skillList2);
            Skill skill2 = new("Fireball", "Lorem Ipsum", DamageType.MAGIC, new(5));
            Skill skill3 = new("Death", "Max DMG", DamageType.PHYSICAL, new(999));
            skillList2.Add(skill2);
            skillList2.Add(skill3);
            // Items
            Item i1 = new("Healing Potion", "Heal for 2 HP");
            inventory = new();
            inventory.Add(i1);
            // Characters
            CharacterBase player = new("Player01", new CharacterStats(maxHP: 20));
            CharacterBase p2 = new("Player02", new CharacterStats());
            CharacterBase p3 = new("Player03", new CharacterStats());
            CharacterBase e1 = new("Enemy01", new CharacterStats(maxHP: 5));

            characters = new CharactersList();
            characters.Add(player);
            characters.Add(p2);
            characters.Add(p3);
            characters.Add(e1);
        }

        public static void SaveData(CharactersList characters, List<SkillList> skills, Inventory inventory)
        {
            Logger.Instance().Info("Saving Data", "DevTools.SaveData(...)");
        }
    }
}
