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
        //private Image backgroundImage;
        private Bitmap highQualityBackground;
        private Image rocketImage;

        List<Emitter> emitters = new List<Emitter>();
        GunEmitter gun;
        TopEmitter topEmitter;
        int score = 0;

        public Form1()
        {
            InitializeComponent();

            // Загрузка фона с высоким качеством
            try
            {
                var originalBg = Image.FromFile("Resources/fon2.jpg");
                highQualityBackground = new Bitmap(originalBg, picDisplay.Width, picDisplay.Height);
                originalBg.Dispose();

                // Настройка PictureBox для качественного отображения
                picDisplay.SizeMode = PictureBoxSizeMode.Normal;
                picDisplay.BackColor = Color.Transparent;
                picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);

                // Установка параметров качества графики
                SetStyle(ControlStyles.OptimizedDoubleBuffer |
                        ControlStyles.AllPaintingInWmPaint |
                        ControlStyles.UserPaint, true);
            }
            catch
            {
                // Резервный вариант
                highQualityBackground = new Bitmap(picDisplay.Width, picDisplay.Height);
                using (var g = Graphics.FromImage(highQualityBackground))
                {
                    g.Clear(Color.DarkBlue);
                }
            }

            // Настройка PictureBox
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);
            picDisplay.MouseMove += picDisplay_MouseMove;

            // Инициализация пушки
            gun = new GunEmitter
            {
                Direction = 90,
                Spreading = 5,
                SpeedMin = 15,  // Увеличиваем скорость пуль
                SpeedMax = 20,
                ColorFrom = Color.Yellow,
                ColorTo = Color.FromArgb(0, Color.Orange),
                FireRate = 3,
                ParticlesCount = 200, // Больше частиц в пуле
                X = picDisplay.Width / 2,
                Y = picDisplay.Height - 30,
            };

            // Инициализация эмиттера ракет с уменьшенной скоростью
            topEmitter = new TopEmitter
            {
                Width = picDisplay.Width,
                ParticlesPerTick = 1,
                ParticlesCount = 5, // Меньше ракет одновременно
                ColorFrom = Color.LightBlue,
                ColorTo = Color.FromArgb(0, Color.Blue),
                SpeedMin = 1,     // Медленная постоянная скорость
                SpeedMax = 2,
                RadiusMin = 15,    // Увеличиваем размер ракет
                RadiusMax = 25,
                LifeMin = 300,     // Уменьшаем время жизни
                LifeMax = 500,
                HitsToDestroyMin = 2, // Легче уничтожить
                HitsToDestroyMax = 5,
                GravitationY = 0   // Нет гравитации
            };

            emitters.Add(gun);
            emitters.Add(topEmitter);

            //topEmitter.GravitationY = 0.5f;

            // Настройка TrackBar
            trackBarSpeed.Minimum = 1;
            trackBarSpeed.Maximum = 10;
            trackBarSpeed.Value = 3;
            trackBarSpeed.ValueChanged += TrackBarSpeed_ValueChanged;

            /*trackBarCount.Minimum = 1;
            trackBarCount.Maximum = 10;
            trackBarCount.Value = 2;
            trackBarCount.ValueChanged += TrackBarCount_ValueChanged;*/

            trackBarHits.Minimum = 1;
            trackBarHits.Maximum = 10;
            trackBarHits.Value = 3;
            trackBarHits.ValueChanged += TrackBarHits_ValueChanged;

            // Добавляем TrackBar для скоростистрельности
            trackBarFireRate.Minimum = 1;
            trackBarFireRate.Maximum = 10;
            trackBarFireRate.Value = 3;
            trackBarFireRate.ValueChanged += TrackBarFireRate_ValueChanged;
            
        }

        private void TrackBarFireRate_ValueChanged(object sender, EventArgs e)
        {
            gun.FireRate = trackBarFireRate.Value;
            labelFireRate.Text = $"Скорострельность: {trackBarFireRate.Value}";
        }

        private void TrackBarSpeed_ValueChanged(object sender, EventArgs e)
        {
            topEmitter.SpeedMin = trackBarSpeed.Value;
            topEmitter.SpeedMax = trackBarSpeed.Value + 2;

            labelSpeed.Text = $"Скорость: {trackBarSpeed.Value}";
        }

        private void TrackBarCount_ValueChanged(object sender, EventArgs e)
        {
            /*topEmitter.ParticlesPerTick = trackBarFireRate.Value;

            labelFireRate.Text = $"Частиц/тик: {trackBarFireRate.Value}";*/
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
                // Очистка с прозрачным фоном
                g.Clear(Color.Transparent);

                // Рисуем фон с высоким качеством
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                g.DrawImage(highQualityBackground,
                           new Rectangle(0, 0, picDisplay.Width, picDisplay.Height),
                           new Rectangle(0, 0, highQualityBackground.Width, highQualityBackground.Height),
                           GraphicsUnit.Pixel);
                // Рисуем землю
                g.FillRectangle(new SolidBrush(Color.FromArgb(150, Color.DarkGreen)),
                    0, picDisplay.Height - 20, picDisplay.Width, 20);

                lblScore.Text = $"Очки: {score}";

                gun.DrawGun(g);

                foreach (var emitter in emitters)
                {
                    emitter.Render(g);
                }
            }

            picDisplay.Invalidate();
        }

        private void UpdateState()
        {
            // Обновляем только пули (ракеты обновляются в своем темпе)
            gun.UpdateState();
            topEmitter.UpdateState();

            foreach (var bullet in gun.particles)
            {
                if (bullet.Life <= 0) continue;

                foreach (var target in topEmitter.particles.OfType<TargetParticle>())
                {
                    if (target.Life <= 0) continue;

                    // Более точное определение столкновения
                    float dx = bullet.X - target.X;
                    float dy = bullet.Y - target.Y;
                    float minDistance = bullet.Radius + target.Radius;

                    if (dx * dx + dy * dy < minDistance * minDistance)
                    {
                        target.HitCount++;
                        score += 10;

                        // Пуля наносит больше урона
                        if (target.HitCount >= target.HitsToDestroy)
                        {
                            target.Life = 0;
                            score += 50;
                        }

                        // Пуля не исчезает сразу после попадания
                        bullet.Life -= 20; // Теряет часть жизни
                        if (bullet.Life <= 0) break;
                    }
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
