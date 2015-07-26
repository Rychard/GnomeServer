using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using GameLibrary;
using Microsoft.Xna.Framework;

namespace GnomeServer.Models
{
    public class GnomeSummary
    {
        public int Population { get; set; }
        public Gnome[] Gnomes { get; set; }
    }

    public class Gnome
    {
        public String Name { get; set; }
        public GnomeStats Stats { get; set; }
        public GnomeLocation Location { get; set; }
        public GnomeBodyPartStatus[] BodyParts { get; set; }
        public GnomeSkill[] Skills { get; set; }

        public Gnome() { }

        public Gnome(Character gnome, Dictionary<string, SkillDef> skillDefinitions = null)
        {
            Name = gnome.NameAndTitle();
            Location = new GnomeLocation(gnome.Position);
            Stats = GetGnomeStats(gnome);
            BodyParts = GetBodyStatus(gnome);
            Skills = GetGnomeSkills(skillDefinitions, gnome);
        }

        private static GnomeStats GetGnomeStats(Character gnome)
        {
            GnomeStats stats = new GnomeStats
            {
                // Mind
                Happiness = gnome.Mind.HappinessLevel,

                // Body
                BloodLevel = gnome.Body.BloodLevel,
                Hunger = gnome.Body.HungerLevel,
                Rest = gnome.Body.RestLevel,
                Thirst = gnome.Body.ThirstLevel,
            };
            return stats;
        }

        private static GnomeSkill[] GetGnomeSkills(Dictionary<string, SkillDef> skillDefinitions, Character gameGnome)
        {
            var skills = skillDefinitions.Select(skill => new GnomeSkill
            {
                Name = skill.Value.Name,
                Skill = gameGnome.SkillLevel(skill.Value.ID),
            }).ToArray();
            return skills;
        }

        private static GnomeBodyPartStatus[] GetBodyStatus(Character gameGnome)
        {
            var statuses = gameGnome.Body.BodySections.Select(bodyPart => new GnomeBodyPartStatus
            {
                BodyPart = bodyPart.Name,
                Status = bodyPart.Status.ToString(),
            }).ToArray();
            return statuses;
        }
    }

    public class GnomeStats
    {
        public float Happiness { get; set; }
        public float BloodLevel { get; set; }
        public float Rest { get; set; }
        public float Hunger { get; set; }
        public float Thirst { get; set; }
    }

    public class GnomeSkill
    {
        public String Name { get; set; }
        public int Skill { get; set; }
    }

    public class GnomeLocation
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public GnomeLocation() { }

        public GnomeLocation(Vector3 location)
        {
            X = (int)location.X;
            X = (int)location.Y;
            Z = (int)location.Z;
        }
    }

    public class GnomeBodyPartStatus
    {
        public String BodyPart { get; set; }
        public String Status { get; set; }
    }
}
