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
    public partial class Form1 : Form
    {
        List<Emitter> emitters = new List<Emitter>();
        GunEmitter gun;
        TopEmitter topEmitter;
        int score = 0;

        public Form1()
        {
            InitializeComponent();
            picDisplay.MouseMove += picDisplay_MouseMove;
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);

            // Инициализация пушки
            gun = new GunEmitter
            {
                Direction = 90,
                Spreading = 5,
                SpeedMin = 10,
                SpeedMax = 15,
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
                ParticlesPerTick = 1,
                ParticlesCount =2, 
                ColorFrom = Color.LightBlue,
                ColorTo = Color.FromArgb(0, Color.Blue),
                SpeedMin = 1,
                SpeedMax =3,
                RadiusMin = 10,
                RadiusMax = 20,
                LifeMin = 500,
                LifeMax = 1000,
                HitsToDestroyMin = 3,  // Новые параметры
                HitsToDestroyMax = 6
            };

            emitters.Add(gun);
            emitters.Add(topEmitter);

            topEmitter.GravitationY = 0.5f;

            // Настройка TrackBar
            trackBarSpeed.Minimum = 1;
            trackBarSpeed.Maximum = 10;
            trackBarSpeed.Value = 3;
            trackBarSpeed.ValueChanged += TrackBarSpeed_ValueChanged;

            trackBarCount.Minimum = 1;
            trackBarCount.Maximum = 10;
            trackBarCount.Value = 2;
            trackBarCount.ValueChanged += TrackBarCount_ValueChanged;

            trackBarHits.Minimum = 1;
            trackBarHits.Maximum = 10;
            trackBarHits.Value = 3;
            trackBarHits.ValueChanged += TrackBarHits_ValueChanged;
        }

        private void TrackBarSpeed_ValueChanged(object sender, EventArgs e)
        {
            topEmitter.SpeedMin = trackBarSpeed.Value;
            topEmitter.SpeedMax = trackBarSpeed.Value + 2;

            labelSpeed.Text = $"Скорость: {trackBarSpeed.Value}";
        }

        private void TrackBarCount_ValueChanged(object sender, EventArgs e)
        {
            topEmitter.ParticlesPerTick = trackBarCount.Value;

            labelCount.Text = $"Частиц/тик: {trackBarCount.Value}";
        }

        private void TrackBarHits_ValueChanged(object sender, EventArgs e)
        {
            topEmitter.HitsToDestroyMin = trackBarHits.Value;
            topEmitter.HitsToDestroyMax = trackBarHits.Value + 3;

            labelHits.Text = $"Ударов для уничтожения: {trackBarHits.Value}";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateState();

            using (var g = Graphics.FromImage(picDisplay.Image))
            {
                g.Clear(Color.Black);
                g.FillRectangle(Brushes.DarkGreen, 0, picDisplay.Height - 20, picDisplay.Width, 20);
                lblScore.Text = $"Очки: {score}";

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
            //1.Обновляем позицию пушки(только по горизонтали)
            gun.X = e.X;

            // 2. Рассчитываем направление стрельбы
            float dx = e.X - (gun.X); // Разница по X
            float dy = e.Y - (gun.Y - 30); // Разница по Y (с учётом высоты пушки)

            // 3. Нормализуем вектор направления
            float length = (float)Math.Sqrt(dx * dx + dy * dy);
            if (length > 0)
            {
                dx /= length;
                dy /= length;
            }

            // 4. Задаём направление (в градусах, где 0 - вправо, 90 - вверх)
            gun.Direction = (float)(Math.Atan2(-dy, dx) * 180 / Math.PI);

            // 5. Принудительно обновляем отрисовку
            picDisplay.Invalidate();
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
