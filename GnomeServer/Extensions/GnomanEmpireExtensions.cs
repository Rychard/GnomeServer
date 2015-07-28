using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;

namespace GnomeServer.Extensions
{
    public static class GnomanEmpireExtensions
    {
        public static Dictionary<UInt32, Character> GetGnomes(this GnomanEmpire instance)
        {
            try
            {
                var gnomes = instance.World.AIDirector.PlayerFaction.Members;
                return gnomes;
            }
            catch (Exception)
            {
                // Before the player has loaded a game, PlayerFaction throws an exception.
                return new Dictionary<UInt32, Character>();
            }
        }
    }
}
