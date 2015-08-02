using System;
using System.Linq;
using System.Reflection;
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

        public static Boolean IsPaused(this GnomanEmpire instance)
        {
            return instance.World.Paused.Value;
        }


        private static FieldInfo _gameStateFieldInfo = null;
        public static GnomanEmpire.GameState GameState(this GnomanEmpire instance)
        {
            // There is an instance field on the GnomanEmpire type, where a GameState enum value is stored.
            // Because this field is not public, we have to rely on reflection to obtain its value.

            if (_gameStateFieldInfo == null)
            {
                var typeGnomanEmpire = typeof(GnomanEmpire);
                _gameStateFieldInfo = typeGnomanEmpire.GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Single(obj => obj.FieldType == typeof (GnomanEmpire.GameState));
            }

            GnomanEmpire.GameState value = (GnomanEmpire.GameState)_gameStateFieldInfo.GetValue(GnomanEmpire.Instance);
            return value;
        }
    }
}
