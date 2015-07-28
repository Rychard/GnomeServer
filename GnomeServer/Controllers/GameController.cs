using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Game;
using GnomeServer.Extensions;
using GnomeServer.Routing;

namespace GnomeServer.Controllers
{
    [Route("Game")]
    public sealed class GameController : ConventionRoutingController
    {
        [HttpGet]
        [Route("")]
        public IResponseFormatter Get(int speed)
        {
            GnomanEmpire.Instance.World.GameSpeed.Value = speed;
            String content = String.Format("Game Speed set to '{0}'", speed);
            return JsonResponse(content);
        }

        public IResponseFormatter Test()
        {
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

            var fields = typeof(Character).GetFields(bindFlags);
            var behaviorTypeFields = fields.Where(obj => obj.FieldType == typeof(BehaviorType)).ToList();

            List<TestResponse> testResponses = new List<TestResponse>();

            var members = GnomanEmpire.Instance.GetGnomes();
            foreach (var characterKey in members)
            {
                var character = characterKey.Value;
                var name = character.NameAndTitle();

                foreach (var fieldInfo in behaviorTypeFields)
                {
                    var val = (BehaviorType)(fieldInfo.GetValue(character));
                    testResponses.Add(new TestResponse
                    {
                        Name = name,
                        Value = val.ToString(),
                    });
                }
            }
            return JsonResponse(testResponses);
        }

        private class TestResponse
        {
            public String Name { get; set; }
            public String Value { get; set; }
        }
    }
}
