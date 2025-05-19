using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba6_charp_last
{
    public class ExplosionEmitter : Emitter
    {
        public int ParticlesCount = 30;
        public Color ExplosionColor = Color.Orange;
        public float X;
        public float Y;

        public override Particle CreateParticle()
        {
            return new ParticleColorful
            {
                FromColor = ExplosionColor,
                ToColor = Color.FromArgb(0, Color.Red),
                Radius = 3 + Particle.rand.Next(7)
            };
        }

        public override void ResetParticle(Particle particle)
        {
            particle.Life = 20 + Particle.rand.Next(50);
            particle.X = X;
            particle.Y = Y;

            var direction = Particle.rand.Next(360);
            var speed = 1 + Particle.rand.Next(10);

            particle.SpeedX = (float)(Math.Cos(direction / 180f * Math.PI) * speed) * 0.3f;
            particle.SpeedY = -(float)(Math.Sin(direction / 180f * Math.PI) * speed) * 0.3f;
        }

        public void CreateExplosion(float x, float y, Color color)
        {
            X = x;
            Y = y;
            ExplosionColor = color;
            ParticlesPerTick = ParticlesCount;

            for (int i = 0; i < ParticlesCount; i++)
            {
                var particle = CreateParticle();
                ResetParticle(particle);
                particles.Add(particle);
            }
        }
    }
}
