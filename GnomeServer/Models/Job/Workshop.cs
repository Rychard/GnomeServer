using System;
using TypeLite;

namespace GnomeServer.Models
{
    [TsClass]
    public class Workshop
    {
        public String Name { get; set; }
        public Boolean AcceptGeneratedJobs { get; set; }
        public Single Efficiency { get; set; }
        public Location Location { get; set; }


        public Workshop(Game.Workshop workshop)
        {
            Name = workshop.Name();
            AcceptGeneratedJobs = workshop.AcceptGeneratedJobs;
            Location = new Location(workshop.Position);
            Efficiency = workshop.Efficiency();
        }


    }
}
