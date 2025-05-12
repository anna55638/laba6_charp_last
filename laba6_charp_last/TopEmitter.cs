using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba6_charp_last
{
    public class TopEmitter : Emitter
    {
        public int Width;
        public int SpeedMin = 1;
        public int SpeedMax = 3;
        public int RadiusMin = 10;
        public int RadiusMax = 20;
        public int LifeMin = 100;
        public int LifeMax = 200;
        public Color ColorFrom = Color.White; // Исправлено именование
        public Color ColorTo = Color.FromArgb(0, Color.Black); // Исправлено именование

        public override Particle CreateParticle()
        {
            var particle = new TargetParticle
            {
                FromColor = ColorFrom, // Исправлено
                ToColor = ColorTo,     // Исправлено
                HitsToDestroy = Particle.rand.Next(2, 5)
            };
            return particle;
        }

        public override void ResetParticle(Particle particle)
        {
            particle.Life = Particle.rand.Next(LifeMin, LifeMax);
            particle.X = Particle.rand.Next(Width);
            particle.Y = -20;

            particle.SpeedX = Particle.rand.Next(-2, 2);
            particle.SpeedY = Particle.rand.Next(SpeedMin, SpeedMax);

            particle.Radius = Particle.rand.Next(RadiusMin, RadiusMax);

            if (particle is TargetParticle target)
            {
                target.HitCount = 0;
                target.HitsToDestroy = Particle.rand.Next(2, 5);
            }
        }
    }
}
