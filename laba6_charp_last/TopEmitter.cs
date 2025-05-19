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
        public int SpeedMax = 2;
        public int RadiusMin = 10;
        public int RadiusMax = 20;
        public int LifeMin = 700;
        public int LifeMax = 900;
        public Color ColorFrom = Color.White;
        public Color ColorTo = Color.FromArgb(0, Color.Black);
        public int HitsToDestroyMin = 3;  // Новые параметры
        public int HitsToDestroyMax = 6;
        //public float GravitationY = 0.5f;

        public override Particle CreateParticle()
        {
            return new TargetParticle
            {
                FromColor = ColorFrom,
                ToColor = ColorTo,
                HitsToDestroy = Particle.rand.Next(HitsToDestroyMin, HitsToDestroyMax),
                SpeedY = Particle.rand.Next(SpeedMin, SpeedMax) * 0.2f // Медленное движение
            };
        }

        public override void ResetParticle(Particle particle)
        {
            particle.Life = Particle.rand.Next(LifeMin, LifeMax);
            particle.X = Particle.rand.Next(Width);
            particle.Y = -TargetParticle.RocketImage.Height;

            // Фиксированная скорость без ускорения
            particle.SpeedX = (float)(Particle.rand.NextDouble() - 0.5) * 0.5f;
            particle.SpeedY = Particle.rand.Next(SpeedMin, SpeedMax) * 0.3f;

            // Убираем гравитацию для этого эмиттера
            this.GravitationY = 0;

            if (particle is TargetParticle target)
            {
                target.HitCount = 0;
                target.HitsToDestroy = Particle.rand.Next(HitsToDestroyMin, HitsToDestroyMax);
            }
        }
    }
}
