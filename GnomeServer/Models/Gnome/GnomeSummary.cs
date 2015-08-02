using System;
using System.Collections.Generic;
using Game;
using GameLibrary;
using TypeLite;

namespace GnomeServer.Models
{
    [TsClass]
    public class GnomeSummary
    {
        public Int32 Population { get; set; }
        public Gnome[] Gnomes { get; set; }

        public GnomeSummary()
        {
        }

        public GnomeSummary(IEnumerable<Character> gnomes, SkillDef[] skillDefinitions)
        {
            List<Gnome> gnomeList = new List<Gnome>();

            foreach (var gnome in gnomes)
            {
                gnomeList.Add(new Gnome(gnome, skillDefinitions));
            }

            Gnomes = gnomeList.ToArray();
            Population = gnomeList.Count;
        }
    }
}
