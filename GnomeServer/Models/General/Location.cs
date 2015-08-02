using System;
using Microsoft.Xna.Framework;
using TypeLite;

namespace GnomeServer.Models
{
    [TsClass]
    public class Location
    {
        public Int32 X { get; set; }
        public Int32 Y { get; set; }
        public Int32 Z { get; set; }

        public Location() { }

        public Location(Vector3 location)
        {
            X = (Int32)location.X;
            X = (Int32)location.Y;
            Z = (Int32)location.Z;
        }
    }
}
