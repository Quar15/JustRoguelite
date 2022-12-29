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

            #region LOAD_DATA
            // Skills
            List<SkillList> fullSkillsLists = new();
            SkillList skillList = new(SkillType.ATTACK);
            fullSkillsLists.Add(skillList);
            Skill skill1 = new("Basic Attack", "Lorem Ipsum", DamageType.PHYSICAL, new(5));
            skillList.Add(skill1);

            SkillList skillList2 = new(SkillType.SPELL, new());
            fullSkillsLists.Add(skillList2);
            Skill skill2 = new("Fireball", "Lorem Ipsum", DamageType.MAGIC, new(5));
            Skill skill3 = new("Death", "Max DMG", DamageType.PHYSICAL, new(999));
            skillList2.Add(skill2);
            skillList2.Add(skill3);
            // Items
            Item i1 = new("Healing Potion", "Heal for 2 HP");
            Inventory inventory = new();
            inventory.Add(i1);
            // Characters
            CharacterBase player = new("Player01", new CharacterStats(maxHP: 20), true);
            CharacterBase p2 = new("Player02", new CharacterStats(), true);
            CharacterBase p3 = new("Player03", new CharacterStats(), true);
            CharacterBase e1 = new("Enemy01", new CharacterStats(maxHP: 5));

            CharactersList charactersList = new CharactersList();
            charactersList.Add(player);
            charactersList.Add(p2);
            charactersList.Add(p3);
            charactersList.Add(e1);
            #endregion

            // Main Menu handling
            DevTools.Loop(charactersList, fullSkillsLists, inventory);


            Logger.Instance().Info("Program END", "Program.Main()");
        }
    }
}