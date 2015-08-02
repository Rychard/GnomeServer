using Game;
using System;
using TypeLite;

namespace GnomeServer.Models
{
    [TsClass]
    public class GnomeStats
    {
        public Single Happiness { get; set; }

        // TODO: Should we drop the "Level" suffix?
        public Single BloodLevel { get; set; }

        public Single Rest { get; set; }

        public Single Hunger { get; set; }

        public Single Thirst { get; set; }

        public GnomeStats(Character gnome)
        {
            // Mind
            Happiness = gnome.Mind.HappinessLevel;

            // Body
            BloodLevel = gnome.Body.BloodLevel;
            Hunger = gnome.Body.HungerLevel;
            Rest = gnome.Body.RestLevel;
            Thirst = gnome.Body.ThirstLevel;
        }
    }
}