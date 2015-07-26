using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using GameLibrary;
using GnomeServer.Models;
using GnomeServer.Routing;

namespace GnomeServer.Controllers
{
    [Route("Gnome")]
    public sealed class GnomeController: ConventionRoutingController
    {
        [Route("")]
        public IResponseFormatter Get()
        {
            Dictionary<uint, Character> playerMembers = null;
            Dictionary<string, SkillDef> skillDefinitions = null;

            try
            {
                // Before the player has started/loaded a game, some properties won't be populated.
                playerMembers = GnomanEmpire.Instance.World.AIDirector.PlayerFaction.Members;
                skillDefinitions = GnomanEmpire.Instance.GameDefs.SkillDefs;
            }
            catch (Exception ex)
            {
                OnLogMessage(ex.ToString());
            }

            List<Gnome> gnomes = new List<Gnome>();

            if (playerMembers != null)
            {
                gnomes.AddRange(playerMembers.Select(playerMember => new Gnome(playerMember.Value, skillDefinitions)));
            }

            var summary = new GnomeSummary
            {
                Population = gnomes.Count,
                Gnomes = gnomes.ToArray(),
            };

            return JsonResponse(summary);
        }

        [Route("AddGnome")]
        public IResponseFormatter AddGnome()
        {
            var skillDefinitions = GnomanEmpire.Instance.GameDefs.SkillDefs;
            var faction = GnomanEmpire.Instance.World.AIDirector.PlayerFaction;
            var entryPosition = faction.FindRegionEntryPosition();

            var gnomadRaceClassDefs = faction.FactionDef.Squads.SelectMany(squad => squad.Classes.Where(squadClass => squadClass.Name == "Gnomad")).ToList();
            int defCount = gnomadRaceClassDefs.Count;
            int raceClassDefIndex = GnomanEmpire.Instance.Rand.Next(defCount);
            var raceClassDef = gnomadRaceClassDefs[raceClassDefIndex];
            var gnomad = new Character(entryPosition, raceClassDef, faction.ID);

            gnomad.SetBehavior(BehaviorType.PlayerCharacter);

            GnomanEmpire.Instance.EntityManager.SpawnEntityImmediate(gnomad);

            Gnome gnome = new Gnome(gnomad, skillDefinitions);
            return JsonResponse(gnome);
        }
    }
}
