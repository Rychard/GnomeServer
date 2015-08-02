using System;
using System.Linq;
using Game;
using TypeLite;

namespace GnomeServer.Models
{
    [TsClass]
    public class GnomeProfession
    {
        public String Name { get; set; }
        public String[] Skills { get; set; }

        public GnomeProfession(Profession profession)
        {
            Name = profession.Title;
            Skills = profession.AllowedSkills.AllowedSkills.ToArray();
        }
    }
}