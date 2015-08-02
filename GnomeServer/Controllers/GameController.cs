using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net;
using Game;
using GnomeServer.Extensions;
using GnomeServer.Routing;

namespace GnomeServer.Controllers
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [Route("Game")]
    public sealed class GameController : ConventionRoutingController
    {
        [HttpGet]
        [Route("State")]
        public IResponseFormatter GetState()
        {
            GnomanEmpire.GameState gameState = GnomanEmpire.Instance.GameState();
            var value = Enum.GetName(typeof (GnomanEmpire.GameState), gameState);
            return JsonResponse(value);
        }

        [HttpGet]
        [Route("Speed")]
        public IResponseFormatter GetSpeed()
        {
            var world = GnomanEmpire.Instance.World;
            var speed = new
            {
                Speed = world.GameSpeed.Value.ToString(CultureInfo.InvariantCulture),
                IsPaused = world.Paused.Value.ToString(CultureInfo.InvariantCulture)
            };
            return JsonResponse(speed);
        }

        [HttpPost]
        [Route("Speed")]
        public IResponseFormatter PostSpeed(Int32 speed)
        {
            GnomanEmpire.Instance.World.GameSpeed.Value = speed;
            return BlankResponse(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Route("Pause")]
        public IResponseFormatter PostPause()
        {
            GnomanEmpire.Instance.World.Paused.Value = true;
            return BlankResponse(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Route("Play")]
        public IResponseFormatter PostPlay()
        {
            GnomanEmpire.Instance.World.Paused.Value = false;
            return BlankResponse(HttpStatusCode.NoContent);
        }
    }
}
