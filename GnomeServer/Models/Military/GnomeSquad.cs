using System;
using System.Linq;
using Game;
using GnomeServer.Extensions;

namespace GnomeServer.Models
{
    public class GnomeSquad
    {
        public String Name { get; set; }
        public Gnome[] Gnomes { get; set; }

        public GnomeSquad()
        {
        }

        public GnomeSquad(Squad squad)
        {
            var skillDefinitions = GnomanEmpire.Instance.GetSkillDefs();
            var gnomesInSquad = squad.Members.Where(obj => obj != null).Select(obj => new Gnome(obj, skillDefinitions)).ToArray();

            Name = squad.Name;
            Gnomes = gnomesInSquad;
        }
    }
}
