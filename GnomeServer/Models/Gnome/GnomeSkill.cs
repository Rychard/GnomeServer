using System;
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

        public static GnomeSkill[] GetGnomeSkills(SkillDef[] skillDefinitions, Character gameGnome)
        {
            var skills = skillDefinitions.Select(skill => new GnomeSkill(gameGnome, skill)).ToArray();
            return skills;
        }
    }
}