using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba6_charp_last
{
    public class GunEmitter : Emitter
    {
        public float X;
        public float Y;
        public float Direction = 0;  // Изменено на float
        public int Spreading = 10;
        public int SpeedMin = 15;
        public int SpeedMax = 20;
        public int RadiusMin = 2;
        public int RadiusMax = 5;
        public Color ColorFrom = Color.White;
        public Color ColorTo = Color.FromArgb(0, Color.Black);

        public override Particle CreateParticle()
        {
            var particle = new ParticleColorful
            {
                FromColor = ColorFrom,
                ToColor = ColorTo,
                Life = 50
            };
            return particle;
        }

        public override void ResetParticle(Particle particle)
        {
            particle.Life = 50;
            particle.X = X;
            particle.Y = Y - 15; // Корректируем точку вылета

            // Правильный расчёт направления
            double angle = (Direction + Particle.rand.Next(Spreading) - Spreading / 2) * Math.PI / 180;
            float speed = Particle.rand.Next(SpeedMin, SpeedMax);

            particle.SpeedX = (float)(Math.Cos(angle) * speed);
            particle.SpeedY = -(float)(Math.Sin(angle) * speed); // Отрицательное значение, т.к. Y растёт вниз

            particle.Radius = Particle.rand.Next(RadiusMin, RadiusMax);
        }
    }
}
