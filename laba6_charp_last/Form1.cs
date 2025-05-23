﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace laba6_charp_last
{
    public partial class Form1 : Form
    {
        private Bitmap highQualityBackground;
        private List<Emitter> emitters = new List<Emitter>();
        private GunEmitter gun;
        private TopEmitter topEmitter;
        private ExplosionEmitter explosionEmitter;
        private int score = 0;
        private int lives = 5;
        private int rocketsDestroyed = 0;
        private int winCondition = 20;
        private DateTime lastMeteorTime = DateTime.Now.AddSeconds(-30);
        private const double METEOR_INTERVAL_SECONDS = 30.0;
        private List<Bitmap> lifeIcons = new List<Bitmap>();
        private bool gameStarted = false;

        public Form1()
        {
            InitializeComponent();
            InitializeLifeIcons();
            InitializeBackground();
            InitializeGameElements();
            SetupGameModeSelection();
        }

        private void InitializeLifeIcons()
        {
            for (int i = 0; i < 5; i++)
            {
                var bmp = new Bitmap(30, 30);
                using (var g = Graphics.FromImage(bmp))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.FillEllipse(Brushes.Red, 0, 0, 30, 30);
                    g.DrawEllipse(new Pen(Color.DarkRed, 2), 0, 0, 30, 30);
                }
                lifeIcons.Add(bmp);
            }
        }

        private void InitializeBackground()
        {
            try
            {
                var originalBg = Image.FromFile("Resources/fon2.jpg");
                highQualityBackground = new Bitmap(originalBg, picDisplay.Width, picDisplay.Height);
                originalBg.Dispose();
            }
            catch
            {
                highQualityBackground = new Bitmap(picDisplay.Width, picDisplay.Height);
                using (var g = Graphics.FromImage(highQualityBackground))
                {
                    g.Clear(Color.DarkBlue);
                    using (var brush = new LinearGradientBrush(
                        new Point(0, 0),
                        new Point(picDisplay.Width, picDisplay.Height),
                        Color.DarkBlue,
                        Color.Black))
                    {
                        g.FillRectangle(brush, 0, 0, picDisplay.Width, picDisplay.Height);
                    }

                    var rnd = new Random();
                    for (int i = 0; i < 200; i++)
                    {
                        int size = rnd.Next(1, 3);
                        g.FillEllipse(Brushes.White,
                            rnd.Next(picDisplay.Width),
                            rnd.Next(picDisplay.Height),
                            size, size);
                    }
                }
            }
        }

        private void InitializeGameElements()
        {
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);
            picDisplay.MouseMove += picDisplay_MouseMove;

            gun = new GunEmitter
            {
                Direction = 90,
                Spreading = 5,
                SpeedMin = 15,
                SpeedMax = 20,
                ColorFrom = Color.Yellow,
                //FireRate = 3,
                ParticlesCount = 200,
                X = picDisplay.Width / 2,
                Y = picDisplay.Height - 30,
            };

            topEmitter = new TopEmitter
            {
                Width = picDisplay.Width,
                ParticlesPerTick = 1,
                ParticlesCount = 5,
                ColorFrom = Color.LightBlue,
                ColorTo = Color.FromArgb(0, Color.Blue),
                SpeedMin = 1,
                SpeedMax = 2,
                RadiusMin = 15,
                RadiusMax = 25,
                LifeMin = 300,
                LifeMax = 500,
                HitsToDestroyMin = 2,
                HitsToDestroyMax = 5,
                GravitationY = 0
            };

            explosionEmitter = new ExplosionEmitter
            {
                ParticlesCount = 15
            };

            emitters.Add(gun);
            emitters.Add(topEmitter);
            emitters.Add(explosionEmitter);

            InitializeTrackBars();
        }

        private void SetupGameModeSelection()
        {
            btnEasy.Tag = 20;
            btnMedium.Tag = 30;
            btnHard.Tag = 40;

            btnEasy.Click += GameMode_Click;
            btnMedium.Click += GameMode_Click;
            btnHard.Click += GameMode_Click;

            btnStart.Click += btnStart_Click;
            btnRestart.Click += btnRestart_Click;

            btnStart.Enabled = false;
        }

        private void GameMode_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            winCondition = (int)clickedButton.Tag;
            btnStart.Enabled = true;

            // Сброс цвета всех кнопок
            btnEasy.BackColor = SystemColors.Control;
            btnMedium.BackColor = SystemColors.Control;
            btnHard.BackColor = SystemColors.Control;

            // Установка цвета выбранной кнопки
            clickedButton.BackColor = Color.LightGreen;
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            StartNewGame();
        }

        private void InitializeTrackBars()
        {
            trackBarSpeed.Minimum = 1;
            trackBarSpeed.Maximum = 10;
            trackBarSpeed.Value = 3;
            trackBarSpeed.ValueChanged += TrackBarSpeed_ValueChanged;

            trackBarHits.Minimum = 1;
            trackBarHits.Maximum = 10;
            trackBarHits.Value = 3;
            trackBarHits.ValueChanged += TrackBarHits_ValueChanged;

            /*trackBarFireRate.Minimum = 1;
            trackBarFireRate.Maximum = 10;
            trackBarFireRate.Value = 3;
            trackBarFireRate.ValueChanged += TrackBarFireRate_ValueChanged;*/
        }

        private void TrackBarFireRate_ValueChanged(object sender, EventArgs e)
        {
            /*gun.FireRate = 6 - trackBarFireRate.Value;
            labelFireRate.Text = $"Скорострельность: {trackBarFireRate.Value}";
        */
        }

        private void TrackBarSpeed_ValueChanged(object sender, EventArgs e)
        {
            topEmitter.SpeedMin = trackBarSpeed.Value;
            topEmitter.SpeedMax = trackBarSpeed.Value + 2;
            labelSpeed.Text = $"Скорость: {trackBarSpeed.Value}";
        }

        private void TrackBarHits_ValueChanged(object sender, EventArgs e)
        {
            topEmitter.HitsToDestroyMin = trackBarHits.Value;
            topEmitter.HitsToDestroyMax = trackBarHits.Value + 3;
            labelHits.Text = $"Ударов для уничтожения: {trackBarHits.Value}";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!gameStarted) return;
            if (CheckGameOver()) return;

            SpawnMeteorIfNeeded();
            UpdateGameState();
            RenderGame();
        }

        private bool CheckGameOver()
        {
            if (lives <= 0)
            {
                gameStarted = false;
                timer1.Enabled = false;
                MessageBox.Show("Игра окончена! Вы проиграли.", "Результат");
                btnStart.Enabled = true;
                return true;
            }

            if (rocketsDestroyed >= winCondition)
            {
                gameStarted = false;
                timer1.Enabled = false;
                MessageBox.Show($"Поздравляем! Вы выиграли с счетом {score}!", "Результат");
                btnStart.Enabled = true;
                return true;
            }

            return false;
        }

        private void SpawnMeteorIfNeeded()
        {
            if ((DateTime.Now - lastMeteorTime).TotalSeconds >= METEOR_INTERVAL_SECONDS)
            {
                lastMeteorTime = DateTime.Now;
                CreateMeteor();
            }
        }

        private void CreateMeteor()
        {
            var meteor = new MeteorParticle
            {
                FromColor = Color.White,
                ToColor = Color.FromArgb(0, Color.DarkGray),
                X = Particle.rand.Next(picDisplay.Width),
                Y = -50,
                SpeedX = (float)(Particle.rand.NextDouble() - 0.5) * 1f,
                SpeedY = Particle.rand.Next(1, 3) * 0.3f,
                Life = 1000,
                HitsToDestroy = 5 + Particle.rand.Next(6),
                HitCount = 0
            };

            topEmitter.particles.Add(meteor);
        }

        private void UpdateGameState()
        {
            ((GunEmitter)gun).UpdateState();
            topEmitter.UpdateState();

            CheckBulletCollisions();
            CheckRocketLandings();
            explosionEmitter.UpdateState();
        }

        private void CheckBulletCollisions()
        {
            foreach (var bullet in gun.particles.ToList())
            {
                if (bullet.Life <= 0) continue;

                foreach (var target in topEmitter.particles.OfType<TargetParticle>())
                {
                    if (target.Life <= 0) continue;

                    if (CheckCollision(bullet, target))
                    {
                        target.HitCount++;
                        score += target is MeteorParticle ? 20 : 10;

                        if (target.HitCount >= target.HitsToDestroy)
                        {
                            target.Life = 0;
                            if (target is MeteorParticle meteor)
                            {
                                meteor.IsDestroyed = true;
                                score += 100;
                                CreateBigExplosion(meteor.X, meteor.Y, Color.Orange);
                                ((GunEmitter)gun).ActivateUpgrade();
                            }
                            else
                            {
                                rocketsDestroyed++;
                                CreateSmallExplosion(target.X, target.Y, Color.LightBlue);
                            }
                        }
                        bullet.Life = 0;
                        break;
                    }
                }
            }
        }

        private void CreateBigExplosion(float x, float y, Color color)
        {
            explosionEmitter.ParticlesCount = 60;
            explosionEmitter.CreateExplosion(x, y, color);
            explosionEmitter.ParticlesCount = 30;
        }

        private void CreateSmallExplosion(float x, float y, Color color)
        {
            explosionEmitter.CreateExplosion(x, y, color);
        }

        private bool CheckCollision(Particle a, Particle b)
        {
            float dx = a.X - b.X;
            float dy = a.Y - b.Y;
            float minDistance = a.Radius + b.Radius;
            return dx * dx + dy * dy < minDistance * minDistance;
        }

        private void CheckRocketLandings()
        {
            foreach (var particle in topEmitter.particles.ToList())
            {
                if (particle.Y >= picDisplay.Height - 30 && particle.Life > 0)
                {
                    HandleParticleLanding(particle);
                }
            }
        }

        private void HandleParticleLanding(Particle particle)
        {
            if (particle is TargetParticle && !(particle is MeteorParticle))
            {
                lives--;
                explosionEmitter.CreateExplosion(particle.X, picDisplay.Height - 30, Color.Red);
            }
            else if (particle is MeteorParticle meteor && !meteor.IsDestroyed)
            {
                explosionEmitter.CreateExplosion(meteor.X, picDisplay.Height - 30, Color.DarkOrange);
            }
            particle.Life = 0;
        }

        private void RenderGame()
        {
            using (var g = Graphics.FromImage(picDisplay.Image))
            {
                g.Clear(Color.Transparent);
                DrawBackground(g);
                DrawGround(g);
                DrawScore(g);
                DrawLives(g);
                DrawRocketCounter(g);
                gun.DrawGun(g);

                foreach (var emitter in emitters)
                {
                    emitter.Render(g);
                }
            }

            picDisplay.Invalidate();
        }

        private void DrawBackground(Graphics g)
        {
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;

            g.DrawImage(highQualityBackground,
                       new Rectangle(0, 0, picDisplay.Width, picDisplay.Height),
                       new Rectangle(0, 0, highQualityBackground.Width, highQualityBackground.Height),
                       GraphicsUnit.Pixel);
        }

        private void DrawGround(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.FromArgb(150, Color.DarkGreen)),
                0, picDisplay.Height - 20, picDisplay.Width, 20);
        }

        private void DrawScore(Graphics g)
        {
            lblScore.Text = $"Очки: {score}";
        }

        private void DrawLives(Graphics g)
        {
            for (int i = 0; i < lives; i++)
            {
                g.DrawImage(lifeIcons[i], picDisplay.Width - 40 - i * 35, 10);
            }
        }

        private void DrawRocketCounter(Graphics g)
        {
            g.DrawString($"Ракет: {rocketsDestroyed}/{winCondition}",
                new Font("Arial", 12), Brushes.White, 10, 10);
        }

        private void picDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            gun.X = e.X;

            float dx = e.X - (gun.X);
            float dy = e.Y - (gun.Y - 30);

            float length = (float)Math.Sqrt(dx * dx + dy * dy);
            if (length > 0)
            {
                dx /= length;
                dy /= length;
            }

            gun.Direction = (float)(Math.Atan2(-dy, dx) * 180 / Math.PI);
            picDisplay.Invalidate();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartNewGame();
        }

        private void StartNewGame()
        {
            gameStarted = true;
            timer1.Enabled = true;
            btnStart.Enabled = false;
            score = 0;
            lives = 5;
            rocketsDestroyed = 0;
            lastMeteorTime = DateTime.Now.AddSeconds(-25); // Первый метеорит появится через 5 секунд
            lblScore.Text = "Очки: 0";

            gun.particles.Clear();
            topEmitter.particles.Clear();
            explosionEmitter.particles.Clear();
            ((GunEmitter)gun).IsUpgraded = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblScore.Text = "Очки: 0";
        }

        private void picDisplay_Click(object sender, EventArgs e)
        {
            // Обработка клика по игровому полю
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            //lblScore.Text = "Очки: 0";
        }
    }
}