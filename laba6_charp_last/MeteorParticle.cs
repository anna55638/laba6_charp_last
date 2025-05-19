using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba6_charp_last
{
    public class MeteorParticle : TargetParticle
    {
        public static Image MeteorImage;
        private static Size meteorSize = new Size(50, 50);
        public bool IsDestroyed = false;

        static MeteorParticle()
        {
            try
            {
                var originalImage = Image.FromFile("Resources/meteor.png");
                MeteorImage = new Bitmap(originalImage, meteorSize);
                originalImage.Dispose();
            }
            catch
            {
                MeteorImage = new Bitmap(meteorSize.Width, meteorSize.Height);
                using (var g = Graphics.FromImage(MeteorImage))
                {
                    g.FillEllipse(Brushes.DarkGray, 0, 0, meteorSize.Width, meteorSize.Height);
                }
            }
        }

        public override void Draw(Graphics g)
        {
            g.DrawImage(MeteorImage,
                X - MeteorImage.Width / 2,
                Y - MeteorImage.Height / 2,
                MeteorImage.Width,
                MeteorImage.Height);

            var stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            g.DrawString(
                $"{HitCount}/{HitsToDestroy}",
                new Font("Arial", 8),
                Brushes.White,
                X,
                Y + MeteorImage.Height / 2 + 10,
                stringFormat
            );
        }
    }
}
