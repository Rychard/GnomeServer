using System;
using Game;
using GameLibrary;
using TypeLite;

namespace GnomeServer.Models
{
    [TsClass]
    public class Gnome
    {
        public UInt32 ID { get; set; }
        public String Name { get; set; }
        public GnomeStats Stats { get; set; }
        public Location Location { get; set; }
        public GnomeBodyPartStatus[] BodyParts { get; set; }
        public GnomeSkill[] CombatSkills { get; set; }
        public GnomeSkill[] LaborSkills { get; set; }
        public GnomeProfession Profession { get; set; }

        public Gnome()
        {
            // Empty constructor for serialization.
        }

        public Gnome(Character gnome, SkillDef[] skillDefinitions)
        {
            ID = gnome.ID;
            Name = gnome.Name();
            Location = new Location(gnome.Position);
            Stats = new GnomeStats(gnome);
            Profession = new GnomeProfession(gnome.Mind.Profession);
            BodyParts = GnomeBodyPartStatus.GetBodyStatus(gnome);
            LaborSkills = GnomeSkill.GetGnomeSkills(GnomeSkill.GnomeSkillType.Labor, skillDefinitions, gnome);
            CombatSkills = GnomeSkill.GetGnomeSkills(GnomeSkill.GnomeSkillType.Combat, skillDefinitions, gnome);
        }
    }
}