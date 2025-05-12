using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace laba6_charp_last
{
    public partial class Form1: Form
    {
        List<Emitter> emitters = new List<Emitter>();
        GunEmitter gun;
        TopEmitter topEmitter;
        int score = 0;

        public Form1()
        {
            InitializeComponent();
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);

            // Инициализация пушки
            gun = new GunEmitter
            {
                Direction = 90,
                Spreading = 5,
                SpeedMin = 15,
                SpeedMax = 20,
                ColorFrom = Color.Yellow,
                ColorTo = Color.FromArgb(0, Color.Orange),
                ParticlesPerTick = 1,
                ParticlesCount = 100,
                X = picDisplay.Width / 2,
                Y = picDisplay.Height - 30,
            };

            // Инициализация эмиттера мишеней
            topEmitter = new TopEmitter
            {
                Width = picDisplay.Width,
                ParticlesPerTick = 2,
                ParticlesCount = 50,
                ColorFrom = Color.LightBlue,
                ColorTo = Color.FromArgb(0, Color.Blue),
                SpeedMin = 1,
                SpeedMax = 3,
                RadiusMin = 10,
                RadiusMax = 20,
                LifeMin = 500,
                LifeMax = 1000
            };

            emitters.Add(gun);
            emitters.Add(topEmitter);

            topEmitter.GravitationY = 0.5f;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateState();

            using (var g = Graphics.FromImage(picDisplay.Image))
            {
                g.Clear(Color.Black);

                // Рисуем землю
                g.FillRectangle(Brushes.DarkGreen, 0, picDisplay.Height - 20, picDisplay.Width, 20);

                // Рисуем счет
                lblScore.Text = $"Очки: {score}";

                // Рисуем все эмиттеры
                foreach (var emitter in emitters)
                {
                    emitter.Render(g);
                }
            }

            picDisplay.Invalidate();
        }

        private void UpdateState()
        {
            foreach (var emitter in emitters)
            {
                emitter.UpdateState();
            }

            // Проверка столкновений
            foreach (var bullet in gun.particles)
            {
                if (bullet.Life <= 0) continue;

                foreach (var target in topEmitter.particles.OfType<TargetParticle>())
                {
                    if (target.Life <= 0) continue;

                    float dx = bullet.X - target.X;
                    float dy = bullet.Y - target.Y;
                    float distance = (float)Math.Sqrt(dx * dx + dy * dy);

                    if (distance < bullet.Radius + target.Radius)
                    {
                        target.HitCount++;
                        score += 10;

                        if (target.HitCount >= target.HitsToDestroy)
                        {
                            target.Life = 0;
                            score += 50;
                        }

                        bullet.Life = 0;
                        break;
                    }
                }
            }

            // Удаление упавших мишеней
            foreach (var target in topEmitter.particles)
            {
                if (target.Y + target.Radius > picDisplay.Height - 20)
                {
                    target.Life = 0;
                }
            }
        }

        private void picDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            gun.X = e.X;
            gun.Y = e.Y;

            if (e.Y < picDisplay.Height - 50)
            {
                float dx = e.X - gun.X;
                float dy = (picDisplay.Height - 50) - e.Y;
                gun.Direction = (int)(Math.Atan2(dy, dx) * 180 / Math.PI); // Явное приведение к int
            }
            else
            {
                gun.Direction = 90;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            btnStart.Enabled = false;
            score = 0;
            lblScore.Text = "Очки: 0";

            gun.particles.Clear();
            topEmitter.particles.Clear();
        }
    }
}
