using System.Globalization;
using System.Net;
using Game;
using GnomeServer.Routing;

namespace GnomeServer.Controllers
{
    [Route("Game")]
    public sealed class GameController : ConventionRoutingController
    {
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
        public IResponseFormatter PostSpeed(int speed)
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
