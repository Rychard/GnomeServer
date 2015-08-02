using System;
using System.Linq;
using Game;
using TypeLite;

namespace GnomeServer.Models
{
    [TsClass]
    public class GnomeBodyPartStatus
    {
        public String BodyPart { get; set; }
        public String[] Statuses { get; set; }

        public GnomeBodyPartStatus()
        {
            // Empty constructor for serialization.
        }

        public GnomeBodyPartStatus(BodySection bodySection)
        {
            var flags = Enum.GetValues(typeof(BodySectionStatus)).Cast<BodySectionStatus>();
            var statuses = flags.Where(flag => bodySection.Status.HasFlag(flag)).Select(flag => Enum.GetName(typeof(BodyPartStatus), flag)).ToArray();
            
            BodyPart = bodySection.Name;
            Statuses = statuses;
        }

        public static GnomeBodyPartStatus[] GetBodyStatus(Character gameGnome)
        {
            var statuses = gameGnome.Body.BodySections.Select(bodySection => new GnomeBodyPartStatus(bodySection)).ToArray();
            return statuses;
        }
    }
}