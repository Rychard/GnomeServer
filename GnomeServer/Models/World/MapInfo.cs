using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TypeLite;

namespace GnomeServer.Models.World
{
    [TsClass]
    public class MapInfo
    {
        // Map.MapWidth
        public Int32 Width { get; set; }

        // Map.MapHeight
        public Int32 Height { get; set; }

        // Map.MapDepth
        public Int32 Depth { get; set; }

        // Map.WorldSeed
        public Int32 Seed { get; set; }

        // Other interesting properties:
        // Map.SurfaceLevel

        public MapInfo()
        {
            // Empty constructor for serialization.
        }
    }
}
