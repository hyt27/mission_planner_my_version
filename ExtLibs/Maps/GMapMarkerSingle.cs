﻿using System;
using System.Drawing;
using GMap.NET;
using GMap.NET.WindowsForms;
using MissionPlanner.Utilities;

namespace MissionPlanner.Maps
{
    [Serializable]
    public class GMapMarkerSingle : GMapMarkerBase
    {
        private readonly Bitmap icon = global::MissionPlanner.Maps.Resources.redsinglecopter2;

        float heading = 0;
        float cog = -1;
        float target = -1;
        private int sysid = -1;

        public GMapMarkerSingle(PointLatLng p, float heading, float cog, float target, int sysid)
            : base(p)
        {
            this.heading = heading;
            this.cog = cog;
            this.target = target;
            this.sysid = sysid;
            Size = icon.Size;
        }

        public override void OnRender(IGraphics g)
        {
            if (IsHidden)
            {
                return;
            }
            
            var temp = g.Transform;
            g.TranslateTransform(LocalPosition.X, LocalPosition.Y);

            // anti NaN
            try
            {
                if (DisplayHeading)
                    g.DrawLine(new Pen(Color.Red, 2), 0.0f, 0.0f,
                        (float)Math.Cos((heading - 90) * MathHelper.deg2rad) * length,
                        (float)Math.Sin((heading - 90) * MathHelper.deg2rad) * length);
            }
            catch
            {
            }

            if (DisplayCOG)
                g.DrawLine(new Pen(Color.Black, 2), 0.0f, 0.0f,
                    (float)Math.Cos((cog - 90) * MathHelper.deg2rad) * length,
                    (float)Math.Sin((cog - 90) * MathHelper.deg2rad) * length);
            if (DisplayTarget)
                g.DrawLine(new Pen(Color.Orange, 2), 0.0f, 0.0f,
                    (float)Math.Cos((target - 90) * MathHelper.deg2rad) * length,
                    (float)Math.Sin((target - 90) * MathHelper.deg2rad) * length);
            // anti NaN
            try
            {
                g.RotateTransform(heading);
            }
            catch
            {
            }

            var fontBrush = new SolidBrush(Color.Red);
#if NET472_OR_GREATER
            var ia = new System.Drawing.Imaging.ImageAttributes();
            if (IsTransparent)
            {
                // Draw image with transparency using a color matrix
                var cm = new System.Drawing.Imaging.ColorMatrix { Matrix33 = 0.39f };
                ia.SetColorMatrix(cm, System.Drawing.Imaging.ColorMatrixFlag.Default, System.Drawing.Imaging.ColorAdjustType.Bitmap);

                fontBrush.Color = Color.FromArgb(100, fontBrush.Color);
            }
            g.DrawImage(icon, new Rectangle(-icon.Width / 2, -icon.Width / 2, icon.Width, icon.Height), 0, 0, icon.Width, icon.Height, GraphicsUnit.Pixel, ia);
#else
            g.DrawImageUnscaled(icon, icon.Width / -2 + 2, icon.Height / -2);
#endif

            g.DrawString(sysid.ToString(), new Font(FontFamily.GenericMonospace, 15, FontStyle.Bold), fontBrush, -8, -8);

            g.Transform = temp;
        }
    }
}