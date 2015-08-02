using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Net;
using Game;
using GnomeServer.ResponseFormatters;
using GnomeServer.Routing;
using Microsoft.Xna.Framework;

namespace GnomeServer.Controllers
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [Route("World")]
    public sealed class WorldController : ConventionRoutingController
    {
        [HttpGet]
        [Route("Map")]
        public IResponseFormatter GetMap(Int32 level)
        {
            Int32 z = level;
            var map = GnomanEmpire.Instance.Map;
            Int32 width = map.MapWidth;
            Int32 height = map.MapHeight;

            Int32 tileSizeInPixels = 8;
            Int32 pixelWidth = width * tileSizeInPixels;
            Int32 pixelHeight = height * tileSizeInPixels;
            var size = new Size(tileSizeInPixels, tileSizeInPixels);

            Bitmap bitmap = new Bitmap(pixelWidth, pixelHeight);

            SolidBrush brushGreen = new SolidBrush(System.Drawing.Color.LimeGreen);
            SolidBrush brushRed = new SolidBrush(System.Drawing.Color.IndianRed);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(System.Drawing.Color.Black);
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        Vector3 position = new Vector3(x, y, z);
                        var mapCell = map.GetCell(position);

                        Boolean isWalkable = map.IsWalkable(position);
                        
                        int offsetX = x * tileSizeInPixels;
                        int offsetY = y * tileSizeInPixels;
                        System.Drawing.Point point = new System.Drawing.Point(offsetX, offsetY);

                        var brush = isWalkable ? brushGreen : brushRed;
                        System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(point, size);
                        g.FillRectangle(brush, rectangle);
                    }
                }
            }
            return ImageResponse(bitmap);
        }
    }
}
