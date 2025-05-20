using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba6_charp_last
{
    public class Emitter
    {
        public List<Particle> particles = new List<Particle>();
        public int ParticlesCount = 500;
        public int ParticlesPerTick = 1;
        public float GravitationX = 0;
        public float GravitationY = 0;
        public int MousePositionX;
        public int MousePositionY;

        public float X;
        public float Y;

        public virtual Particle CreateParticle()
        {
            var particle = new ParticleColorful();
            return particle;
        }

        public virtual void ResetParticle(Particle particle)
        {
            particle.Life = 50 + Particle.rand.Next(100); // Increased base life
            particle.X = MousePositionX;
            particle.Y = MousePositionY;

            var direction = (double)Particle.rand.Next(360);
            var speed = 0.5f + Particle.rand.Next(5); // Reduced speed

            particle.SpeedX = (float)(Math.Cos(direction / 180 * Math.PI) * speed) * 0.5f; // Slower movement
            particle.SpeedY = -(float)(Math.Sin(direction / 180 * Math.PI) * speed) * 0.5f; // Slower movement

            particle.Radius = 2 + Particle.rand.Next(5); // Smaller radius range
        }

        public void UpdateState()
        {
            int particlesToCreate = ParticlesPerTick;

            foreach (var particle in particles)
            {
                if (particle.Life <= 0)
                {
                    if (particlesToCreate > 0)
                    {
                        particlesToCreate--;
                        ResetParticle(particle);
                    }
                }
                else
                {
                    particle.X += particle.SpeedX;
                    particle.Y += particle.SpeedY;
                    if (!(particle is TargetParticle))
                    {
                        particle.Life -= 1;
                    }

                    particle.SpeedX += GravitationX;
                    particle.SpeedY += GravitationY;
                }
            }

            while (particlesToCreate > 0 && particles.Count < ParticlesCount)
            {
                particlesToCreate--;
                var particle = CreateParticle();
                ResetParticle(particle);
                particles.Add(particle);
            }
        }

        public void Render(Graphics g)
        {
            foreach (var particle in particles)
            {
                particle.Draw(g);
            }
        }
    }
}