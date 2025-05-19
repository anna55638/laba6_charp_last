using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba6_charp_last
{
    public class GunEmitter : Emitter
    {
        public float X;
        public float Y;
        public float Direction = 0;
        public int Spreading = 10;
        public int SpeedMin = 5;  // Уменьшенная скорость
        public int SpeedMax = 10;
        public int RadiusMin = 2;
        public int RadiusMax = 5;
        public Color ColorFrom = Color.Yellow;
        public Color ColorTo = Color.FromArgb(0, Color.Orange);
        public static Image GunImage; // Изображение пушки
        public int FireRate = 3; // Скорострельность (частиц/тик)

        static GunEmitter()
        {
            try
            {
                var originalImage = Image.FromFile("Resources/cosmo556.jpg");
                GunImage = new Bitmap(originalImage, new Size(70, 60)); // Размер пушки
                originalImage.Dispose();
            }
            catch
            {
                GunImage = new Bitmap(40, 40);
                using (var g = Graphics.FromImage(GunImage))
                {
                    g.FillRectangle(Brushes.Gray, 0, 0, 40, 40);
                }
            }
        }

        public override Particle CreateParticle()
        {
            return new ParticleColorful
            {
                FromColor = ColorFrom,
                ToColor = ColorTo,
                Life = 100, // Увеличиваем время жизни
                Radius = 8  // Увеличиваем размер
            };
        }

        public override void ResetParticle(Particle particle)
        {
            particle.Life = 100; // Увеличенное время жизни
            particle.X = X;
            particle.Y = Y - 15;
            particle.Radius = 6; // Увеличенный размер

            double angle = (Direction + Particle.rand.Next(Spreading) - Spreading / 2) * Math.PI / 180;
            float speed = Particle.rand.Next(SpeedMin, SpeedMax);

            particle.SpeedX = (float)(Math.Cos(angle) * speed);
            particle.SpeedY = -(float)(Math.Sin(angle) * speed);

            // Убираем гравитацию для пуль
            this.GravitationY = 0;
        }

        public void DrawGun(Graphics g)
        {
            // Сохраняем исходное состояние графики
            GraphicsState state = g.Save();

            try
            {
                // 1. Переносим точку вращения в центр пушки
                g.TranslateTransform(X, Y);

                // 2. Поворачиваем вокруг центра (учитываем, что Direction=0 должно быть вправо)
                g.RotateTransform(-Direction + 90); // +90 чтобы 0 градусов было вверх

                // 3. Рисуем изображение с центром в точке вращения
                g.DrawImage(
                    GunImage,
                    -GunImage.Width / 2,
                    -GunImage.Height / 2,
                    GunImage.Width,
                    GunImage.Height
                );
            }
            finally
            {
                // Восстанавливаем исходное состояние
                g.Restore(state);
            }
        }
    }
}
