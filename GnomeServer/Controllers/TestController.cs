using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Game;
using GameLibrary;
using GnomeServer.Extensions;
using GnomeServer.ResponseFormatters;
using GnomeServer.Routing;
using Microsoft.Xna.Framework.Graphics;

namespace GnomeServer.Controllers
{
    [Route("Test")]
    public class TestController : ConventionRoutingController
    {
        [Route("")]
        public IResponseFormatter Get()
        {
            // Note: This outputs a gnome sprite with no equipment. (i.e. naked)
            var gnome = GnomanEmpire.Instance.GetGnomes().FirstOrDefault();
            if (gnome == null)
            {
                return BlankResponse(HttpStatusCode.NoContent);
            }

            // Get the current tileset used by the game.
            Texture2D texture = GnomanEmpire.Instance.Map.TileSet.mTexture;

            // Save the tileset in PNG format to memory.
            MemoryStream ms = new MemoryStream();
            texture.SaveAsPng(ms, texture.Width, texture.Height);

            // Reset the stream to the beginning.
            ms.Seek(0, SeekOrigin.Begin);

            // Load the tileset as a bitmap.
            Bitmap tileset = new Bitmap(ms);
            Bitmap output = new Bitmap(256, 256);

            var bodySections = gnome.Body.BodySections.Where(obj => obj.SectionTile != null);
            var sectionTiles = bodySections.Select(obj => obj.SectionTile).OrderByDescending(obj => obj.DrawOrder);

            using (var imageAttributes = new ImageAttributes())
            {
                imageAttributes.SetColorKey(Color.Magenta, Color.Magenta);
            
                using (Graphics g = Graphics.FromImage(output))
                {
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;

                    var gameDefs = GnomanEmpire.Instance.GameDefs;
                    foreach (var sectionTile in sectionTiles)
                    {
                        String spriteID = sectionTile.BodyTile.SpriteID;
                        var spriteDef = gameDefs.SpriteDef(spriteID);

                        var pSource = new Point(spriteDef.SourceRectangle.Location.X, spriteDef.SourceRectangle.Location.Y);
                        var sSource = new Size(spriteDef.SourceRectangle.Width, spriteDef.SourceRectangle.Height);
                        var rSource = new Rectangle(pSource, sSource);

                        //var pDest = new Point((Int32)spriteDef.Offset.X, (Int32)spriteDef.Offset.Y);
                        var pDest = new Point(0, 0);
                        //var sDest = new Size(spriteDef.SourceRectangle.Width, spriteDef.SourceRectangle.Height);
                        var sDest = new Size(output.Width, output.Height);
                        var rDest = new Rectangle(pDest, sDest);

                        g.DrawImage(tileset, rDest, rSource.X, rSource.Y, rSource.Width, rSource.Height, GraphicsUnit.Pixel, imageAttributes);
                    }

                    
                    //foreach (var obj in gnome.Body.HeldItems)
                    //{
                    //    ItemDef itemDef = gameDefs.ItemDef(obj.ItemID);
                    //    foreach (var spriteDesc in itemDef.SpriteIDs)
                    //    {
                    //        String spriteID = spriteDesc.GetSpriteID(obj.MaterialID);
                    //        var spriteDef = gameDefs.SpriteDef(spriteID);

                    //        var pSource = new Point(spriteDef.SourceRectangle.Location.X, spriteDef.SourceRectangle.Location.Y);
                    //        var sSource = new Size(spriteDef.SourceRectangle.Width, spriteDef.SourceRectangle.Height);
                    //        var rSource = new Rectangle(pSource, sSource);

                    //        //var pDest = new Point((Int32)spriteDef.Offset.X, (Int32)spriteDef.Offset.Y);
                    //        var pDest = new Point(0, 0);
                    //        //var sDest = new Size(spriteDef.SourceRectangle.Width, spriteDef.SourceRectangle.Height);
                    //        var sDest = new Size(output.Width, output.Height);
                    //        var rDest = new Rectangle(pDest, sDest);

                    //        g.DrawImage(tileset, rDest, rSource.X, rSource.Y, rSource.Width, rSource.Height, GraphicsUnit.Pixel, imageAttributes);
                    //    }
                    //}
                }
            }

            return ImageResponse(output);
        }
    }
}
