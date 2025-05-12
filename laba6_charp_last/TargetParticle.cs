using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba6_charp_last
{
    public class TargetParticle : ParticleColorful
    {
        public int HitCount = 0;
        public int HitsToDestroy = 3;

        public override void Draw(Graphics g)
        {
            base.Draw(g);

            var stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            g.DrawString(
                $"{HitCount}/{HitsToDestroy}",
                new Font("Arial", 8),
                Brushes.White,
                X,
                Y,
                stringFormat
            );
        }
    }
}
