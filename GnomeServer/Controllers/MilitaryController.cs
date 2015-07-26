using System;
using System.Linq;
using Game;
using GnomeServer.Models;
using GnomeServer.Routing;

namespace GnomeServer.Controllers
{
    [Route("Military")]
    public sealed class MilitaryController : ConventionRoutingController
    {
        [Route("")]
        public IResponseFormatter Get()
        {
            var squads = GnomanEmpire.Instance.Fortress.Military.Squads;
            var gnomeSquads = squads.Select(obj => new GnomeSquad(obj)).ToArray();
            return JsonResponse(gnomeSquads);
        }

        [Route("Assign")]
        public IResponseFormatter Assign()
        {
            var game = GnomanEmpire.Instance;
            var playerFaction = game.World.AIDirector.PlayerFaction;
            var gnomes = playerFaction.Members.Select(obj => obj.Value);
            foreach (var gnome in gnomes)
            {
                // Because we are potentially creating squads inside this loop, we should always refresh this collection on each iteration.
                var squads = game.Fortress.Military.Squads;

                // Check to see if the gnome is already in a squad.  
                // If not, assign them to the first squad with an available slot.
                // TODO: Should we apply a more complex algorithm for this?  Ideally we would optimize the selection to optimize the quality of the military.
                if (gnome.Squad == null)
                {
                    Boolean added = false;
                    foreach (var squad in squads)
                    {
                        // A Squad can only have 5 Gnomes in it.
                        for (uint i = 0; i < 5; i++)
                        {
                            Boolean isVacant = (squad.Members[i] == null);
                            if (isVacant)
                            {
                                added = true;
                                squad.AddMember(i, gnome);
                            }
                        }
                    }

                    // Determine if the Gnome remains unassigned to a squad.
                    if (!added)
                    {
                        // This must indicate that all existing squads are full, so we need a new Squad.

                        // The game generates a random name for all new Squads.  We'll do the same.
                        var squadName = game.GameDefs.LanguageSettings.RandomSquadName(playerFaction.FactionDef.LanguageID);
                        Squad emptySquad = new Squad(squadName);

                        // TODO: Set a formation?
                        game.Fortress.Military.AddSquad(emptySquad);

                        // Lastly, add our Gnome to the newly created Squad.
                        emptySquad.AddMember(0, gnome);
                    }
                }
            }

            // After assigning our Gnomes to the Squads, we'll redirect to the standard endpoint.
            return Get();
        }
    }
}
