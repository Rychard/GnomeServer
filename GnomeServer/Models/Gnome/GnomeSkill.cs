using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using GameLibrary;
using TypeLite;

namespace GnomeServer.Models
{
    [TsClass]
    public class GnomeSkill
    {
        public String Name { get; set; }
        public Int32 Skill { get; set; }

        public GnomeSkill()
        {
            // Empty constructor for serialization.
        }

        public GnomeSkill(Character gameGnome, SkillDef skill)
        {
            Name = skill.Name;
            Skill = gameGnome.SkillLevel(skill.ID);
        }

        public static GnomeSkill[] GetGnomeSkills(GnomeSkillType skillType, SkillDef[] skillDefinitions, Character gameGnome)
        {
            List<String> skillNames = new List<String>();

            if (skillType.HasFlag(GnomeSkillType.Labor))
            {
                skillNames.AddRange(GnomanEmpire.Instance.GameDefs.CharacterSettings.LaborSkills);
            }

            if (skillType.HasFlag(GnomeSkillType.Combat))
            {
                skillNames.AddRange(GnomanEmpire.Instance.GameDefs.CharacterSettings.CombatSkills);
            }

            var skills = skillDefinitions.Where(obj => skillNames.Contains(obj.Name)).Select(skill => new GnomeSkill(gameGnome, skill)).ToArray();
            return skills;
        }

        [Flags]
        public enum GnomeSkillType
        {
            None = 0,
            Labor = 1,
            Combat = 2,
            All = 3,
        }
    }
}