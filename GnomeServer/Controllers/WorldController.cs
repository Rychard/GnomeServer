using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using Game;
using GnomeServer.Routing;

namespace GnomeServer.Controllers
{
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
