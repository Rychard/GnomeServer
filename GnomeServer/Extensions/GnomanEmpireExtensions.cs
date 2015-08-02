using System;
using System.Linq;
using Game;
using GameLibrary;

namespace GnomeServer.Extensions
{
    public static class GnomanEmpireExtensions
    {
        public static Character[] GetGnomes(this GnomanEmpire instance)
        {
            try
            {
                var gnomes = instance.World.AIDirector.PlayerFaction.Members.Select(obj => obj.Value).ToArray();
                return gnomes;
            }
            catch (Exception)
            {
                // Before the player has loaded a game, PlayerFaction throws an exception.
                return new Character[0];
            }
        }

        public static SkillDef[] GetSkillDefs(this GnomanEmpire instance)
        {
            // TODO: Should we cache this to prevent crashes?
            return instance.GameDefs.SkillDefs.Select(obj => obj.Value).ToArray();
        }
    }
}
