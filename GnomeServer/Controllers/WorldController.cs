using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Game;
using GnomeServer.Routing;

namespace GnomeServer.Controllers
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [Route("World")]
    public sealed class WorldController : ConventionRoutingController
    {
        [HttpGet]
        [Route("Speed")]
        public IResponseFormatter GetSpeed()
        {
            var speed = GnomanEmpire.Instance.World.GameSpeed.Value;
            return JsonResponse(speed);
        }

        [HttpPost]
        [Route("Speed")]
        public IResponseFormatter PostSpeed(Single speed)
        {
            GnomanEmpire.Instance.World.GameSpeed.Value = speed;
            return BlankResponse(HttpStatusCode.NoContent);
        }
    }
}
